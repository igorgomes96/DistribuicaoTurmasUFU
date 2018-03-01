using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class BloqueioMapper : ISingleMapper<Bloqueio, BloqueioDto>
    {
        private readonly ISingleMapper<Professor, ProfessorDto> _profMapper;
        private readonly ISingleMapper<Turma, TurmaDto> _turmaMapper;

        public BloqueioMapper(ISingleMapper<Professor, ProfessorDto> profMapper,
            ISingleMapper<Turma, TurmaDto> turmaMapper)
        {
            _profMapper = profMapper;
            _turmaMapper = turmaMapper;
        }

        public BloqueioDto Map(Bloqueio source)
        {
            return source == null ? null : new BloqueioDto
            {
                IdTurma = source.FilaTurma.Turma.Id,
                IdFila = source.FilaTurma.Fila.Id,
                Siape = source.FilaTurma.Fila.Professor.Siape,
                TipoBloqueio = source.TipoBloqueio,
                Dependente = Map(source.Dependente),
                Tamanho = source.Tamanho
            };
        }

        public Bloqueio Map(BloqueioDto destination)
        {
            throw new NotImplementedException();
        }
    }
}