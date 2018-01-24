using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class ProfessorDto
    {
        public string Siape { get; set; }
        public string Nome { get; set; }
        public DateTime DataIngresso { get; set; }
        public int CH { get; set; }
    }
}