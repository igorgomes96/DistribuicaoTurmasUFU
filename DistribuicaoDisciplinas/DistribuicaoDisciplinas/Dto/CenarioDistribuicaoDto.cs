using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class CenarioDistribuicaoDto
    {
        public Cenario Cenario { get; set; }
        public RespostaDto Resposta { get; set; }
    }
}