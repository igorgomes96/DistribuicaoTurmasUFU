using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    public class CenarioFilaTurma
    {
        public Cenario Cenario { get; set; }
        public FilaTurma FilaTurma { get; set; }
        public StatusFila Status { get; set; }
    }
}