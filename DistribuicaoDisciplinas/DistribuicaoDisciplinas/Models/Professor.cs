using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    [Table("professor")]
    public class Professor
    {
        public Professor()
        {
            Restricoes = new HashSet<Restricao>();
            Prioridades = new HashSet<FilaTurma>();
        }

        [Key]
        [Column("siape")]
        public string Siape { get; set; }

        [Column("nome")]
        public string Nome { get; set; }
        [Column("data_ingresso")]
        public DateTime DataIngresso { get; set; }
        [Column("data_nasc")]
        public DateTime? DataNascimento { get; set; }
        [NotMapped]
        public int CH { get; set; }

        public virtual ICollection<Restricao> Restricoes { get; set; }

        [NotMapped]
        public ICollection<FilaTurma> Prioridades { get; set; }


        public int CHAtribuida()
        {
            return Prioridades.Where(p => p.StatusAlgoritmo == StatusFila.Atribuida)
                .Select(p => p.Turma.CH).Sum();
        }

        /// <summary>
        /// Verifica se as turmas atribuídas são suficientes para completas a carga horário do professor
        /// </summary>
        /// <returns></returns>
        public bool CHCompletaAtribuida()
        {
            return CHAtribuida() >= CH;
        }

        
        /// <summary>
        /// Retorna a soma da CH das turmas em espera com menor prioridade que a turma passada por parâmetro
        /// </summary>
        /// <param name="filaTurma"></param>
        /// <returns>Soma da CH</returns>
        public int CHEmEspera(FilaTurma filaTurma)
        {
            int ch = 0;
            foreach (FilaTurma ft in Prioridades)
            {
                if (ft.Fila.Id.Equals(filaTurma.Fila.Id) && ft.Turma.Id.Equals(filaTurma.Turma.Id))
                    break;

                if (ft.StatusAlgoritmo == StatusFila.EmEspera)
                    ch += ft.Turma.CH;
            }

            return ch;
        }

        public override bool Equals(object obj)
        {
            Professor prof = obj as Professor;
            return prof != null && Siape.Trim() == prof.Siape.Trim();
        }

        public override int GetHashCode()
        {
            return Siape.GetHashCode();
        }

    }
}