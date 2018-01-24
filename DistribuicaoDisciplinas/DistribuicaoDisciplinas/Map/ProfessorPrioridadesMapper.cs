using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class ProfessorPrioridadesMapper : ISingleMapper<Professor, ProfessorPrioridadesDto>
    {
        private readonly IMapper<FilaTurma, TurmaRespostaDto> _turmaMapper;
        private readonly IMapper<Professor, ProfessorDto> _profMapper;

        public ProfessorPrioridadesMapper(IMapper<FilaTurma, TurmaRespostaDto> turmaMapper,
            IMapper<Professor, ProfessorDto> profMapper)
        {
            _turmaMapper = turmaMapper;
            _profMapper = profMapper;
        }
        public ProfessorPrioridadesDto Map(Professor source)
        {
            return new ProfessorPrioridadesDto
            {
                Professor = _profMapper.Map(source),
                Prioridades = _turmaMapper.Map(source.Prioridades)
            };
        }

        public Professor Map(ProfessorPrioridadesDto destination)
        {
            throw new NotImplementedException();
        }
    }
}