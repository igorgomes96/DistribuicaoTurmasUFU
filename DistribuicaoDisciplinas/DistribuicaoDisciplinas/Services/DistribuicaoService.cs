using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Services
{
    public class DistribuicaoService : IDistribuicaoService
    {
        #region Constantes
        //Constante que define o quanto a CH do professor pode ser ultrapassada, no máximo
        const int ACRESCIMO_CH = 2;
        #endregion

        #region Private Properties
        private ICollection<FilaTurma> filasTurmas;
        private IDictionary<string, Professor> professores;
        private IDictionary<int, Turma> turmas;
        private Cenario cenario;
        #endregion

        #region Repositories
        private readonly IGenericRepository<FilaTurmaEntity> _filasTurmasRep;
        #endregion

        #region Services
        private readonly IProfessoresService _professoresService;
        private readonly ITurmasService _turmasService;
        private readonly ICenariosService _cenariosService;
        private readonly IMinistraService _ministraService;
        #endregion

        #region Map
        private readonly IMapper<Turma, TurmaDto> _turmaMapper;
        private readonly IMapper<Bloqueio, BloqueioDto> _bloqueioMapper;
        private readonly IMapper<Ministra, FilaTurma> _ministraFTMapper;
        private readonly IMapper<Professor, ProfessorDto> _professorMapper;
        private readonly IMapper<FilaTurma, FilaTurmaDto> _filaTurmaMapper;
        #endregion

        #region Constructor
        public DistribuicaoService(
            IGenericRepository<FilaTurmaEntity> filasTurmasRep,
            IProfessoresService professoresService,
            ITurmasService turmasService,
            ICenariosService cenariosService,
            IMinistraService ministraService,
            IMapper<Turma, TurmaDto> turmaMapper,
            IMapper<Bloqueio, BloqueioDto> bloqueioMapper,
            IMapper<Ministra, FilaTurma> ministraFTMapper,
            IMapper<Professor, ProfessorDto> professorMapper,
            IMapper<FilaTurma, FilaTurmaDto> filaTurmaMapper)
        {
            _professoresService = professoresService;
            _turmasService = turmasService;
            _cenariosService = cenariosService;
            _ministraService = ministraService;
            _filasTurmasRep = filasTurmasRep;
            _turmaMapper = turmaMapper;
            _bloqueioMapper = bloqueioMapper;
            _ministraFTMapper = ministraFTMapper;
            _professorMapper = professorMapper;
            _filaTurmaMapper = filaTurmaMapper;

            //Instancia um novo Set de FilasTurmas
            filasTurmas = new HashSet<FilaTurma>();
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Faz o encadeamento de objetos.
        /// </summary>
        /// <param name="filasTurmasEntities"></param>
        /// <returns></returns>
        private ICollection<FilaTurma> Encadear(ICollection<FilaTurmaEntity> filasTurmasEntities)
        {
            //Carrega todos os professores
            professores = _professoresService.List().ToDictionary(p => p.Siape.Trim());

            //Carrega todas as turmas ofertadas no semestre
            turmas = _turmasService.List(cenario.Ano, cenario.Semestre).ToDictionary(t => t.Id);
            IDictionary<string, Disciplina> disciplinas = turmas.Values.Select(t => t.Disciplina)
                .Distinct().ToDictionary(d => d.Codigo.Trim());

            //Atualiza a referência das disciplinas das turmas, para que as turmas de uma mesma disciplina aponte para a mesma instância da discplina
            foreach (Turma t in turmas.Values)
                t.Disciplina = disciplinas[t.CodigoDisc.Trim()];

            //Carrega todas as filas da collection passada por parâmetro
            IDictionary<int, Fila> filas = filasTurmasEntities
                .Select(ft =>
                {
                    return new Fila
                    {
                        Id = ft.id_fila,
                        Professor = professores[ft.Fila.siape.Trim()],
                        Disciplina = disciplinas[ft.Fila.codigo_disc.Trim()],
                        Posicao = ft.Fila.pos.Value,
                        QtdaMaxima = ft.Fila.qte_maximo.Value,
                        QtdaMinistrada = ft.Fila.qte_ministrada.Value
                    };
                }).Distinct().ToDictionary(f => f.Id);

            //Atualiza os ponteiros de filasTurmas
            filasTurmasEntities.Select(ft =>
            {
                return new FilaTurma
                {
                    Fila = filas[ft.id_fila],
                    Turma = turmas[ft.id_turma],
                    Prioridade = ft.prioridade.Value
                };
            }).ToList().ForEach(ft =>
            {
                filasTurmas.Add(ft);
            });

            List<FilaTurma> optativas = GetOptativas().ToList();
            optativas.ForEach(op =>
            {
                op.Turma = turmas[op.Turma.Id];
                op.Fila.Disciplina = disciplinas[op.Turma.CodigoDisc];
                op.Fila.Professor = professores[op.Fila.Professor.Siape];
                filasTurmas.Add(op);
            });

            //Atualiza prioridades dos professores
            filasTurmas.OrderBy(ft => ft.Fila.QtdaMaximaJaMinistrada).ThenBy(ft => ft.Prioridade) //Ordena por prioridade, colocando as filas onde o professor já ministrou a qtda máxima de vezes a turma
                .ToList().ForEach(ft => ft.Fila.Professor.Prioridades.Add(ft));

            //Atualiza posições das turmas
            filasTurmas.OrderBy(ft => ft.Fila.Posicao)
                .ToList().ForEach(ft => ft.Turma.Posicoes.Add(ft));

            return filasTurmas;

        }

        public ICollection<FilaTurma> GetOptativas()
        {
            ICollection<Ministra> ministraOptativas = _ministraService.List(cenario.Ano, cenario.Semestre);
            ICollection<FilaTurma> filasTurmasOptativas = _ministraFTMapper.Map(ministraOptativas).ToList();

            filasTurmasOptativas.ToList().ForEach(ft => {
                    ft.StatusAlgoritmo = StatusFila.Atribuida;
                });

            return filasTurmasOptativas;

        }

        /// <summary>
        /// Altera o status das FilasTurmas que com certeza não serão atribuídas ao professor,
        /// pois as turmas com maior prioridade completarão sua CH.
        /// </summary>
        private void LimpezaInicial()
        {
            foreach (Professor prof in professores.Values)
            {
                List<FilaTurma> turmasAtribuidas = new List<FilaTurma>();
                bool flagCHCompleta = false;

                foreach (FilaTurma ft in prof.Prioridades.Where(x => x.StatusAlgoritmo != StatusFila.ChoqueRestricao))
                {
                    if (flagCHCompleta)
                    {
                        ft.StatusAlgoritmo = StatusFila.Desconsiderada;
                    }
                    else
                    {
                        if (ft.Turma.Posicoes.FirstOrDefault().Equals(ft))
                        {
                            if (!turmasAtribuidas
                                .Any(x => _turmasService.ChoqueHorario(x.Turma, ft.Turma)
                                    || _turmasService.ChoquePeriodo(x.Turma, ft.Turma)))
                            {
                                turmasAtribuidas.Add(ft);
                            }
                        }

                        if (turmasAtribuidas.Select(x => x.Turma.CH).Sum() >= prof.CH)
                            flagCHCompleta = true;
                    }
                }
            }
        }

        /// <summary>
        /// Atualiza o status das turmas com restrição.
        /// </summary>
        private void TurmasComRestricao()
        {
            filasTurmas.Where(x => x.Turma.Horarios.Any(h => x.Fila.Professor.Restricoes.Any(r => r.Dia == h.Dia && r.Letra == h.Letra)))
                .ToList().ForEach(x => x.StatusAlgoritmo = StatusFila.ChoqueRestricao);
        }

        /// <summary>
        /// Para todos os professores que já estão com a CH preenchida, atualiza as prioridades EmEspera e NaoAnalisadasAinda
        /// para CHCompleta.
        /// </summary>
        private void AtualizaPrioridadesCHCompleta()
        {
            filasTurmas
                .Where(ft => ft.Fila.Professor.CHCompletaAtribuida() && (ft.StatusAlgoritmo == StatusFila.EmEspera ||
                    ft.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda))
                .ToList()
                .ForEach(ft => {
                    ft.StatusAlgoritmo = StatusFila.CHCompleta;
                });
        }

        /// <summary>
        ///  Para o professor passado por parâmetro, atualiza as prioridades EmEspera e NaoAnalisadasAinda
        /// para CHCompleta.
        /// </summary>
        /// <param name="professor">Professor a ser atualizado</param>
        private void AtualizaPrioridadesCHCompleta(Professor professor)
        {
            professor.Prioridades
                .Where(ft => ft.StatusAlgoritmo == StatusFila.EmEspera ||
                    ft.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda)
                .ToList()
                .ForEach(ft => ft.StatusAlgoritmo = StatusFila.CHCompleta);
        }

        /// <summary>
        /// Faz a atribuição dos casos triviais (Turmas não analisadas ou em espera que são a próxima prioridade do professor)
        /// </summary>
        /// <returns>true, se houve alguma atribuição; false, se não.</returns>
        private bool CasosTriviais()
        {
            bool flagHouveAtribuicao = false;
            foreach (Professor p in professores.Values)
            {
                List<FilaTurma> possibilidadesProf = p.Prioridades
                    .Where(pp => pp.StatusAlgoritmo == StatusFila.EmEspera
                        || pp.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda)
                    .ToList();

                ICollection<Turma> prioridadesEmEspera = new List<Turma>();
                int chEmEspera = 0;

                foreach (FilaTurma filaTurma in possibilidadesProf)
                {
                    List<FilaTurma> possibilidadesTurma = filaTurma.Turma.Posicoes
                        .Where(pt => pt.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda
                            || pt.StatusAlgoritmo == StatusFila.EmEspera).ToList();

                    int chLimite = (p.CH + ACRESCIMO_CH);

                    if (possibilidadesTurma.FirstOrDefault().Equals(filaTurma) //Verifica se o professor está na primeira posição da turma
                        && !_turmasService.ChoqueHorario(filaTurma.Turma, prioridadesEmEspera)  //Verifica se a turma tem choque de horário com as turmas em espera
                        && !_turmasService.ChoquePeriodo(filaTurma.Turma, prioridadesEmEspera)) //Verifica se a turma tem choque de período com as turmas em espera
                    {

                        if ((p.CHAtribuida() + filaTurma.Turma.CH + chEmEspera) <= chLimite) {
                            AtribuirTurma(filaTurma);
                            flagHouveAtribuicao = true;

                            //Se a CH do professor já estiver completa e não tiver turmas em espera, atualiza o status 
                            //de todas as demais FilasTurmas do professor e passa para o próximo.
                            //A CH somente será completa tendo turmas em espera quando a soma da CH das turmas 
                            //em espera for igual a ACRESCIMO_CH
                            if (p.CHCompletaAtribuida() && chEmEspera <= 0)
                            {
                                AtualizaPrioridadesCHCompleta(p);
                                break;
                            }

                        } else if ((p.CHAtribuida() + filaTurma.Turma.CH) > chLimite && chEmEspera <= 0)
                        //Se ultrapassar a CH na atribuição, mas não tiver turmas em espera
                        //Analisar caso da Sara Luzia de Melo (1937166)
                        {
                            filaTurma.StatusAlgoritmo = StatusFila.UltrapassariaCH;
                        } else
                        {
                            prioridadesEmEspera.Add(filaTurma.Turma);
                            filaTurma.StatusAlgoritmo = StatusFila.EmEspera;
                        }
                    }
                    else
                    {
                        prioridadesEmEspera.Add(filaTurma.Turma);
                        filaTurma.StatusAlgoritmo = StatusFila.EmEspera;
                    }

                    chEmEspera = prioridadesEmEspera.Select(x => x.CH).Sum();
                    //Se o que já foi atribuído mais o que está em espera ultrapassar a CH limite do
                    //professor, posso passar para o próximo professsor
                    if ((p.CHAtribuida() + chEmEspera) >= chLimite) break;
                }
            }

            return flagHouveAtribuicao;
        }

        private RespostaDto GeraResposta(ICollection<Bloqueio> bloqueios)
        {
            ICollection<Turma> turmasAtribuidas = filasTurmas
                .Where(x => x.StatusAlgoritmo == StatusFila.Atribuida).Select(x => x.Turma)
                .Distinct()
                .ToList();

            return new RespostaDto {
                Professores = _professorMapper.Map(professores.Values).OrderBy(x => x.Nome).ToList(),
                TurmasPendentes = turmas.Values.Where(t => !turmasAtribuidas.Any(x => x.Id == t.Id))
                    .Select(x => x.Id).ToList(),
                Turmas = _turmaMapper.Map(turmas.Values),
                FilasTurmas = _filaTurmaMapper.Map(filasTurmas),
                Bloqueios = _bloqueioMapper.Map(bloqueios).OrderBy(x => x.Tamanho).ToList()
            };

        }

        /// <summary>
        /// Identifica e retorna todos os deadlocks
        /// </summary>
        /// <returns>Lista de todos os deadlocks</returns>
        private ICollection<Bloqueio> GetTodosDeadlocks()
        {
            ICollection<Bloqueio> deadlocks = new List<Bloqueio>();
            int i = 1;
            //Buscas os deadlocks de cada professor que tem turma em espera
            ICollection<Professor> professoresPendentes = professores.Values
                .Where(p => p.Prioridades.Any(pri => pri.StatusAlgoritmo == StatusFila.EmEspera)).ToList();

            foreach(Professor p in professoresPendentes)
            {
                Trace.WriteLine(string.Format("\nDeadlock {0}:", i++));
                Bloqueio deadlock = GetDeadlock(p);
                if (deadlock != null && !deadlocks.Any(x => x.FilaTurma.Equals(deadlock.FilaTurma)))
                    deadlocks.Add(deadlock);
                
            }

            Trace.WriteLine("\n***********************************************\n");
            i = 0;
            foreach (Bloqueio deadlock in deadlocks) {
                Trace.WriteLine(string.Format("\nDeadlock {0}:", i++));
                PrintDeadlock(deadlock);
            }

            return deadlocks;
        }

        private void PrintDeadlock(Bloqueio bloqueio)
        {
            if (bloqueio == null || bloqueio.FilaTurma == null) return;
            do
            {
                Professor professor = bloqueio.FilaTurma.Fila.Professor;
                Trace.Write(professor.Nome + "(" + professor.Siape + ") -> " + bloqueio.FilaTurma.Turma.CodigoDisc);

                if (bloqueio.Dependente != null)
                {
                    professor = bloqueio.Dependente.FilaTurma.Fila.Professor;
                    Trace.WriteLine(" -> " + professor.Nome + "(" + professor.Siape + ")\n");
                }

                bloqueio = bloqueio.Dependente;
            } while (bloqueio != null);
        }

        /// <summary>
        /// Encontra toda a cadeia de deadlock a partir de um professor
        /// </summary>
        /// <param name="professor"></param>
        /// <returns></returns>
        private Bloqueio GetDeadlock(Professor professor)
        {
            HashSet<FilaTurma> trace = new HashSet<FilaTurma>();
            FilaTurma ftCabeca = professor.Prioridades
                            .FirstOrDefault(ft => ft.StatusAlgoritmo == StatusFila.EmEspera);

            if (ftCabeca == null) return null;
            trace.Add(ftCabeca);

            Bloqueio cabeca = new Bloqueio
            {
                FilaTurma = ftCabeca,
                TipoBloqueio = TipoBloqueio.Deadlock
            };
            Bloqueio ultimoBloqueio = cabeca;

            Trace.Write(professor.Nome + "(" + professor.Siape  + ") -> " + ftCabeca.Turma.CodigoDisc);

            for (; ; )
            {

                professor = ultimoBloqueio.FilaTurma.Turma.Posicoes
                    .FirstOrDefault(x => x.StatusAlgoritmo == StatusFila.EmEspera 
                        || x.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda).Fila.Professor;

                Trace.WriteLine(" -> " + professor.Nome + "(" + professor.Siape + ")");

                FilaTurma ftBloqueada = professor.Prioridades
                            .FirstOrDefault(ft => ft.StatusAlgoritmo == StatusFila.EmEspera);

                Trace.Write(professor.Nome + "(" + professor.Siape + ") -> " + ftBloqueada.Turma.CodigoDisc);

                if (ftBloqueada == null) break;

                Bloqueio bloqueio = new Bloqueio
                {
                    FilaTurma = ftBloqueada,
                    TipoBloqueio = TipoBloqueio.Deadlock
                };

                ultimoBloqueio.Dependente = bloqueio;

                if (trace.Contains(ftBloqueada))
                {
                    Bloqueio b = cabeca;
                    while (!b.FilaTurma.Equals(ftBloqueada)) { b = b.Dependente; }
                    cabeca = b;
                    break;
                }
                    

                trace.Add(ftBloqueada);
                ultimoBloqueio = bloqueio;

            }

            Trace.WriteLine("");
            return cabeca;
        }

        /// <summary>
        /// Atualiza os status das FilasTurmas do professor e verifica se alguma turma não analisada choca com a turma atribuída
        /// </summary>
        /// <param name="filaTurma"></param>
        private void AtribuirTurma(FilaTurma filaTurma)
        {
            //Atualiza o status de todas as filas da turma que foi atribuída para OutroProfessor
            filaTurma.Turma.Posicoes
                .Where(pt => pt.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda
                    || pt.StatusAlgoritmo == StatusFila.EmEspera).ToList()
                .ForEach(ft => ft.StatusAlgoritmo = StatusFila.OutroProfessor);

            //Atualiza o status das FilasTurmas que chocam horário ou período com a turma atribuída
            foreach (FilaTurma prioridade in filaTurma.Fila.Professor.Prioridades
                .Where(x => x.StatusAlgoritmo == StatusFila.EmEspera || x.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda))
            {
                if (!prioridade.Equals(filaTurma))
                {
                    if (_turmasService.ChoqueHorario(filaTurma.Turma, prioridade.Turma))
                        prioridade.StatusAlgoritmo = StatusFila.ChoqueHorario;
                    else if (_turmasService.ChoquePeriodo(filaTurma.Turma, prioridade.Turma))
                        prioridade.StatusAlgoritmo = StatusFila.ChoquePeriodo;
                }
            }

            //Atualiza o status da Fila atribuída
            filaTurma.StatusAlgoritmo = StatusFila.Atribuida;
        }
        #endregion

        #region Public Methods
        public RespostaDto Distribuir(int numCenario)
        {
            cenario = _cenariosService.Find(numCenario);
            ICollection<FilaTurmaEntity> filasTurmasEntities = _filasTurmasRep
                .Query(ft => ft.Turma.ano == cenario.Ano
                    && ft.Turma.semestre == cenario.Semestre
                    && ft.Fila.ano == cenario.Ano
                    && ft.Fila.semestre == cenario.Semestre);

            GetOptativas();

            filasTurmas = Encadear(filasTurmasEntities);

            //Atualiza o status de todas para NaoAnalisadaAinda
            filasTurmas
                .Where(x => x.StatusAlgoritmo != StatusFila.Atribuida).ToList()
                .ForEach(x => x.StatusAlgoritmo = StatusFila.NaoAnalisadaAinda);

            TurmasComRestricao();

            AtualizaPrioridadesCHCompleta();

            //LimpezaInicial();

            while (CasosTriviais())
            {
            };

            return GeraResposta(GetTodosDeadlocks());
        }
        #endregion

    }
}