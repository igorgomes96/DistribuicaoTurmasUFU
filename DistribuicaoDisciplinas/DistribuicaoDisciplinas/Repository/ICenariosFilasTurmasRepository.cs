using DistribuicaoDisciplinas.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Repository
{
    public interface ICenariosFilasTurmasRepository : IGenericRepository<CenarioFilaTurmaEntity>
    {
        void DeleteByCenario(int numCenario);
        void SaveDistribuicao(ICollection<CenarioFilaTurmaEntity> distribuicao);
    }
}
