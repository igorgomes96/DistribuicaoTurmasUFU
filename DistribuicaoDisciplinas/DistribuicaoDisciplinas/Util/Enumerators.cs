using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Util
{
    public class Enumerators
    {
        public enum StatusFila
        {
            Desconsiderada = -1,     //FilaTurma que está com uma prioridade muito baixa e as que estão com prioridade maior com certeza completarão a CH do professor
            Atribuida = 1,          //FilaTurma atribuída
            NaoAnalisadaAinda = 2,  //FilaTurma não desconsiderada, porém o algoritmo ainda não chegou à ela.
            EmEspera = 3,           //FilaTurma analisada, porém o professor não está na primeira posição da turma
            ChoqueHorario = 4,      //FilaTurma em que a turma choca horário com outra já atribuída ao professor
            ChoqueRestricao = 5,    //FilaTurma em que a turma choca com um horário que o professor estará indisponível
            ChoquePeriodo = 6,      //FilaTurma em que a turma choca período com outra já atribuída ao professor
            OutroProfessor = 7,     //FilaTurma em que a turma já foi atribuída a outro professor no momento da análise
            CHCompleta = 8,         //FilaTurma em que o professor já está com CH completa no momento em que foi atribuída a outro professor
            UltrapassariaCH = 9,    //FilaTurma que se atribuída faria a CH do professor ser ultrapassada
        }

        public enum TipoBloqueio
        {
            Deadlock,
            DisciplinaCHDiferente4
        }

    }
}