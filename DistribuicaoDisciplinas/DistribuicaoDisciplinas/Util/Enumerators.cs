using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Util
{
    public class Enumerators
    {
        public enum StatusFilaAlgoritmo
        {
            NaoAnalisadaAinda,
            Desconsiderada,  //As filas com maior prioridade completarão a CH do professor, com certeza
            EmEspera,  //Filas já analisadas, porém por algum motivo não foi possível fazer a atribuição até momento
            Atribuda,
            ChoqueHorario,
            ChoqueRestricao,
            ChoquePeriodo,
            OutroProfessor,
            CHCompleta,
            CHUltrapassadaSeAtribuida,
        }
    }
}