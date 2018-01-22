using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Fila
    {
        public int Id { get; set; }
        public Professor Professor { get; set; }
        public Disciplina Disciplina { get; set; }
        public int Posicao { get; set; }
        public int QtdaMinistrada { get; set; }
        public int QtdaMaxima { get; set; }

        //0, se QtdaMinistrada < QtdaMaxima; 1, caso contrário
        public int QtdaMaximaJaMinistrada()
        {
            return QtdaMinistrada < QtdaMaxima ? 0 : 1;
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