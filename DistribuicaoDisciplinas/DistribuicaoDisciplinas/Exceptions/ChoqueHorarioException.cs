using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Exceptions
{
    public class ChoqueHorarioException: Exception
    {
        public ChoqueHorarioException() { }
        public ChoqueHorarioException(string message): base(message) { }
    }
}