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
using static DistribuicaoDisciplinas.Util.Constants;
using DistribuicaoDisciplinas.Repository;

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
        private readonly ICenariosFilasTurmasRepository _cenarioFilaTurmaRep;
        private readonly IMinistraRepository _ministraRepository;
        private readonly IGenericRepository<CenarioEntity> _cenarioRep;
        private readonly IGenericRepository<AtribuicaoManualEntity> _atribuicaoManualRep;
        #endregion

        #region Services
        private readonly IProfessoresService _professoresService;
        private readonly ITurmasService _turmasService;
        private readonly ICenariosService _cenariosService;
        private readonly IMinistraService _ministraService;
        private readonly ICenariosFilasTurmasService _cenariosFilasTurmasService;
        private readonly IFilasTurmasService _filasTurmasService;
        private readonly IRestricoesService _restricoesService;
        #endregion

        #region Map
        private readonly IMapper<Turma, TurmaDto> _turmaMapper;
        private readonly IMapper<Fila, FilaDto> _filaMapper;
        private readonly IMapper<Bloqueio, BloqueioDto> _bloqueioMapper;
        private readonly IMapper<Ministra, FilaTurma> _ministraFTMapper;
        private readonly IMapper<Professor, ProfessorDto> _professorMapper;
        private readonly IMapper<FilaTurma, FilaTurmaDto> _filaTurmaMapper;
        private readonly IMapper<AtribuicaoManual, AtribuicaoManualEntity> _atribuicaoManualMapper;
        #endregion

        #region Constructor
        public DistribuicaoService(
            IGenericRepository<FilaTurmaEntity> filasTurmasRep,
            ICenariosFilasTurmasRepository cenarioFilaTurmaRep,
            IMinistraRepository ministraRepository,
            IGenericRepository<CenarioEntity> cenarioRep,
            IGenericRepository<AtribuicaoManualEntity> atribuicaoManualRep,
            IProfessoresService professoresService,
            ITurmasService turmasService,
            ICenariosService cenariosService,
            IMinistraService ministraService,
            ICenariosFilasTurmasService cenariosFilasTurmasService,
            IFilasTurmasService filasTurmasService,
            IRestricoesService restricoesService,
            IMapper<Turma, TurmaDto> turmaMapper,
            IMapper<Fila, FilaDto> filaMapper,
            IMapper<Bloqueio, BloqueioDto> bloqueioMapper,
            IMapper<Ministra, FilaTurma> ministraFTMapper,
            IMapper<Professor, ProfessorDto> professorMapper,
            IMapper<FilaTurma, FilaTurmaDto> filaTurmaMapper,
            IMapper<AtribuicaoManual, AtribuicaoManualEntity> atribuicaoManualMapper)
        {
            _filasTurmasRep = filasTurmasRep;
            _cenarioFilaTurmaRep = cenarioFilaTurmaRep;
            _ministraRepository = ministraRepository;
            _cenarioRep = cenarioRep;
            _atribuicaoManualRep = atribuicaoManualRep;

            _professoresService = professoresService;
            _turmasService = turmasService;
            _cenariosService = cenariosService;
            _ministraService = ministraService;
            _cenariosFilasTurmasService = cenariosFilasTurmasService;
            _filasTurmasService = filasTurmasService;
            _restricoesService = restricoesService;

            _turmaMapper = turmaMapper;
            _filaMapper = filaMapper;
            _bloqueioMapper = bloqueioMapper;
            _ministraFTMapper = ministraFTMapper;
            _professorMapper = professorMapper;
            _filaTurmaMapper = filaTurmaMapper;
            _atribuicaoManualMapper = atribuicaoManualMapper;

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
            professores = _professoresService.ListAtivos().ToDictionary(p => p.Siape.Trim());

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
                    Fila = filas[ft.id_fila],
                    Turma = turmas[ft.id_turma],
                    PrioridadeReal = ft.prioridade.Value,
                    PrioridadeBanco = ft.prioridade.Value
                };
            }).ToList().ForEach(ft =>
            {
                filasTurmas.Add(ft);
            });

            // Disciplinas optativas e de pós-graduação (não tem fila)
            List<FilaTurma> optativasPos = GetTurmasSemFilas().ToList();
            optativasPos.ForEach(op =>
            {
                op.Turma = turmas[op.Turma.Id];
                op.Fila.Disciplina = disciplinas[op.Turma.CodigoDisc];
                op.Fila.Professor = professores[op.Fila.Professor.Siape];
                filasTurmas.Add(op);
            });

            //Atualiza prioridades dos professores
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
        public ICollection<FilaTurma> GetTurmasSemFilas()
        {
            ICollection<Ministra> ministraTurmasSemFila = _ministraService.ListTurmasSemFila(cenario.Ano, cenario.Semestre);
            ICollection<FilaTurma> novasFilasTurmas = _ministraFTMapper.Map(ministraTurmasSemFila).ToList();

            novasFilasTurmas.ToList().ForEach(ft =>
            {
                ft.AtribuicaoFixa = true;
                ft.StatusAlgoritmo = StatusFila.Atribuida;
            });


            //Ajustes manuais
            ICollection<AtribuicaoManual> atribuicoesManuais = _atribuicaoManualMapper.Map(
                _atribuicaoManualRep.Query(a => a.num_cenario == cenario.NumCenario)).ToList();

            foreach (AtribuicaoManual a in atribuicoesManuais)
            {
                novasFilasTurmas.Add(new FilaTurma
                {
                    Fila = new Fila
                    {
                        Professor = a.Professor,
                        Disciplina = a.Turma.Disciplina,
                        PosicaoReal = -1,
                        QtdaMaxima = 1,
                        QtdaMinistrada = 0
                    },
                    Turma = a.Turma,
                    StatusAlgoritmo = StatusFila.Atribuida,
                    PrioridadeReal = -1
                });
            }

            return novasFilasTurmas;

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

                    //ESSA CONDIÇÃO É EXTREMAMENTE IMPORTANTE, APESAR DE PARECER REDUNDANTE!!!
                    //A lista possibilidadesProf é um filtro das prioridades EmEspera e NaoAnalisadaAinda.
                    //Todavia, em passos anteriores desse foreach, o status de filaTurma que antes estava
                    //EmEspera ou NaoAnalisadaAinda pode ter sido alterado devido a atribuição de alguma outra turma de p,
                    //com isso, podendo ter conflitado horário ou período, por exemplo. Portanto, não posso considerar que 
                    //ela ainda seja uma possibilidade de atribuição para o professor p, devendo então passa para a próxima
                    //filaTurma a ser analisada
                    if (filaTurma.StatusAlgoritmo != StatusFila.EmEspera && filaTurma.StatusAlgoritmo != StatusFila.NaoAnalisadaAinda)
                        continue;
                    
                    List<FilaTurma> possibilidadesTurma = filaTurma.Turma.Posicoes
                        .Where(pt => pt.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda
                            || pt.StatusAlgoritmo == StatusFila.EmEspera).ToList();

                    if (possibilidadesTurma.Count <= 0)
                        break;

                    int chLimite = (p.CH + ACRESCIMO_CH);

                    if (possibilidadesTurma.FirstOrDefault().Equals(filaTurma)                  //Verifica se o professor está na primeira posição da turma
                        && !_turmasService.ChoqueHorario(filaTurma.Turma, prioridadesEmEspera)  //Verifica se a turma tem choque de horário com as turmas em espera
                        && !_turmasService.ChoquePeriodo(filaTurma.Turma, prioridadesEmEspera)  //Verifica se a turma tem choque de período com as turmas em espera
                        && filaTurma.Turma.CH == CH_DEFAULT)                     //Verifica se a CH da turma tem CH default
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
                Bloqueios = bloqueios == null ? new List<BloqueioDto>() : _bloqueioMapper.Map(bloqueios).OrderBy(x => x.Tamanho).ToList()
            };

            return resposta;

        }

        /// <summary>
        /// Identifica as turmas em espera, que são primeira prioridade, que tem a carga horária diferente da carga horária default.
        /// </summary>
        /// <returns>Bloqueios</returns>
        private ICollection<Bloqueio> GetCHNotDefault()
        {
            ICollection<Bloqueio> bloqueios = new List<Bloqueio>();
            foreach (Professor p in professores.Values)
            {
                FilaTurma primeiraPrioridade = p.PrimeiraPrioridadeDisponivel();
                if (primeiraPrioridade != null && primeiraPrioridade.Turma.CH != CH_DEFAULT)
                {
                    bloqueios.Add(new Bloqueio
                    {
                        TipoBloqueio = TipoBloqueio.DisciplinaCHDiferente4,
                        FilaTurma = p.PrimeiraPrioridadeDisponivel()
                    });
                }
            }
            return bloqueios;
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

                //Verifica se é uma turma que somente está esperando confirmação por ter CH diferente da CH default
                if (deadlock.Tamanho == 2 && deadlock.Dependente.FilaTurma.Turma.CH != CH_DEFAULT)
                    continue;

                //Verifico se esse deadlock já foi identificado. Para isso,
                //basta verificar se a FilaTurma cabeça do deadlock já existe
                //em alqum dos deadlocks anteriores
                if (deadlock != null && !deadlocks.Any(x => x.Contains(deadlock.FilaTurma)))
                    deadlocks.Add(deadlock);

            }

            return deadlocks;
        }

        private ICollection<Bloqueio> GetBloqueios()
        {
            return GetTodosDeadlocks().Concat(GetCHNotDefault())
                .OrderBy(x => x.Tamanho)
                .ThenBy(x => x.FilaTurma.PrioridadeReal)
                .ThenBy(x => x.FilaTurma.Fila.PosicaoReal)
                .ToList();
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
                            .FirstOrDefault(ft => ft.StatusAlgoritmo == StatusFila.EmEspera);  // VERIFICAR

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
                        ft.Turma.JogarParaFinalFila(ft);
                        //ft.Fila.Professor.JogarParaUltimaPrioridadeReal(ft);
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
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        private void PreparaDistribuicao(int numCenario, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            cenario = _cenariosService.Find(numCenario);

            if (cenario == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

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
        /// Verifica se a atribuição de uma turma a um professor é válida, retornando o motivo.
        /// </summary>
        /// <param name="filaTurma"></param>
        /// <returns></returns>
        private ValidaAtribuicao VerificaAtribuicaoTurma(FilaTurma filaTurma)
        {
            if (!filaTurma.Turma.TurmaPendente())
                return ValidaAtribuicao.JaAtribuida;

            List<Turma> turmasAtribuidas = filaTurma.Fila.Professor.Prioridades
                .Where(p => p.StatusAlgoritmo == StatusFila.Atribuida)
                .Select(x => x.Turma).ToList();

            if (_turmasService.ChoqueHorario(filaTurma.Turma, turmasAtribuidas))
                return ValidaAtribuicao.ChoqueHorario;

            if (_turmasService.ChoquePeriodo(filaTurma.Turma, turmasAtribuidas))
                return ValidaAtribuicao.ChoquePeriodo;

            if (_restricoesService.TemRestricao(filaTurma.Fila.Professor, filaTurma.Turma))
                return ValidaAtribuicao.RestricaoHorario;

            return ValidaAtribuicao.Valida;

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
            //altera o status das turmas = CHCompleta para EmEspera daquelas que ainda não estão com nenhum professor
            if (!professor.CHCompletaAtribuida())
            {
                List<FilaTurma> pendentesEmEspera = professor
                    .Prioridades.Where(p => p.Turma.TurmaPendente() && p.StatusAlgoritmo == StatusFila.CHCompleta)
                    .ToList();
                pendentesEmEspera.ForEach(p => p.StatusAlgoritmo = StatusFila.EmEspera);
            }

            // Como uma turma foi removida, alterar o status daquelas que tem choque com ela
            // para EmEspera, se ela não tiver choque com mais nenhum atribuída.
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

                // Se não choque e está pendente, coloca em espera
                if (!temChoque && ft.Turma.TurmaPendente())
                    ft.StatusAlgoritmo = StatusFila.EmEspera;
                else if (!temChoque)// Se não tem choque, mas já foi atribuída a outro professor, altera para AtribuidoParaOutroProfessor
                    ft.StatusAlgoritmo = StatusFila.OutroProfessor;
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
        /// <exception cref="FilaTurmaNaoEncontradaException"></exception>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto Remover(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            Remover(filaTurma, StatusFila.Desconsiderada);

            //Se for uma turma atribuída manualmente, remove do banco e remove do encadeamento
            if (_atribuicaoManualRep.Existe(numCenario, siape, turma)) { 
                _atribuicaoManualRep.Delete(numCenario, siape, turma);
                filaTurma.Turma.Posicoes.Remove(filaTurma);
                filaTurma.Fila.Professor.Prioridades.Remove(filaTurma);
                filasTurmas.Remove(filaTurma);
            }

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetBloqueios();

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
        /// <exception cref="FilaTurmaNaoEncontradaException"></exception>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto UltimaPrioridade(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            filaTurma.Fila.Professor.JogarParaUltimaPrioridadeReal(filaTurma);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetBloqueios();

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
        /// <exception cref="FilaTurmaNaoEncontradaException"></exception>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto FinalFila(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null)
                throw new FilaTurmaNaoEncontradaException();

            filaTurma.Turma.JogarParaFinalFila(filaTurma);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetBloqueios();

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
        /// <exception cref="ChoqueHorarioException"></exception>
        /// <exception cref="ChoquePeriodoException"></exception>
        /// <exception cref="JaAtribuidaException"></exception>
        /// <exception cref="RestricaoHorarioException"></exception>
        /// <exception cref="FilaTurmaNaoEncontradaException"></exception>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto Atribuir(int numCenario, string siape, int turma, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            bool flagAtribuicaoManual = false;
            PreparaDistribuicao(numCenario, filasTurmasDto);

            //Obtém a referência para FilaTurma, ou se for uma atribuição manual e não houver FilaTurma correspondente
            //cria um novo objeto e faz o encadeamento do mesmo
            FilaTurma filaTurma = GetFilaTurma(siape, turma);

            if (filaTurma == null) {

                flagAtribuicaoManual = true;

                filaTurma = new FilaTurma
                {
                    Fila = new Fila
                    {
                        Professor = professores[siape],
                        Disciplina = turmas[turma].Disciplina,
                        PosicaoReal = -1,
                        QtdaMaxima = 1,
                        QtdaMinistrada = 0
                    },
                    PrioridadeReal = -1,
                    Turma = turmas[turma]
                };

                professores[siape].Prioridades.Add(filaTurma);
                turmas[turma].Posicoes.Add(filaTurma);
                filasTurmas.Add(filaTurma);
                OrdenaPrioridadesPosicoes();

            }

            switch (VerificaAtribuicaoTurma(filaTurma))
            {
                case ValidaAtribuicao.ChoqueHorario:
                    throw new ChoqueHorarioException("Atribuição inválida! Choque de horário com turma já atribuída!");
                case ValidaAtribuicao.ChoquePeriodo:
                    throw new ChoquePeriodoException("Atribuição inválida! Choque de período com turma já atribuída!");
                case ValidaAtribuicao.JaAtribuida:
                    throw new JaAtribuidaException("Atribuição inválida! Turma já atribuída a outro professor!");
                case ValidaAtribuicao.RestricaoHorario:
                    throw new RestricaoHorarioException("Atribuição inválida! O professor possui restrição em algum horário da turma!");
            }

            //Se houve atribuição manual, insere o novo registro de atribuição na tabela atribuicao_manual
            if (flagAtribuicaoManual)
                _atribuicaoManualRep.Add(new AtribuicaoManualEntity
                    {
                        num_cenario = numCenario,
                        siape = siape,
                        id_turma = turma
                    });

            AtribuirTurma(filaTurma);

            if (filaTurma.Fila.Professor.CHCompletaAtribuida() && filaTurma.Fila.Professor.CHEmEspera(filaTurma) <= 0)
                AtualizaPrioridadesCHCompleta(filaTurma.Fila.Professor);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetBloqueios();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Encontra os casos triviais, encontra os bloqueios e retorna a distribuição.
        /// </summary>
        /// <param name="numCenario">Número do cenário a que se refere a distribuição</param>
        /// <param name="filasTurmasDto">Distribuição em seu estado atual (null, se estiver começando a distribuição)</param>
        /// <returns>Objeto RespostaDto</returns>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto Distribuir(int numCenario, ICollection<FilaTurmaDto> filasTurmasDto)
        {
            PreparaDistribuicao(numCenario, filasTurmasDto);

            while (CasosTriviais()) { };

            ICollection<Bloqueio> bloqueios = GetBloqueios();

            return GeraResposta(bloqueios);
        }

        /// <summary>
        /// Carrega do banco o status de cada FilaTurma do que já foi distribuido para determinado cenário.
        /// </summary>
        /// <param name="numCenario"></param>
        /// <returns></returns>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public RespostaDto CarregaDistribuicao(int numCenario)
        {
            if (!_cenarioRep.Existe(numCenario))
                throw new CenarioNaoEncontradoException("Código de cenário não encontrado!");

            ICollection<CenarioFilaTurma> cenarioFilasTurmas = _cenariosFilasTurmasService.List(numCenario);

            ICollection<FilaTurmaDto> filasTurmasDto = cenarioFilasTurmas?
                .Select(x => new FilaTurmaDto
                {
                    Fila = _filaMapper.Map(x.FilaTurma.Fila),
                    IdTurma = x.FilaTurma.Turma.Id,
                    PrioridadeBanco = x.FilaTurma.PrioridadeBanco,
                    PrioridadeReal = x.FilaTurma.PrioridadeReal,
                    Status = x.Status
                }).ToList();

            if (filasTurmasDto == null || filasTurmasDto.Count == 0) return null;

            PreparaDistribuicao(numCenario, filasTurmasDto);
            return GeraResposta(null);
        }

        /// <summary>
        /// Salva a distribuição para o cenário especificado
        /// </summary>
        /// <param name="numCenario"></param>
        /// <param name="filasTurmas"></param>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public void SalvarDistribuicao(int numCenario, ICollection<FilaTurmaDto> filasTurmas)
        {
            cenario = _cenariosService.Find(numCenario);

            if (cenario == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            ICollection<CenarioFilaTurmaEntity> entitiesToSave = new List<CenarioFilaTurmaEntity>();

            //_cenarioFilaTurmaRep.Delete(x => x.num_cenario == numCenario);
            _cenarioFilaTurmaRep.DeleteByCenario(numCenario);

            ICollection<FilaTurma> turmasSemFila = GetTurmasSemFilas();

            entitiesToSave = filasTurmas.Where(x => !turmasSemFila.Any(y => y.Turma.Id == x.IdTurma))
                .Select(x => new CenarioFilaTurmaEntity
                {
                    id_fila = x.Fila.Id,
                    id_turma = x.IdTurma,
                    num_cenario = cenario.NumCenario,
                    status = x.Status
                }).ToList();

            _cenarioFilaTurmaRep.SaveDistribuicao(entitiesToSave);
        }


        /// <summary>
        /// Oficiliza (salva na tabela ministra) uma distribuição já realizada para um cenário
        /// </summary>
        /// <param name="numCenario"></param>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public void OficializarDistribuicao(int numCenario)
        {
            cenario = _cenariosService.Find(numCenario);

            if (cenario == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            // Limpa todas as atribuições para o semestre, ignorando as turmas que não possuem fila
            // nem foram atribuída manualmente
            _ministraRepository.DeleteTurmasComFilaBySemestre(cenario.Ano, cenario.Semestre);

            // Salvar a distribuição, concatenando com as atribuições manuais
            _ministraRepository.SalvarDistribuicao(
                _cenarioFilaTurmaRep.Query(x => x.status == StatusFila.Atribuida && x.num_cenario == numCenario)
                .Select(x => new MinistraEntity
                {
                    id_turma = x.id_turma,
                    siape = x.FilaTurma.Fila.siape
                }).Concat(_atribuicaoManualRep.Query(x => x.num_cenario == numCenario)
                .Select(x => new MinistraEntity
                {
                    id_turma = x.id_turma,
                    siape = x.siape
                })).ToList());
        }

        /// <summary>
        /// Cria um novo cenário, e duplica a distribuição do cenário de id numCenario para o novo cenário
        /// criado.
        /// </summary>
        /// <param name="numCenario">Id do cenário base</param>
        /// <param name="novoCenario">Cenário a ser criado</param>
        /// <returns></returns>
        /// <exception cref="SemestreDiferenteCenarioException"></exception>
        /// <exception cref="CenarioNaoEncontradoException"></exception>
        public CenarioDistribuicaoDto DuplicarDistribuicao(int numCenario, Cenario novoCenario)
        {
            Cenario cenario = _cenariosService.Find(numCenario);

            if (cenario == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            if (novoCenario.Ano != cenario.Ano || novoCenario.Semestre != cenario.Semestre)
                throw new SemestreDiferenteCenarioException("O novo cenário deve ser no mesmo semestre do cenário base!");

            novoCenario = _cenariosService.DuplicarCenario(numCenario, novoCenario);

            ICollection<CenarioFilaTurmaEntity> distBase = _cenarioFilaTurmaRep
                .Query(x => x.num_cenario == numCenario);

            ICollection<CenarioFilaTurmaEntity> distribuicao = distBase
                .Select(x => new CenarioFilaTurmaEntity
                {
                    num_cenario = novoCenario.NumCenario,
                    id_fila = x.id_fila,
                    id_turma = x.id_turma,
                    status = x.status
                }).ToList();
                
            _cenarioFilaTurmaRep.SaveAll(distribuicao);

            return new CenarioDistribuicaoDto
            {
                Resposta = CarregaDistribuicao(novoCenario.NumCenario),
                Cenario = novoCenario
            };
        }
        #endregion

    }
}