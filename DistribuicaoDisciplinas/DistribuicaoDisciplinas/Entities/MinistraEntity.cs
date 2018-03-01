using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("ministra")]
    public class MinistraEntity
    {
        [Key]
        [Column(Order = 0)]
        public string siape { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_turma { get; set; }

        [ForeignKey("siape")]
        public virtual ProfessorEntity Professor { get; set; }
        [ForeignKey("id_turma")]
        public virtual TurmaEntity Turma { get; set; }
    }
}