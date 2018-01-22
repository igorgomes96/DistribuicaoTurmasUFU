using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Restricao
    {
        public string Siape { get; set; }
        public string Dia { get; set; }
        public string Letra { get; set; }

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