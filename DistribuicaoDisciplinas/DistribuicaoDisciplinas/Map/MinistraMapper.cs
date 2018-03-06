using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class MinistraMapper : ISingleMapper<Ministra, MinistraEntity>
    {

        private readonly ISingleMapper<Professor, ProfessorEntity> _profMap;
        private readonly ISingleMapper<Turma, TurmaEntity> _turmaMap;

        public MinistraMapper(ISingleMapper<Professor, ProfessorEntity> profMap,
            ISingleMapper<Turma, TurmaEntity> turmaMap)
        {
            _profMap = profMap;
            _turmaMap = turmaMap;
        }

        public MinistraEntity Map(Ministra source)
        {
            return new MinistraEntity
            {
                id_turma = source.IdTurma,
                siape = source.Siape
            };
        }

        public Ministra Map(MinistraEntity destination)
        {
            return destination == null ? null : new Ministra
            {
                Professor = _profMap.Map(destination.Professor),
                Turma = _turmaMap.Map(destination.Turma)
            };
        }
    }
}