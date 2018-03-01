using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Entities
{
    [Table("fila")]
    public class FilaEntity
    {
        [Key]
        public int id { get; set; }
        public string siape { get; set; }
        public string codigo_disc { get; set; }
        public int? pos { get; set; }
        public int? prioridade { get; set; }
        public int? qte_ministrada { get; set; }
        public int? qte_maximo { get; set; }
        public int? ano { get; set; }
        public int? semestre { get; set; }

        [ForeignKey("siape")]
        public virtual ProfessorEntity Professor { get; set; }
        [ForeignKey("codigo_disc")]
        public virtual DisciplinaEntity Disciplina { get; set; }

        public virtual ICollection<FilaTurmaEntity> FilasTurmas { get; set; }

        /// <summary>
        /// Verifica se o professor já ministrou a disciplina a quantidade máxima de vezes
        /// </summary>
        /// <returns>1, Sim; 0, Não</returns>
        public int QtdaMaximaJaMinistrada() => qte_maximo >= qte_ministrada ? 1 : 0;
    }
}