using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Dto
{
    public class FilaTurmaDto
    {
        public FilaDto Fila { get; set; }
        public int IdTurma { get; set; }
        public int Prioridade { get; set; }

        public StatusFila Status { get; set; }
    }
}