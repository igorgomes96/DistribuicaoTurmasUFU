using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Services
{
    public interface ICenariosFilasTurmasService
    {

        void Delete(CenarioFilaTurma cenarioFilaTurma);
        int Count(int numCenario);
        ICollection<CenarioFilaTurma> List(int numCenario);

    }
}