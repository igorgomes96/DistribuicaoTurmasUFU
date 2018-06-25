using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class TurmasService : ITurmasService
    {

        private readonly IGenericRepository<TurmaEntity> _turmasRep;
        private readonly IMapper<Turma, TurmaEntity> _turmasMap;

        public TurmasService(IGenericRepository<TurmaEntity> turmasRep,
            IMapper<Turma, TurmaEntity> turmasMap)
        {
            _turmasRep = turmasRep;
            _turmasMap = turmasMap;
        }

        public bool ChoqueHorario(Turma turma1, Turma turma2)
        {
            return turma1.Horarios.Any(h => turma2.Horarios.Any(x => x.Dia == h.Dia && x.Letra == h.Letra));
        }

        public bool ChoqueHorario(Turma turma, ICollection<Turma> turmas)
        {
            return turmas.Any(x => ChoqueHorario(turma, x));
        }

        public bool ChoquePeriodo(Turma turma1, Turma turma2)
        {
            return turma1.Disciplina.Curso.Codigo == turma2.Disciplina.Curso.Codigo
                && turma1.Disciplina.Periodo == turma2.Disciplina.Periodo
                && (turma1.Disciplina.Codigo != turma2.Disciplina.Codigo || (turma1.LetraTurma != turma2.LetraTurma));
        }

        public bool ChoquePeriodo(Turma turma, ICollection<Turma> turmas)
        {
            return turmas.Any(x => ChoquePeriodo(turma, x));
        }

        public ICollection<Turma> TurmasSemFila(int ano, int semestre)
        {
            return _turmasMap.Map(_turmasRep.Query(x => x.ano == ano && x.semestre == semestre && x.FilasTurmas.Count == 0));
        }

        public ICollection<Turma> List(int ano, int semestre)
        {
            return _turmasMap.Map(_turmasRep.Query(t => t.ano == ano && t.semestre == semestre));
        }
    }
}