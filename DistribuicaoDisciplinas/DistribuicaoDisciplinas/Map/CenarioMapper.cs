using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class CenarioMapper : ISingleMapper<Cenario, CenarioEntity>
    {
        public CenarioEntity Map(Cenario source)
        {
            throw new NotImplementedException();
        }

        public Cenario Map(CenarioEntity destination)
        {
            return destination == null ? null : new Cenario
            {
                NumCenario = destination.num_cenario,
                Descricao = destination.descricao_cenario,
                Ano = destination.ano,
                Semestre = destination.semestre
            };
        }
    }
}