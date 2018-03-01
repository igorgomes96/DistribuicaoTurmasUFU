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
        [Key]
        [Column("codigo")]
        public string Codigo { get; set; }
        [Column("nome")]
        public string Nome { get; set; }
        [Column("ch_teorica")]
        public int? CHTeorica { get; set; }
        [Column("ch_pratica")]
        public int? CHPratica { get; set; }
        [Column("periodo")]
        public int Periodo { get; set; }
        [Column("curso")]
        public string CodigoCurso { get; set; }

        [ForeignKey("CodigoCurso")]
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