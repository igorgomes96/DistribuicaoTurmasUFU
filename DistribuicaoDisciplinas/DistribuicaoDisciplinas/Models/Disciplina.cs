using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("disciplina")]
    public class Disciplina
    {

        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int? CHTeorica { get; set; }
        public int? CHPratica { get; set; }
        public int Periodo { get; set; }
        public string CodigoCurso { get; set; }

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