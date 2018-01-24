using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    public class Bloqueio
    {
        public TipoBloqueio TipoBloqueio;
        public FilaTurma FilaTurma;
        public Bloqueio Dependente;
    }
}