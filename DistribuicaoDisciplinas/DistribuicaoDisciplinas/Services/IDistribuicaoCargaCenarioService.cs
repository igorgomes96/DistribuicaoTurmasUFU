using DistribuicaoDisciplinas.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Services
{
    public interface IDistribuicaoCargaCenarioService
    {
        void CHPadraoPorCenario(int codigoCenario);
        void AtualizaCH(int numCenario, ICollection<DistribuicaoCargaEntity> carga);
    }
}
