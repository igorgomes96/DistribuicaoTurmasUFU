using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    [Table("fila_turma_new")]
    public class FilaTurma
    {
        [Key]
        [Column("id_turma", Order = 0)]
        public int IdTurma { get; set; }
        [Key]
        [Column("id_fila", Order = 1)]
        public int IdFila { get; set; }
        [Column("prioridade")]
        public int Prioridade { get; set; }
        
        [ForeignKey("IdFila")]
        public Fila Fila { get; set; }
        [ForeignKey("IdTurma")]
        public Turma Turma { get; set; }
        
        [NotMapped]
        public StatusFila StatusAlgoritmo { get; set; }

    }
}