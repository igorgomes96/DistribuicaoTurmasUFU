using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Util
{
    public static class Constants
    {
        public const int CH_DEFAULT = 4;
        public static class TipoBloqueio
        {
            public const string Deadlock = "Deadlock";
            public const string DisciplinaCHDiferente4 = "Disciplina com CH diferente de 4 horas";
        }
    }
}