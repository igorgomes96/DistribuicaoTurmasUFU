using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("cenario_fila_turma")]
    public class CenarioFilaTurmaEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 0)]
        public int num_cenario { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 1)]
        public int id_turma { get; set; }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column(Order = 2)]
        public int id_fila { get; set; }
        public int posicao { get; set; }
        public int prioridade { get; set; }
        public StatusFila status { get; set; }

        [ForeignKey("num_cenario")]
        public virtual CenarioEntity Cenario { get; set; }
        [ForeignKey("id_turma, id_fila")]
        public virtual FilaTurmaEntity FilaTurma { get; set; }
    }
}