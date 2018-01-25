using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Dto
{
    public class ProfessorPrioridadesDto
    {
        public ProfessorDto Professor { get; set; }
        public ICollection<TurmaRespostaDto> Prioridades { get; set; }

        //Variável usada para indicar se o professor pode ministrar determinada disciplina,
        //quando o front-end solicitar a informação para uma disciplina específica
        public StatusFila StatusMinistra { get; set; }
    }
}