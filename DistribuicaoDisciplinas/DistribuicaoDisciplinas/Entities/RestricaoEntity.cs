using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("restricoes")]
    public class RestricaoEntity
    {
        [Key]
        [Column(Order = 0)]
        public string siape { get; set; }
        [Key]
        [Column(Order = 1)]
        public string dia { get; set; }
        [Key]
        [Column(Order = 2)]
        public string letra { get; set; }

        [ForeignKey("siape")]
        public virtual ProfessorEntity Professor { get; set; }
    }
}