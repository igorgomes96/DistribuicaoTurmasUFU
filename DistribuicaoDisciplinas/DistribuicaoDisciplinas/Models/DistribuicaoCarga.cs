using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class DistribuicaoCarga
    {
        public int IdCenario { get; set; }
        public string Siape { get; set; }
        public string Regra { get; set; }
        public int CH { get; set; }
    }
}