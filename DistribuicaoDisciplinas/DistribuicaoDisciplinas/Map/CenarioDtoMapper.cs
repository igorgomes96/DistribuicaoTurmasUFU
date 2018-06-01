using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class CenarioDtoMapper : ISingleMapper<Cenario, CenarioDto>
    {
        public CenarioDto Map(Cenario source)
        {
            return new CenarioDto
            {
                NumCenario = source.NumCenario,
                Descricao = source.Descricao,
                Ano = source.Ano,
                Semestre = source.Semestre
            };
        }

        public Cenario Map(CenarioDto destination)
        {
            throw new NotImplementedException();
        }
    }
}