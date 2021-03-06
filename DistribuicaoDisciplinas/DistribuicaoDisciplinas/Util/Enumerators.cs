﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Util
{
    public static class Enumerators
    {
        public enum StatusFila
        {
            NaoAnalisadaAinda = 0,  //FilaTurma não desconsiderada, porém o algoritmo ainda não chegou à ela.
            Atribuida = 1,          //FilaTurma atribuída
            Desconsiderada = 2,     //FilaTurma foi removida durante a distriuição
            EmEspera = 3,           //FilaTurma analisada, porém o professor não está na primeira posição da turma
            ChoqueHorario = 4,      //FilaTurma em que a turma choca horário com outra já atribuída ao professor
            ChoqueRestricao = 5,    //FilaTurma em que a turma choca com um horário que o professor estará indisponível
            ChoquePeriodo = 6,      //FilaTurma em que a turma choca período com outra já atribuída ao professor
            OutroProfessor = 7,     //FilaTurma em que a turma já foi atribuída a outro professor no momento da análise
            CHCompleta = 8,         //FilaTurma em que o professor já está com CH completa no momento em que foi atribuída a outro professor
            UltrapassariaCH = 9,    //FilaTurma que se atribuída faria a CH do professor ser ultrapassada
            //CHLimite = 10           //Se a turma for atribuída, o professor ultrapassará a ch do professor, mas ficará igual ou abaixo do limite CH_LIMITE
        }

        public enum ValidaAtribuicao
        {
            Valida,
            JaAtribuida,
            ChoqueHorario,
            ChoquePeriodo,
            RestricaoHorario
        }

    }
}