using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Exceptions;
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
        //private readonly IGenericRepository<FilaTurmaEntity> _filasTurmasRep;
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
                        PosicaoReal = ft.Fila.pos.Value,
                        QtdaMaxima = ft.Fila.qte_maximo.Value,
                        QtdaMinistrada = ft.Fila.qte_ministrada.Value
                    };
                }).Distinct().ToDictionary(f => f.Id);

            //Atualiza os ponteiros de filasTurmas
            filasTurmasEntities.Select(ft =>
            {
                return new FilaTurma
                {
                    IdTurma = ft.id_turma,
                    IdFila = ft.id_fila,
                    Fila = filas[ft.id_fila],
                    Turma = turmas[ft.id_turma],
                    PrioridadeReal = ft.prioridade.Value,
                    PrioridadeBanco = ft.prioridade.Value
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
            /*filasTurmas.OrderBy(ft => ft.Fila.QtdaMaximaJaMinistrada).ThenBy(ft => ft.PrioridadeReal) //Ordena por prioridade, colocando no final as filas onde o professor já ministrou a qtda máxima de vezes a turma
                .ToList().ForEach(ft => ft.Fila.Professor.Prioridades.Add(ft));*/
            filasTurmas.ToList()
                .ForEach(ft => ft.Fila.Professor.Prioridades.Add(ft));

            //Atualiza posições das turmas
            filasTurmas.ToList()
                .ForEach(ft => ft.Turma.Posicoes.Add(ft));

            return filasTurmas;

        }

        /// <summary>
        /// Ordena as prioridades de cada professor e as posições de cada turma
        /// </summary>
        private void OrdenaPrioridadesPosicoes()
        {
            foreach (Professor p in professores.Values)
                p.OrdenaPrioridades();

            foreach (Turma t in turmas.Values)
                t.OrdenaPosicoes();

        }

        /// <summary>
        /// Lê os dados que estão na tabela ministra, e cria instâncias de FilaTurma pra cada registro
        /// </summary>
        /// <returns>Collection de FilaTurma</returns>
        public ICollection<FilaTurma> GetOptativas()
        {
            ICollection<Ministra> ministraOptativas = _ministraService.List(cenario.Ano, cenario.Semestre);
            ICollection<FilaTurma> filasTurmasOptativas = _ministraFTMapper.Map(ministraOptativas).ToList();

            filasTurmasOptativas.ToList().ForEach(ft =>
            {
                ft.StatusAlgoritmo = StatusFila.Atribuida;
            });

            return filasTurmasOptativas;

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
        private void AtualizaStatusCHCompleta()
        {
            filasTurmas
                .Where(ft => ft.Fila.Professor.CHCompletaAtribuida() && (ft.StatusAlgoritmo == StatusFila.EmEspera ||
                    ft.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda))
                .ToList()
                .ForEach(ft =>
                {
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

                    if (possibilidadesTurma.Count <= 0)
                        break;

                    int chLimite = (p.CH + ACRESCIMO_CH);

                    if (possibilidadesTurma.FirstOrDefault().Equals(filaTurma) //Verifica se o professor está na primeira posição da turma
                        && !_turmasService.ChoqueHorario(filaTurma.Turma, prioridadesEmEspera)  //Verifica se a turma tem choque de horário com as turmas em espera
                        && !_turmasService.ChoquePeriodo(filaTurma.Turma, prioridadesEmEspera)) //Verifica se a turma tem choque de período com as turmas em espera
                    {

                        if ((p.CHAtribuida() + filaTurma.Turma.CH + chEmEspera) <= chLimite)
                        {
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

                        }
                        else if ((p.CHAtribuida() + filaTurma.Turma.CH) > chLimite && chEmEspera <= 0)
                        //Se ultrapassar a CH na atribuição, mas não tiver turmas em espera
                        //Analisar caso da Sara Luzia de Melo (1937166)
                        {
                            filaTurma.StatusAlgoritmo = StatusFila.UltrapassariaCH;
                        }
                        else
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

        /// <summary>
        /// Faz a transformação das filas turmas em RespostaDto.
        /// </summary>
        /// <param name="bloqueios"></param>
        /// <returns></returns>
        private RespostaDto GeraResposta(ICollection<Bloqueio> bloqueios)
        {
            ICollection<Turma> turmasAtribuidas = filasTurmas
                .Where(x => x.StatusAlgoritmo == StatusFila.Atribuida).Select(x => x.Turma)
                .Distinct()
                .ToList();

            RespostaDto resposta = new RespostaDto
            {
                Professores = _professorMapper.Map(professores.Values).OrderBy(x => x.Nome).ToList(),
                TurmasPendentes = turmas.Values.Where(t => t.TurmaPendente())
                    .Select(x => x.Id).ToList(),
                Turmas = _turmaMapper.Map(turmas.Values),
                FilasTurmas = _filaTurmaMapper.Map(filasTurmas),
                Bloqueios = _bloqueioMapper.Map(bloqueios).OrderBy(x => x.Tamanho).ToList()
            };

            //SalvaResposta(resposta);

            return resposta;

        }

        /// <summary>
        /// Identifica e retorna todos os deadlocks
        /// </summary>
        /// <returns>Lista de todos os deadlocks</returns>
        private ICollection<Bloqueio> GetTodosDeadlocks()
        {
            ICollection<Bloqueio> deadlocks = new List<Bloqueio>();

            //Buscas os deadlocks de cada professor que tem turma em espera
            ICollection<Professor> professoresPendentes = professores.Values
                .Where(p => p.Prioridades.Any(pri => pri.StatusAlgoritmo == StatusFila.EmEspera
                    || pri.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda)).ToList();

            foreach (Professor p in professoresPendentes)
            {
                Bloqueio deadlock = GetDeadlock(p);
                //Verifico se esse deadlock já foi identificado. Para isso,
                //basta verificar se a FilaTurma cabeça do deadlock já existe
                //em alqum dos deadlocks anteriores
                if (deadlock != null && !deadlocks.Any(x => x.Contains(deadlock.FilaTurma)))
                    deadlocks.Add(deadlock);

            }

            //Trace.WriteLine("\n***********************************************\n");
            //i = 0;
            //foreach (Bloqueio deadlock in deadlocks) {
            //    Trace.WriteLine(string.Format("\n\nDeadlock {0}:", i++));
            //    PrintDeadlock(deadlock);
            //}

            return deadlocks;
        }

        /// <summary>
        /// Printa um deadlock.
        /// </summary>
        /// <param name="bloqueio"></param>
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
                    Trace.WriteLine(" -> " + professor.Nome + "(" + professor.Siape + ")");
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

            for (; ; )
            {

                professor = ultimoBloqueio.FilaTurma.Turma.Posicoes
                    .FirstOrDefault(x => x.StatusAlgoritmo == StatusFila.EmEspera
                        || x.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda).Fila.Professor;

                FilaTurma ftBloqueada = professor.Prioridades
                            .FirstOrDefault(ft => ft.StatusAlgoritmo == StatusFila.EmEspera);

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

            return cabeca;
        }

        /// <summary>
        /// Atualiza os status das FilasTurmas do professor e verifica se alguma turma não 
        /// analisada choca com a turma atribuída
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

        /// <summary>
        /// Atualiza os status de cada fila turma de acordo as filas turmas recebida por parâmetro.
        /// </summary>
        /// <param name="filasTurmasDto"></param>
        private void AtualizaStatus(ICollection<FilaTurmaDto> filasTurmasDto)
        {
            if (filasTurmasDto == null)
                return;

            foreach (FilaTurmaDto ft in filasTurmasDto)
            {
                FilaTurma filaTurma = filasTurmas
                    .FirstOrDefault(x => x.Fila.Id == ft.Fila.Id && x.Turma.Id == ft.IdTurma);

                if (filaTurma == null)
                    throw new FilaTurmaNaoEncontradaException();

                filaTurma.StatusAlgoritmo = ft.Status;
            }
        }

        /// <summary>
        /// Atualiza a propriedade CH de cada professor de acordo com a CH em um cenário específico.
        /// </summary>
        /// <param name="idCenario"></param>
        private void AtualizaCHProfessores(int idCenario)
        {
            foreach (Professor p in professores.Values)
                p.CH = p.CHCenario(idCenario);

        }

        /// <summary>
        /// Se for a primeira vez (filaTurmaDto vier vazio), joga para a última prioridade do professor cada
        /// turma cuja disciplina já foi ministrada por ele a quantidade máxima de vezes.
        /// Se estiver no meio da distribuição (filaTurmaDto não vier vazio), atualiza a prioridade com o que foi 
        /// recebido do front-end.
        /// </summary>
        /// <param name="filasTurmasDto"></param>
        private void AtualizaPrioridadesReais(ICollection<FilaTurmaDto> filasTurmasDto)
        {
            if (filasTurmasDto == null || filasTurmasDto.Count == 0)
            {
                foreach (FilaTurma ft in filasTurmas)
                {
                    if (ft.Fila.QtdaMaximaJaMinistrada)
                        ft.Fila.Professor.JogarParaUltimaPrioridadeReal(ft);
                }
            }
            else
            {
                foreach (FilaTurmaDto ft in filasTurmasDto)
                    filasTurmas.First(x => x.Fila.Id == ft.Fila.Id && x.Turma.Id == ft.IdTurma).PrioridadeReal = ft.PrioridadeReal;
            }
        }

        /// <summary>
        /// Atualiza as posições de acordo com o que foi recebido do front-end.
        /// </summary>
        /// <param name="filasTurmasDto"></param>
        private void AtualizaPosicoesReais(ICollection<FilaTurmaDto> filasTurmasDto)
        {
            if (filasTurmasDto != null && filasTurmasDto.Count > 0)
            {
                foreach (FilaTurmaDto ft in filasTurmasDto)
                    filasTurmas.First(x => x.Fila.Id == ft.Fila.Id && x.Turma.Id == ft.IdTurma).Fila.PosicaoReal = ft.Fila.PosicaoReal;
            }
        }

        /// <summary>
        /// Carrega as filas turmas do semestre, chama função de encadeamento e deixa as 
        /// propriedades privadas da classe prontas para a distribuição.
        /// </summary>
        /// <param name="numCenario"></param>
        /// <param name="filasTurmasDto"></param>
        private void PreparaDistribuicao(int numCenario, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            cenario = _cenariosService.Find(numCenario);
            ICollection<FilaTurmaEntity> filasTurmasEntities = _filasTurmasRep
                .Query(ft => ft.Turma.ano == cenario.Ano
                    && ft.Turma.semestre == cenario.Semestre
                    && ft.Fila.ano == cenario.Ano
                    && ft.Fila.semestre == cenario.Semestre);

            filasTurmas = Encadear(filasTurmasEntities);

            AtualizaPrioridadesReais(filasTurmasDto);
            AtualizaPosicoesReais(filasTurmasDto);
            OrdenaPrioridadesPosicoes();

            AtualizaCHProfessores(cenario.NumCenario);

            //Atualiza o status de todas para NaoAnalisadaAinda
            filasTurmas
                .Where(x => x.StatusAlgoritmo != StatusFila.Atribuida
                     && x.StatusAlgoritmo != StatusFila.Desconsiderada).ToList()
                .ForEach(x => x.StatusAlgoritmo = StatusFila.NaoAnalisadaAinda);

            TurmasComRestricao();

            AtualizaStatus(filasTurmasDto);

            AtualizaStatusCHCompleta();

        }

        /// <summary>
        /// Atualiza o status da FilaTurma, desde que seja diferente de atribuída
        /// </summary>
        /// <param name="filaTurma"></param>
        /// <param name="novoStatus"></param>
        private void Remover(FilaTurma filaTurma, StatusFila novoStatus)
        {
            if (novoStatus == StatusFila.Atribuida) return;
            filaTurma.StatusAlgoritmo = novoStatus;

            Professor professor = filaTurma.Fila.Professor;

            //Se não tiver a CH completa (já que uma turma foi removida),
            //altera o status das turmas = CHCompleta para EmEspera
            if (!professor.CHCompletaAtribuida())
            {
                professor.Prioridades.Where(p => p.Turma.TurmaPendente() && p.StatusAlgoritmo == StatusFila.CHCompleta)
                    .ToList().ForEach(p => p.StatusAlgoritmo = StatusFila.EmEspera);
            }

            ICollection<FilaTurma> choques = professor.Prioridades
                .Where(x => x.StatusAlgoritmo == StatusFila.ChoqueHorario || x.StatusAlgoritmo == StatusFila.ChoquePeriodo)
                .ToList();

            ICollection<Turma> atribuidas = professor.Prioridades
                .Where(x => x.StatusAlgoritmo == StatusFila.Atribuida)
                .Select(x => x.Turma)
                .ToList();

            foreach (FilaTurma ft in choques)
            {
                bool temChoque = _turmasService.ChoqueHorario(ft.Turma, atribuidas)
                    || _turmasService.ChoquePeriodo(ft.Turma, atribuidas);

                if (!temChoque)
                    ft.StatusAlgoritmo = StatusFila.EmEspera;
            }
        }

        /// <summary>
        /// Procura uma FilaTurma pelo siape do professor e o Id da Turma
        /// </summary>
        /// <param name="siape"></param>
        /// <param name="turma"></param>
        /// <returns></returns>
        private FilaTurma GetFilaTurma(string siape, int turma)
        {
            return filasTurmas.FirstOrDefault(ft => ft.Fila.Professor.Siape.Equals(siape)
                && ft.Turma.Id.Equals(turma));
        }

        /// <summary>
        /// Salva as turmas distribuídas e pendentes e a quantidade das mesmas
        /// </summary>
        /// <param name="resposta"></param>
        private void SalvaResposta(RespostaDto resposta)
        {
            List<FilaTurmaDto> atribuidas = resposta.FilasTurmas.Where(x => x.Status == StatusFila.Atribuida).ToList();
            using (StreamWriter file =
                new StreamWriter(@"C:\Users\igorg\desktop\resposta.txt", true))
            {
                file.WriteLine("******************************************");
                file.WriteLine(DateTime.Now.ToString("dd/MM/yyy HH:mm"));
                file.WriteLine("{0} turmas distribuídas", atribuidas.Count);
                file.WriteLine("{0} turmas pendentes", resposta.TurmasPendentes.Count);
                file.WriteLine("Pendentes + Distribuídas = {0}", atribuidas.Count + resposta.TurmasPendentes.Count);
                file.WriteLine("Total de turmas = {0}", resposta.Turmas.Count);
                file.WriteLine("\nTurmas Distribuídas:");
                atribuidas.ForEach(ft =>
                {
                    TurmaDto t = resposta.Turmas.FirstOrDefault(x => x.Id == ft.IdTurma);
                    file.WriteLine("{0} - {1} {2} ({3})", t.Id, t.CodigoDisc, t.Disciplina.Nome, t.LetraTurma);
                });

                file.WriteLine("\nTurmas Pendentes:");
                resposta.TurmasPendentes.ToList().ForEach(i =>
                {
                    TurmaDto t = resposta.Turmas.FirstOrDefault(x => x.Id == i);
                    file.WriteLine("{0} - {1} {2} ({3})", t.Id, t.CodigoDisc, t.Disciplina.Nome, t.LetraTurma);
                });

                file.WriteLine("******************************************\n\n\n");
            }

        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Altera o status da FilaTurma para DESCONSIDERADA, distribui os casos triviais, encontra os bloqueios
        /// e retorna a resposta
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="siape">Siape do professor utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="turma">Id da turma utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="filasTurmasDto">Estado atual da distribuição</param>
        /// <returns>Objeto RespostaDto</returns>
        public RespostaDto Remover(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            Remover(filaTurma, StatusFila.Desconsiderada);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetTodosDeadlocks();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Joga a FilaTurma para última prioridade do professor.
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="siape">Siape do professor utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="turma">Id da turma utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="filasTurmasDto">Estado atual da distribuição</param>
        /// <returns></returns>
        public RespostaDto UltimaPrioridade(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            filaTurma.Fila.Professor.JogarParaUltimaPrioridadeReal(filaTurma);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetTodosDeadlocks();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Joga a FilaTurma para última posição da fila da turma.
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="siape">Siape do professor utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="turma">Id da turma utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="filasTurmasDto">Estado atual da distribuição</param>
        /// <returns></returns>
        public RespostaDto FinalFila(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            filaTurma.Turma.JogarParaFinalFila(filaTurma);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetTodosDeadlocks();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Altera o status da FilaTurma para ATRIBUIDA, distribui os casos triviais, encontra os bloqueios
        /// e retorna a resposta
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="siape">Siape do professor utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="turma">Id da turma utilizado para encontrar o objeto FilaTurma</param>
        /// <param name="filasTurmasDto">Estado atual da distribuição</param>
        /// <returns>Objeto RespostaDto</returns>
        public RespostaDto Atribuir(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            AtribuirTurma(filaTurma);

            if (filaTurma.Fila.Professor.CHCompletaAtribuida() && filaTurma.Fila.Professor.CHEmEspera(filaTurma) <= 0)
                AtualizaPrioridadesCHCompleta(filaTurma.Fila.Professor);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetTodosDeadlocks();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Encontra os casos triviais, encontra os bloqueios e retorna a distribuição.
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="filasTurmasDto">Distribuição em seu estado atual (null, se estiver começando a distribuição)</param>
        /// <returns>Objeto RespostaDto</returns>
        public RespostaDto Distribuir(int numCenario, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetTodosDeadlocks();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Exclui todos os registros da tabela Ministra e salva a distribuição.
        /// </summary>
        /// <param name="filasTurmasDto"></param>
        public void SalvarDistribuicao(ICollection<FilaTurmaDto> filasTurmasDto)
        {
            _ministraService.LimparMinistra();

            _ministraService.SalvarDistribuicao(
                filasTurmasDto
                .Where(ft => ft.Status == StatusFila.Atribuida)
                .Select(ft => new Ministra
                {
                    IdTurma = ft.IdTurma,
                    Siape = ft.Fila.Siape
                }).ToList()
            );
        }
        #endregion

    }
}