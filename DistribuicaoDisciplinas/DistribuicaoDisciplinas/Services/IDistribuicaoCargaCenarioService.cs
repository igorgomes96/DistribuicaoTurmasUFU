using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Services
{
    public interface IDistribuicaoCargaCenarioService
    {
        int GetCargaProfessor(int codigoCenario, string siape);
        void CHPadraoPorCenario(int codigoCenario);
        void AtualizaCH(int numCenario, ICollection<DistribuicaoCargaEntity> carga);
    }
}
