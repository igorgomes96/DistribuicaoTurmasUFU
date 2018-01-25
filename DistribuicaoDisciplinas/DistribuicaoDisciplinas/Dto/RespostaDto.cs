using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class RespostaDto
    {
        public ICollection<ProfessorPrioridadesDto> Professores { get; set; }
        public ICollection<TurmaDto> TurmasPendentes { get; set; }
        public ICollection<Bloqueio> Bloqueios { get; set; }
        
    }
}