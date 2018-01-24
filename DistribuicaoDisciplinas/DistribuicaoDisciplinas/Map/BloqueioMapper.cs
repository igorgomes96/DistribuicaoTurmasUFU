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
            return new BloqueioDto
            {
                Professor = _profMapper.Map(source.FilaTurma.Fila.Professor),
                Turma = _turmaMapper.Map(source.FilaTurma.Turma),
                IdFila = source.FilaTurma.Fila.Id,
                Posicao = source.FilaTurma.Fila.Posicao,
                Prioridade = source.FilaTurma.Prioridade,
                Dependente = Map(source.Dependente)
            };
        }

        public Bloqueio Map(BloqueioDto destination)
        {
            throw new NotImplementedException();
        }
    }
}