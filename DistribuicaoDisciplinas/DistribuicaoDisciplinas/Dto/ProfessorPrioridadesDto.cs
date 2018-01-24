using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class ProfessorPrioridadesDto
    {
        public ProfessorDto Professor { get; set; }
        public ICollection<TurmaRespostaDto> Prioridades { get; set; }
    }
}