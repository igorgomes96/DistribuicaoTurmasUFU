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
    public class MinistraService : IMinistraService
    {
        //private readonly IGenericRepository<MinistraEntity> _ministraRep;
        //private readonly IMapper<Ministra, MinistraEntity> _ministraMap;

        //public MinistraService(IGenericRepository<MinistraEntity> ministraRep,
        //    IMapper<Ministra, MinistraEntity> ministraMap)
        //{
        //    _ministraRep = ministraRep;
        //    _ministraMap = ministraMap;
        //}

        //public ICollection<Ministra> List()
        //{
        //    return _ministraMap.Map(_ministraRep.List());
        //}

        //public ICollection<Ministra> List(int ano, int semestre)
        //{
        //    return _ministraMap.Map(_ministraRep.Query(m => m.Turma.ano == ano && m.Turma.semestre == semestre));
        //}

        private readonly IGenericRepository<Ministra> _ministraRep;

        public MinistraService(IGenericRepository<Ministra> ministraRep)
        {
            _ministraRep = ministraRep;
        }

        public ICollection<Ministra> List()
        {
            return _ministraRep.List();
        }

        public ICollection<Ministra> List(int ano, int semestre)
        {
            return _ministraRep.Query(m => m.Turma.Ano == ano && m.Turma.Semestre == semestre);
        }
    }
}