using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Exceptions
{
    public class ChoquePeriodoException: Exception
    {
        public ChoquePeriodoException() { }
        public ChoquePeriodoException(string message): base(message) { }
    }
}