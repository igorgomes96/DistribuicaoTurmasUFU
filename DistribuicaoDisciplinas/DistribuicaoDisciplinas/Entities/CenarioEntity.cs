using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("cenario")]
    public class CenarioEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int num_cenario { get; set; }
        public string descricao_cenario { get; set; }
        public int ano { get; set; }
        public int semestre { get; set; }

        public virtual ICollection<CenarioFilaTurmaEntity> FilasTurmas { get; set; }
    }
}