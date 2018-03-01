using DistribuicaoDisciplinas.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Services
{
    public interface IMinistraService
    {
        ICollection<Ministra> List();
        ICollection<Ministra> List(int ano, int semestre);

    }
}