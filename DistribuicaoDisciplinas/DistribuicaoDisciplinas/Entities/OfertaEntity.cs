using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("oferta")]
    public class OfertaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int id { get; set; }
        public string dia { get; set; }
        public string letra { get; set; }
        public int id_turma { get; set; }

        [ForeignKey("id_turma")]
        public TurmaEntity Turma { get; set; }
    }
}