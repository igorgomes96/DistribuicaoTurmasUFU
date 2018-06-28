using DistribuicaoDisciplinas.Entities;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
    public class Program
    {
        static void Main(string[] args)
        {
            DbContext db = new Modelo();
            GenericRepository<AtribuicaoManualEntity> rep = new GenericRepository<AtribuicaoManualEntity>(db);

            AtribuicaoManualEntity at = rep.Find(3, 974);

        }
    }
}
