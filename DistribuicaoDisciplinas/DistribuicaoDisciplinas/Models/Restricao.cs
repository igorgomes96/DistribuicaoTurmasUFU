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
        public string Siape { get; set; }
        public string Dia { get; set; }
        public string Letra { get; set; }

        public virtual Professor Professor { get; set; }

        public override bool Equals(object obj)
        {
            Restricao restricao = obj as Restricao;
            return restricao.Dia == Dia && restricao.Letra == Letra && restricao.Siape == Siape;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}