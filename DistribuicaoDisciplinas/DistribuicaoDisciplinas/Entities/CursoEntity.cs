using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("curso")]
    public class CursoEntity
    {
        [Key]
        public string codigo { get; set; }
        public string nome { get; set; }
        public string unidade { get; set; }
        public string campus { get; set; }
    }
}