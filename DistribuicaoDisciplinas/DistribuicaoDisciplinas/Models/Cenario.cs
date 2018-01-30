using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Cenario
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("num_cenario")]
        public int NumCenario { get; set; }

        [Column("descricao_cenario")]
        public string Descricao { get; set; }

        [Column("ano")]
        public int Ano { get; set; }

        [Column("semestre")]
        public int Semestre { get; set; }

    }
}