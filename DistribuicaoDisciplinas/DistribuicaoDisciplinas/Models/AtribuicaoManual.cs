using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class AtribuicaoManual
    {
        public Cenario Cenario { get; set; }
        public Professor Professor { get; set; }
        public Turma Turma { get; set; }
    }
}