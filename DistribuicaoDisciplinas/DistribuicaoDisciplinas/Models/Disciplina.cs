using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Disciplina
    {
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int Periodo { get; set; }

        public Curso Curso { get; set; }

        public override bool Equals(object obj)
        {
            Disciplina disciplina = obj as Disciplina;
            return disciplina == null ? false : disciplina.Codigo == Codigo;
        }

        public override int GetHashCode()
        {
            return Codigo.GetHashCode();
        }



    }
}