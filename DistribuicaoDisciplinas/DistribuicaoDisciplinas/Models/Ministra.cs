using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("ministra")]
    public class Ministra
    {
        public string Siape { get; set; }
        public int IdTurma { get; set; }

        public Professor Professor { get; set; }
        public Turma Turma { get; set; }
    }
}