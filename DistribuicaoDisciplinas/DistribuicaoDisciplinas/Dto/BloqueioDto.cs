using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class BloqueioDto
    {
        public ProfessorDto Professor { get; set; }
        public TurmaDto Turma { get; set; }
        public int IdFila { get; set; }
        public int Posicao { get; set; }
        public int Prioridade { get; set; }
        public BloqueioDto Dependente { get; set; }
    }
}