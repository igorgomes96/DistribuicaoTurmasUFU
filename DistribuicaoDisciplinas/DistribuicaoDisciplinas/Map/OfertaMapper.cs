using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class OfertaMapper : ISingleMapper<Oferta, OfertaEntity>
    {

        public OfertaEntity Map(Oferta source)
        {
            throw new NotImplementedException();
        }

        public Oferta Map(OfertaEntity destination)
        {
            return destination == null ? null : new Oferta
            {
                IdTurma = destination.id_turma,
                Dia = destination.dia,
                Letra = destination.letra,
            };
        }
    }
}