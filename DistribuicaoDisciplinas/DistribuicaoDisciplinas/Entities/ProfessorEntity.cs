using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("professor")]
    public class ProfessorEntity
    {
        [Key]
        public string siape { get; set; }
        public string nome { get; set; }
        public DateTime? data_ingresso { get; set; }
        public DateTime? data_nasc { get; set; }
        public bool afastado { get; set; }
        public string regime { get; set; }
        public int? carga_atual { get; set; }
        public string lotacao { get; set; }
        public string cnome { get; set; }
        public DateTime? data_saida { get; set; }
        public DateTime? data_exoneracao { get; set; }
        public DateTime? data_aposentadoria { get; set; }
        public string status { get; set; }

        public virtual ICollection<RestricaoEntity> Restricoes { get; set; }

    }
}