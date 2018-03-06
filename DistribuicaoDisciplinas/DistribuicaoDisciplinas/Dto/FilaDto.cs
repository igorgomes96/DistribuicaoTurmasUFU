using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class FilaDto
    {
        public int Id { get; set; }
        public string Siape { get; set; }
        public string CodigoDisc { get; set; }
        public int PosicaoReal { get; set; }
        public int QtdaMinistrada { get; set; }
        public int QtdaMaxima { get; set; }
        public bool QtdaMaximaJaMinistrada { get; set; }
    }
}