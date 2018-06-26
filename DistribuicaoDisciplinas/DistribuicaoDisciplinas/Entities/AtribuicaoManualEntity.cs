using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("atribuicao_manual")]
    public class AtribuicaoManualEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 0)]
        public int num_cenario { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int id_turma { get; set; }

        public string siape { get; set; }

        [ForeignKey("num_cenario")]
        public virtual CenarioEntity Cenario { get; set; }
        [ForeignKey("siape")]
        public virtual ProfessorEntity Professor { get; set; }
        [ForeignKey("id_turma")]
        public virtual TurmaEntity Turma { get; set; }
    }
}