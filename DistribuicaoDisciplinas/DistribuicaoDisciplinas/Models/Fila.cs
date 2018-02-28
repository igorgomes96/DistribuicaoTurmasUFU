using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("fila")]
    public class Fila
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("pos")]
        public int Posicao { get; set; }
        [Column("qte_ministrada")]
        public int QtdaMinistrada { get; set; }
        [Column("qte_maximo")]
        public int QtdaMaxima { get; set; }
        [Column("siape")]
        public string Siape { get; set; }
        [Column("codigo_disc")]
        public string CodigoDisc { get; set; }
        [Column("ano")]
        public int Ano { get; set; }
        [Column("semestre")]
        public int Semestre { get; set; }

        [ForeignKey("Siape")]
        public virtual Professor Professor { get; set; }

        [ForeignKey("CodigoDisc")]
        public virtual Disciplina Disciplina { get; set; }

        //0, se QtdaMinistrada < QtdaMaxima; 1, caso contrário
        public int QtdaMaximaJaMinistrada
        {
            get
            {
                return QtdaMinistrada < QtdaMaxima ? 0 : 1;
            }
        }


        public override bool Equals(object obj)
        {
            Fila fila = obj as Fila;
            return fila == null ? false : fila.Id == Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }



    }
}