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
            //CHsPorCenarios = new HashSet<DistribuicaoCarga>();
        }

        private string siape;
        public string Siape {
            get { return siape.Trim(); }
            set { siape = value.Trim(); }
        }

        public string Nome { get; set; }
        public DateTime DataIngresso { get; set; }
        public DateTime DataNascimento { get; set; }
        public int CH { get; set; }
        public bool Afastado { get; set; } 

        public ICollection<Restricao> Restricoes { get; set; }
        public ICollection<FilaTurma> Prioridades { get; set; }
        //public ICollection<DistribuicaoCarga> CHsPorCenarios { get; set; }

        public FilaTurma PrimeiraPrioridadeDisponivel()
        {
            return Prioridades
                .Where(x => x.StatusAlgoritmo == StatusFila.EmEspera || x.StatusAlgoritmo == StatusFila.NaoAnalisadaAinda)
                .OrderBy(x => x.PrioridadeReal)
                .FirstOrDefault();
        }

        /// <summary>
        /// Altera a prioridade para a última.
        /// </summary>
        /// <param name="filaTurma"></param>
        public void JogarParaUltimaPrioridadeReal(FilaTurma filaTurma)
        {
            Prioridades.Where(p => p.PrioridadeReal > filaTurma.PrioridadeReal).ToList().ForEach(p =>
            {
                p.PrioridadeReal -= 1;
            });
            filaTurma.PrioridadeReal = Prioridades.Max(x => x.PrioridadeReal) + 1;
            OrdenaPrioridades();
        }

        public void OrdenaPrioridades()
        {
            Prioridades = Prioridades.OrderBy(x => x.PrioridadeReal).ToList();
        }

        /// <summary>
        /// CH do professor para um cenário específico.
        /// </summary>
        /// <param name="idCenario"></param>
        /// <returns></returns>
        //public int CHCenario(int idCenario)
        //{
        //    return CHsPorCenarios.Where(x => x.IdCenario == idCenario).Select(x => x.CH).Sum();
        //}

        /// <summary>
        /// CH já atribuída.
        /// </summary>
        /// <returns></returns>
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