using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Exceptions
{
    public class RestricaoHorarioException: Exception
    {
        public RestricaoHorarioException() { }
        public RestricaoHorarioException(string message): base(message) { }
    }
}