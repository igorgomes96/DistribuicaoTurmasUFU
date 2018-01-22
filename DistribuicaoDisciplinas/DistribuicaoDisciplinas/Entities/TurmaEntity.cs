using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("turma")]
    public class TurmaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string codigo_disc { get; set; }
        public string turma { get; set; }
        public int? ch { get; set; }
        public int? ano { get; set; }
        public int? semestre { get; set; }

        [ForeignKey("codigo_disc")]
        public virtual DisciplinaEntity Disciplina { get; set; }
        public virtual ICollection<OfertaEntity> Horarios { get; set; }
        public virtual ICollection<FilaTurmaEntity> FilasTurmas { get; set; }
    }
}