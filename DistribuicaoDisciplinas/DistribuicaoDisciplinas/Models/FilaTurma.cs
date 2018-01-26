using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    public class FilaTurma
    {
        public Fila Fila { get; set; }
        public Turma Turma { get; set; }
        public int Prioridade { get; set; }
        public StatusFila StatusAlgoritmo { get; set; }

    }
}