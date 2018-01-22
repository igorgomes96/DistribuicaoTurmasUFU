using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Ministra
    {
        public Professor Professor { get; set; }
        public Turma Turma { get; set; }
    }
}