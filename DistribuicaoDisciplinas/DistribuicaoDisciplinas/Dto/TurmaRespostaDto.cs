using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Dto
{
    public class TurmaRespostaDto
    {
        public int IdTurma { get; set; }
        public string Turma { get; set; }
        public string CodigoDisc { get; set; }
        public int Posicao { get; set; }
        public int Prioridade { get; set; }

        public StatusFilaAlgoritmo Status { get; set; }

    }
}