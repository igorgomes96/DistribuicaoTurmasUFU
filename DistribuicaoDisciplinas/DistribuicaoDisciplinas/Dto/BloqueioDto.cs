using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Dto
{
    public class BloqueioDto
    {
        public int IdTurma { get; set; }
        public int IdFila { get; set; }
        public string Siape { get; set; }

        public string TipoBloqueio;
        public BloqueioDto Dependente { get; set; }
        public int Tamanho { get; set; }

    }
}