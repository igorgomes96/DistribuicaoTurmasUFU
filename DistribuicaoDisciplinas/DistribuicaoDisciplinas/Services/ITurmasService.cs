using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Services
{
    public interface ITurmasService
    {
        ICollection<Turma> List(int ano, int semestre);
        bool ChoqueHorario(Turma turma1, Turma turma2);
        bool ChoquePeriodo(Turma turma1, Turma turma2);
        bool ChoqueHorario(Turma turma, ICollection<Turma> turmas);
        bool ChoquePeriodo(Turma turma, ICollection<Turma> turmas);
        ICollection<Turma> TurmasSemFila(int ano, int semestre);

    }
}
