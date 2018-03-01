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
    public class CenariosService : ICenariosService
    {
        private readonly IGenericRepository<CenarioEntity> _rep;
        private readonly IMapper<Cenario, CenarioEntity> _map;

        public CenariosService(IGenericRepository<CenarioEntity> rep,
            IMapper<Cenario, CenarioEntity> map)
        {
            _rep = rep;
            _map = map;
        }
        public Cenario Find(int num)
        {
            return _map.Map(_rep.Find(num));
        }

    }
}