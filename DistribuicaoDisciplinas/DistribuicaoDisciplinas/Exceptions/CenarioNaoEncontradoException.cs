using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Exceptions
{
    public class CenarioNaoEncontradoException : Exception
    {
        public CenarioNaoEncontradoException() { }
        public CenarioNaoEncontradoException(string message): base(message) { }
    }
}