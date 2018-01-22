using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("fila_turma_new")]
    public class FilaTurmaEntity
    {
        [Key]
        [Column(Order = 0)]
        public int id_turma { get; set; }
        [Key]
        [Column(Order = 1)]
        public int id_fila { get; set; }
        public int? prioridade { get; set; }

        [ForeignKey("id_fila")]
        public virtual FilaEntity Fila { get; set; }
        [ForeignKey("id_turma")]
        public virtual TurmaEntity Turma { get; set; }
    }
}