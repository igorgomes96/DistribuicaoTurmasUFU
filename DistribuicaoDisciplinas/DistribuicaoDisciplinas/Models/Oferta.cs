using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("oferta")]
    public class Oferta
    {
        public int Id { get; set; }
        public int IdTurma { get; set; }
        public string Dia { get; set; }
        public string Letra { get; set; }

        public Turma Turma { get; set; }
    }
}