using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class CenarioDto
    {
        public int NumCenario { get; set; }
        public string Descricao { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }
    }
}