using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Repository;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class MinistraService : IMinistraService
    {
        private readonly IMinistraRepository _ministraRep;
        private readonly IMapper<Ministra, MinistraEntity> _ministraMap;

        public MinistraService(IMinistraRepository ministraRep,
            IMapper<Ministra, MinistraEntity> ministraMap)
        {
            _ministraRep = ministraRep;
            _ministraMap = ministraMap;
        }

        public ICollection<Ministra> List()
        {
            return _ministraMap.Map(_ministraRep.List());
        }

        public ICollection<Ministra> List(int ano, int semestre)
        {
            return _ministraMap.Map(_ministraRep.Query(m => m.Turma.ano == ano && m.Turma.semestre == semestre));
        }

        //public void LimparMinistra(int ano, int semestre, bool ignorarSemFila = true)
        //{
        //    if (ignorarSemFila)
        //        _ministraRep.Delete(x => x.Turma.ano == ano && x.Turma.semestre == semestre && x.Turma.FilasTurmas.Count == 0);
        //    else
        //        _ministraRep.Delete(x => x.Turma.ano == ano && x.Turma.semestre == semestre);
        //}


        public ICollection<Ministra> ListTurmasSemFila(int ano, int semestre)
        {
            return _ministraMap.Map(_ministraRep
                .Query(x => x.Turma.ano == ano && x.Turma.semestre == semestre && x.Turma.FilasTurmas.Count == 0));
        }

        public void SalvarDistribuicao(ICollection<Ministra> distribuicao)
        {
            _ministraRep.SalvarDistribuicao(_ministraMap.Map(distribuicao));
        }
    }
}