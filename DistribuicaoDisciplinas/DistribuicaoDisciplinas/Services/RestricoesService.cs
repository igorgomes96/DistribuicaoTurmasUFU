using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Models;

namespace DistribuicaoDisciplinas.Services
{
    public class RestricoesService : IRestricoesService
    {
        public bool TemRestricao(Professor professor, Turma turma)
        {
            return professor.Restricoes.Any(r => turma.Horarios.Any(h => h.Dia == r.Dia && h.Letra == r.Letra));
        }
    }
}