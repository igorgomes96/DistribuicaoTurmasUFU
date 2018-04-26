using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Exceptions
{
    public class JaAtribuidaException : Exception
    {
        public JaAtribuidaException() { }
        public JaAtribuidaException(string message) : base(message) { }
    }
}