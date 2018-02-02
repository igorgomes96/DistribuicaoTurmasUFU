using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static DistribuicaoDisciplinas.Util.Enumerators;

namespace DistribuicaoDisciplinas.Models
{
    public class Professor
    {

        private string siape;
        public string Siape {
            get { return siape.Trim(); }
            set { siape = value.Trim(); }
        }
        public string Nome { get; set; }
        public DateTime DataIngresso { get; set; }
        public int CH { get; set; }

        public ICollection<Restricao> Restricoes { get; set; } = new List<Restricao>();
        public ICollection<FilaTurma> Prioridades { get; set; } = new List<FilaTurma>();

        //public int CHEmEspera()
        //{
        //    return Prioridades.Where(p => p.StatusAlgoritmo == StatusFila.EmEspera)
        //        .Select(p => p.Turma.CH).Sum();
        //}

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