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
        [Key]
        [Column("siape", Order = 0)]
        public string Siape { get; set; }
        [Key]
        [Column("id_turma", Order = 1)]
        public int IdTurma { get; set; }

        [ForeignKey("Siape")]
        public virtual Professor Professor { get; set; }
        [ForeignKey("IdTurma")]
        public virtual Turma Turma { get; set; }
    }
}