using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class CenarioFilaTurmaMapper : ISingleMapper<CenarioFilaTurma, CenarioFilaTurmaEntity>
    {
        private readonly IMapper<FilaTurma, FilaTurmaEntity> _filaTurmaMap;

        public CenarioFilaTurmaMapper(IMapper<FilaTurma, FilaTurmaEntity> filaTurmaMap)
        {
            _filaTurmaMap = filaTurmaMap;
        }

        public CenarioFilaTurmaEntity Map(CenarioFilaTurma source)
        {
            return new CenarioFilaTurmaEntity
            {
                num_cenario = source.Cenario.NumCenario,
                id_fila = source.FilaTurma.Fila.Id,
                id_turma = source.FilaTurma.Turma.Id,
                status = source.FilaTurma.StatusAlgoritmo,
                posicao = source.FilaTurma.Fila.PosicaoReal,
                prioridade = source.FilaTurma.PrioridadeReal
            };
        }

        public CenarioFilaTurma Map(CenarioFilaTurmaEntity destination)
        {
            return new CenarioFilaTurma
            {
                Cenario = new Cenario
                {
                    NumCenario = destination.num_cenario,
                    //Ano = destination.Cenario.ano,
                    //Semestre = destination.Cenario.semestre,
                    //Descricao = destination.Cenario.descricao_cenario
                },
                FilaTurma = _filaTurmaMap.Map(destination.FilaTurma),
                Status = destination.status,
                Posicao = destination.posicao,
                Prioridade = destination.prioridade
            };
        }
    }
}