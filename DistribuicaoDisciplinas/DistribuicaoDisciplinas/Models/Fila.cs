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
        public int Id { get; set; }
        public int PosicaoBanco { get; set; }
        public int PosicaoReal { get; set; }
        public int QtdaMinistrada { get; set; }
        public int QtdaMaxima { get; set; }
        public string Siape { get; set; }
        public string CodigoDisc { get; set; }
        public int Ano { get; set; }
        public int Semestre { get; set; }

        public Professor Professor { get; set; }
        public Disciplina Disciplina { get; set; }

        public bool QtdaMaximaJaMinistrada
        {
            get
            {
                return QtdaMinistrada >= QtdaMaxima;
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