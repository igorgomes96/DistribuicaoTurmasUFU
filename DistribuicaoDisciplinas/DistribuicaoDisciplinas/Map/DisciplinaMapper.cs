using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class DisciplinaMapper : ISingleMapper<Disciplina, DisciplinaEntity>
    {
        private readonly IMapper<Curso, CursoEntity> _cursoMapper;

        public DisciplinaMapper(IMapper<Curso, CursoEntity> cursoMapper)
        {
            _cursoMapper = cursoMapper;
        }

        public DisciplinaEntity Map(Disciplina source)
        {
            throw new NotImplementedException();
        }

        public Disciplina Map(DisciplinaEntity destination)
        {
            return destination == null ? null : new Disciplina
            {
                Codigo = destination.codigo,
                Nome = destination.nome,
                Periodo = destination.periodo.Value,
                Curso = _cursoMapper.Map(destination.Curso)
            };

        }
    }
}