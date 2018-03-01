using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("disciplina")]
    public class DisciplinaEntity
    {
        [Key]
        public string codigo { get; set; }
        public string nome { get; set; }
        public int? ch_teorica { get; set; }
        public int? ch_pratica { get; set; }
        public int? ch_total { get; set; }
        public string curso { get; set; }
        public bool? temfila { get; set; }
        public int? periodo { get; set; }
        public string cod_antigo { get; set; }


        [ForeignKey("curso")]
        public virtual CursoEntity Curso { get; set; }
    }
}