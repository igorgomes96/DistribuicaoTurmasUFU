using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class TurmaMapper : ISingleMapper<Turma, TurmaEntity>
    {
        private readonly IMapper<Disciplina, DisciplinaEntity> _discMapper;
        private readonly IMapper<Oferta, OfertaEntity> _ofertaMapper;

        public TurmaMapper(IMapper<Disciplina, DisciplinaEntity> discMapper,
            IMapper<Oferta, OfertaEntity> ofertaMapper)
        {
            _discMapper = discMapper;
            _ofertaMapper = ofertaMapper;
        }

        public TurmaEntity Map(Turma source)
        {
            throw new NotImplementedException();
        }

        public Turma Map(TurmaEntity destination)
        {
            return destination == null ? null : new Turma
            {
                Id = destination.id,
                CodigoDisc = destination.codigo_disc,
                LetraTurma = destination.turma,
                CH = destination.ch.Value,
                Disciplina = _discMapper.Map(destination.Disciplina),
                Horarios = _ofertaMapper.Map(destination.Horarios)
            };
        }
    }
}