using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("distribuicao_carga")]
    public class DistribuicaoCargaEntity
    {
        [Key]
        [Column("cenario", Order = 0)]
        public int IdCenario { get; set; }
        [Key]
        [Column("siape", Order = 1)]
        public string Siape { get; set; }
        [Key]
        [Column("regra", Order = 2)]
        public string Regra { get; set; }
        [Column("carga")]
        public int CH { get; set; }

        [ForeignKey("IdCenario")]
        public virtual CenarioEntity Cenario { get; set; }
        [ForeignKey("Siape")]
        public virtual ProfessorEntity Professor { get; set; }
    }
}