using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class ProfessorRespostaMapper : ISingleMapper<Professor, ProfessorRespostaDto>
    {
        private readonly IMapper<FilaTurma, TurmaRespostaDto> _turmaMapper;

        public ProfessorRespostaMapper(IMapper<FilaTurma, TurmaRespostaDto> turmaMapper)
        {
            _turmaMapper = turmaMapper;
        }
        public ProfessorRespostaDto Map(Professor source)
        {
            return new ProfessorRespostaDto
            {
                Siape = source.Siape,
                Nome = source.Nome,
                DataIngresso = source.DataIngresso,
                CH = source.CH,
                Prioridades = _turmaMapper.Map(source.Prioridades)
            };
        }

        public Professor Map(ProfessorRespostaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}