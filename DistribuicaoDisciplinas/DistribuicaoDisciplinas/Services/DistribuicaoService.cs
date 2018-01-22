﻿using System;
using System.Collections.Generic;
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
        #endregion

        #region Map
        private readonly IMapper<FilaTurma, FilaTurmaRespostaDto> _filaTurmaRespMapper;
        #endregion

        #region Constructor
        public DistribuicaoService(
            IProfessoresService professoresService,
            ITurmasService turmasService,
            ICenariosService cenariosService,
            IMapper<FilaTurma, FilaTurmaRespostaDto> filaTurmaRespMapper)
        {
            _professoresService = professoresService;
            _turmasService = turmasService;
            _cenariosService = cenariosService;
            _filaTurmaRespMapper = filaTurmaRespMapper;
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
            ICollection<FilaTurma> filasTurmas = filasTurmasEntities.Select(ft => {
                return new FilaTurma
                {
                    Fila = filas[ft.id_fila],
                    Turma = turmas[ft.id_turma],
                    Prioridade = ft.prioridade.Value
                };
            }).ToList();

            //Atualiza prioridades dos professores
            filasTurmas.OrderBy(ft => ft.Fila.QtdaMaximaJaMinistrada()).ThenBy(ft => ft.Prioridade) //Ordena por prioridade, colocando as filas onde o professor já ministrou a qtda máxima de vezes a turma
                .ToList().ForEach(ft => ft.Fila.Professor.Prioridades.Add(ft));

            //Atualiza prioridades das turmas
            filasTurmas.OrderBy(ft => ft.Fila.Posicao)
                .ToList().ForEach(ft => ft.Turma.Posicoes.Add(ft));

            return filasTurmas;

        }

        private void LimpezaInicial()
        {
            foreach (Professor prof in professores.Values)
            {
                List<FilaTurma> turmasAtribuidas = new List<FilaTurma>();
                bool flagCHCompleta = false;

                foreach (FilaTurma ft in prof.Prioridades.Where(x => x.StatusAlgoritmo != StatusFilaAlgoritmo.ChoqueRestricao))
                {
                    if (flagCHCompleta)
                    {
                        ft.StatusAlgoritmo = StatusFilaAlgoritmo.Desconsiderada;
                    }
                    else
                    {
                        if (ft.Turma.Posicoes.FirstOrDefault(t => t.StatusAlgoritmo != StatusFilaAlgoritmo.ChoqueRestricao) == ft)
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
                .ToList().ForEach(x => x.StatusAlgoritmo = StatusFilaAlgoritmo.ChoqueRestricao);
        }

        /// <summary>
        /// Para todos os professores que já estão com a CH preenchida, atualiza as prioridades EmEspera e NaoAnalisadasAinda
        /// para CHCompleta.
        /// </summary>
        private void AtualizaPrioridadesCHCompleta()
        {
            filasTurmas
                .Where(ft => ft.Fila.Professor.CHCompletaAtribuida() && (ft.StatusAlgoritmo == StatusFilaAlgoritmo.EmEspera ||
                    ft.StatusAlgoritmo == StatusFilaAlgoritmo.NaoAnalisadaAinda))
                .ToList()
                .ForEach(ft => ft.StatusAlgoritmo = StatusFilaAlgoritmo.CHCompleta);
        }

        /// <summary>
        /// Faz a atribuição dos casos triviais (Turmas não analisadas ou em espera que são a próxima prioridade do professor)
        /// </summary>
        /// <returns>true, se houve alguma atribuição; false, se não.</returns>
        private bool CasosTriviais()
        {
            bool flag = false;
            foreach (Professor p in professores.Values)
            {
                List<FilaTurma> possibilidadesProf = p.Prioridades
                    .Where(pp => pp.StatusAlgoritmo == StatusFilaAlgoritmo.EmEspera
                        || pp.StatusAlgoritmo == StatusFilaAlgoritmo.NaoAnalisadaAinda)
                    .ToList();

                foreach (FilaTurma filaTurma in possibilidadesProf)
                {
                    List<FilaTurma> possibilidadesTurma = filaTurma.Turma.Posicoes
                        .Where(pt => pt.StatusAlgoritmo == StatusFilaAlgoritmo.NaoAnalisadaAinda
                            || pt.StatusAlgoritmo == StatusFilaAlgoritmo.EmEspera).ToList();

                    if (possibilidadesTurma.FirstOrDefault() == filaTurma)
                    {
                        if ((p.CHAtribuida() + filaTurma.Turma.CH + ACRESCIMO_CH + p.CHEmEspera()) <= p.CH)
                        {
                            AtribuirTurma(filaTurma);
                            flag = true;
                        }
                        else
                            filaTurma.StatusAlgoritmo = StatusFilaAlgoritmo.EmEspera;
                    }
                    else
                    {
                        filaTurma.StatusAlgoritmo = StatusFilaAlgoritmo.EmEspera;
                    }
                }
            }

            return flag;
        }

        private void AtribuirTurma(FilaTurma filaTurma)
        {
            //Atualiza o status de todas as filas da turma que foi atribuída para OutroProfessor
            filaTurma.Turma.Posicoes
                .Where(pt => pt.StatusAlgoritmo == StatusFilaAlgoritmo.NaoAnalisadaAinda
                    || pt.StatusAlgoritmo == StatusFilaAlgoritmo.EmEspera).ToList()
                .ForEach(ft => ft.StatusAlgoritmo = StatusFilaAlgoritmo.OutroProfessor);

            foreach (FilaTurma prioridade in filaTurma.Fila.Professor.Prioridades)
            {
                if (!prioridade.Equals(filaTurma))
                {
                    if (_turmasService.ChoqueHorario(filaTurma.Turma, prioridade.Turma))
                        prioridade.StatusAlgoritmo = StatusFilaAlgoritmo.ChoqueHorario;
                    else if (_turmasService.ChoquePeriodo(filaTurma.Turma, prioridade.Turma))
                        prioridade.StatusAlgoritmo = StatusFilaAlgoritmo.ChoquePeriodo;
                }
            }

            //Atualiza o status da Fila atribuída
            filaTurma.StatusAlgoritmo = StatusFilaAlgoritmo.Atribuda;
        }
        #endregion

        #region Public Methods
        public ICollection<FilaTurmaRespostaDto> Distribuir(int numCenario)
        {
            cenario = _cenariosService.Find(numCenario);
            ICollection<FilaTurmaEntity> filasTurmasEntities = _filasTurmasRep
                .Query(ft => ft.Turma.ano == cenario.Ano
                    && ft.Turma.semestre == cenario.Semestre
                    && ft.Fila.ano == cenario.Ano
                    && ft.Fila.semestre == cenario.Semestre);

            filasTurmas = Encadear(filasTurmasEntities);
            //Atualiza o status de todas para NaoAnalisadaAinda
            filasTurmas.ToList().ForEach(x => x.StatusAlgoritmo = StatusFilaAlgoritmo.NaoAnalisadaAinda);

            TurmasComRestricao();

            LimpezaInicial();

            while (CasosTriviais())
            {
                AtualizaPrioridadesCHCompleta();
            };

            return _filaTurmaRespMapper
                .Map(filasTurmas)
                .OrderBy(x => x.Professor)
                .ThenBy(x => x.Prioridade)
                .ToList();
        }
        #endregion

    }
}