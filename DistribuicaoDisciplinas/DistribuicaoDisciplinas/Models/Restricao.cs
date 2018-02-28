using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("restricoes")]
    public class Restricao
    {
        [Key]
        [Column("siape", Order = 0)]
        public string Siape { get; set; }
        [Key]
        [Column("dia", Order = 1)]
        public string Dia { get; set; }
        [Key]
        [Column("letra", Order = 2)]
        public string Letra { get; set; }

        [ForeignKey("Siape")]
        public virtual Professor Professor { get; set; }

        public override bool Equals(object obj)
        {
            Restricao restricao = obj as Restricao;
            return restricao == null ? false : restricao.Dia == Dia && restricao.Letra == Letra && restricao.Siape == Siape;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}