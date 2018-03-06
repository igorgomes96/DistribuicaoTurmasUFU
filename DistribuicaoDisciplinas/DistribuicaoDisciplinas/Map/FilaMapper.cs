using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class FilaMapper : ISingleMapper<Fila, FilaEntity>
    {
        private readonly IMapper<Professor, ProfessorEntity> _profMapper;
        private readonly IMapper<Disciplina, DisciplinaEntity> _discMapper;

        public FilaMapper (IMapper<Professor, ProfessorEntity> profMapper,
            IMapper<Disciplina, DisciplinaEntity> discMapper)
        {
            _profMapper = profMapper;
            _discMapper = discMapper;
        }

        public FilaEntity Map(Fila source)
        {
            throw new NotImplementedException();
        }

        public Fila Map(FilaEntity destination)
        {
            return destination == null ? null : new Fila
            {
                Id = destination.id,
                Professor = _profMapper.Map(destination.Professor),
                Disciplina = _discMapper.Map(destination.Disciplina),
                PosicaoReal = destination.pos.Value,
                PosicaoBanco = destination.pos.Value,
                QtdaMaxima = destination.qte_maximo.Value,
                QtdaMinistrada = destination.qte_ministrada.Value
            };
        }
    }
}