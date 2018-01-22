using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class RestricaoMapper : ISingleMapper<Restricao, RestricaoEntity>
    {
        public RestricaoEntity Map(Restricao source)
        {
            throw new NotImplementedException();
        }

        public Restricao Map(RestricaoEntity destination)
        {
            return destination == null ? null : new Restricao
            {
                Siape = destination.siape,
                Dia = destination.dia,
                Letra = destination.letra
            };
        }
    }
}