using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    [Table("fila_turma_new")]
    public class FilaTurma
    {
        public int IdTurma { get; set; }
        public int IdFila { get; set; }
        //Prioridade cadastrada
        public int PrioridadeBanco { get; set; }

        //Prioridade utilizada no momento da distribuição. Pode mudar
        //se o professor já houver ministrado a quantidade máxima de
        //vezes, ou em caso de quebra de deadlock
        public int PrioridadeReal { get; set; }
        
        public Fila Fila { get; set; }
        public Turma Turma { get; set; }
        
        public StatusFila StatusAlgoritmo { get; set; }

    }
}