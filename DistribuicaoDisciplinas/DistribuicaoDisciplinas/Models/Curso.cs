using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("curso")]
    public class Curso
    {
        [Key]
        [Column("codigo")]
        public string Codigo { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("unidade")]
        public string Unidade { get; set; }

        [Column("campus")]
        public string Campus { get; set; }
    }
}