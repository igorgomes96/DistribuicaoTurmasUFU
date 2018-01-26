using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class TurmaDto
    {
        public int Id { get; set; }
        public string CodigoDisc { get; set; }
        public string NomeDisciplina { get; set; }
        public string LetraTurma { get; set; }
        public int CH { get; set; }

        public Disciplina Disciplina { get; set; }

    }
}