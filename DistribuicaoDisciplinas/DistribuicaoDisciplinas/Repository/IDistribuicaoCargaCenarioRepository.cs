using DistribuicaoDisciplinas.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Repository
{
    public interface IDistribuicaoCargaCenarioRepository : IGenericRepository<DistribuicaoCargaEntity>
    {
        void DeleteByCenario(int numCenario);
        void CleanContext();
    }
}