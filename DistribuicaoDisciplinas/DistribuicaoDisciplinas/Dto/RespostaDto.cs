using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class RespostaDto
    {

        public ICollection<ProfessorRespostaDto> Professores { get; set; }
        public ICollection<TurmaRespostaDto> TurmasPendentes { get; set; }
    }
}