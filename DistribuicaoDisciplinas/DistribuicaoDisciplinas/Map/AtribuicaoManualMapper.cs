using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class AtribuicaoManualMapper : ISingleMapper<AtribuicaoManual, AtribuicaoManualEntity>
    {
        private readonly IMapper<Cenario, CenarioEntity> _cenarioMap;
        private readonly IMapper<Professor, ProfessorEntity> _profMap;
        private readonly IMapper<Turma, TurmaEntity> _turmaMap;

        public AtribuicaoManualMapper(IMapper<Cenario, CenarioEntity> cenarioMap,
            IMapper<Professor, ProfessorEntity> profMap,
            IMapper<Turma, TurmaEntity> turmaMap)
        {
            _cenarioMap = cenarioMap;
            _profMap = profMap;
            _turmaMap = turmaMap;
        }

        public AtribuicaoManualEntity Map(AtribuicaoManual source)
        {
            return new AtribuicaoManualEntity
            {
                id_turma = source.Turma.Id,
                siape = source.Professor.Siape,
                num_cenario = source.Cenario.NumCenario
            };
        }

        public AtribuicaoManual Map(AtribuicaoManualEntity destination)
        {
            return new AtribuicaoManual
            {
                Cenario = _cenarioMap.Map(destination.Cenario),
                Professor = _profMap.Map(destination.Professor),
                Turma = _turmaMap.Map(destination.Turma)
            };
        }
    }
}