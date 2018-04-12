using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Cenario
    {

        public int NumCenario { get; set; }
        public string Descricao { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }

        public ICollection<CenarioFilaTurma> FilasTurmasStatus { get; set; }

    }
}