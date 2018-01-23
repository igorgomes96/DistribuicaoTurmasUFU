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
            Desconsiderada,     //FilaTurma que está com uma prioridade muito baixa e as que estão com prioridade maior com certeza completarão a CH do professor
            Atribuda,           //FilaTurma atribuída
            NaoAnalisadaAinda,  //FilaTurma não desconsiderada, porém o algoritmo ainda não chegou à ela.
            EmEspera,           //FilaTurma analisada, porém o professor não está na primeira posição da turma
            ChoqueHorario,      //FilaTurma em que a turma choca horário com outra já atribuída ao professor
            ChoqueRestricao,    //FilaTurma em que a turma choca com um horário que o professor estará indisponível
            ChoquePeriodo,      //FilaTurma em que a turma choca período com outra já atribuída ao professor
            OutroProfessor,     //FilaTurma em que a turma já foi atribuída a outro professor no momento da análise
            CHCompleta          //FilaTurma em que o professor já está com CH completa no momento em que foi atribuída a outro professor
        }
    }
}