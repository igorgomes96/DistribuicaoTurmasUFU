using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Dto
{
    public class RespostaDto
    {
        //public ICollection<ProfessorPrioridadesDto> Professores { get; set; }
        //public ICollection<TurmaPosicoesDto> TurmasPendentes { get; set; }
        //public ICollection<BloqueioDto> Bloqueios { get; set; }

        public ICollection<ProfessorDto> Professores { get; set; }
        public ICollection<TurmaDto> Turmas { get; set; }
        public ICollection<FilaTurmaDto> FilasTurmas { get; set; }
        public ICollection<BloqueioDto> Bloqueios { get; set; }
        public ICollection<int> TurmasPendentes { get; set; }
        
    }
}