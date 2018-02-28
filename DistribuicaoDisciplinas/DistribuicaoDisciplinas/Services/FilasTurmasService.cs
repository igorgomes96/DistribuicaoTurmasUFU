using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class FilasTurmasService : IFilasTurmasService
    {
        //private readonly IGenericRepository<FilaTurmaEntity> _filaTurmaRep;
        //private readonly IMapper<FilaTurma, FilaTurmaEntity> _filaTurmaMap;

        //public FilasTurmasService(IGenericRepository<FilaTurmaEntity> filaTurmaRep,
        //    IMapper<FilaTurma, FilaTurmaEntity> filaTurmaMap)
        //{
        //    _filaTurmaRep = filaTurmaRep;
        //    _filaTurmaMap = filaTurmaMap;
        //}

        //public ICollection<FilaTurma> List(int ano, int semestre)
        //{
        //    return _filaTurmaMap.Map(_filaTurmaRep.Query(
        //        f => f.Fila.ano == ano
        //        && f.Fila.semestre == semestre
        //        && f.Turma.ano == ano
        //        && f.Turma.semestre == semestre));
        //}

        private readonly IGenericRepository<FilaTurma> _filaTurmaRep;

        public FilasTurmasService(IGenericRepository<FilaTurma> filaTurmaRep)
        {
            _filaTurmaRep = filaTurmaRep;
        }

        public ICollection<FilaTurma> List(int ano, int semestre)
        {
            return _filaTurmaRep.Query(
                f => f.Fila.Ano == ano
                && f.Fila.Semestre == semestre
                && f.Turma.Ano == ano
                && f.Turma.Semestre == semestre);
        }
    }
}