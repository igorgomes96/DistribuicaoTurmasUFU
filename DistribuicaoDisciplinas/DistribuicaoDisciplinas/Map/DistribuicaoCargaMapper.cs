using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class DistribuicaoCargaMapper : ISingleMapper<DistribuicaoCarga, DistribuicaoCargaEntity>
    {
        public DistribuicaoCargaEntity Map(DistribuicaoCarga source)
        {
            throw new NotImplementedException();
        }

        public DistribuicaoCarga Map(DistribuicaoCargaEntity destination)
        {
            return new DistribuicaoCarga
            {
                IdCenario = destination.IdCenario,
                Regra = destination.Regra,
                Siape = destination.Siape,
                CH = destination.CH
            };
        }
    }
}