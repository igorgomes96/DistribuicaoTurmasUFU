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
        public TurmaDto Turma { get; set; }
        public int Posicao { get; set; }
        public int Prioridade { get; set; }
        public int QtdaMaxima { get; set; }
        public int QtdaMinistrada { get; set; }

        public StatusFila Status { get; set; }

    }
}