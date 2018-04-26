using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DistribuicaoDisciplinas.Services
{
    public interface ICenariosService
    {
        ICollection<Cenario> List();
        Cenario Find(int num);
        Cenario DuplicarCenario(int cenarioBase, Cenario novoCenario);
        Cenario NovoCenario(Cenario cenario);
        void DeleteCenario(int idCenario);
    }
}
