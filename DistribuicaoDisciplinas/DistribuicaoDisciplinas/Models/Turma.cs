using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("turma")]
    public class Turma
    {

        public Turma()
        {
            Posicoes = new HashSet<FilaTurma>();
            Horarios = new HashSet<Oferta>();
        }

        public int Id { get; set; }

        private string codigoDisc;
        public string CodigoDisc {
            get {
                return codigoDisc.Trim();
            }
            set
            {
                codigoDisc = value.Trim();
            }
        }

        private string letraTurma;
        public string LetraTurma
        {
            get
            {
                return letraTurma;
            }
            set
            {
                letraTurma = value.Trim();
            }
        }

        public int CH { get; set; }
        public int? Ano { get; set; }
        public int? Semestre { get; set; }

        public Disciplina Disciplina { get; set; }

        public ICollection<FilaTurma> Posicoes { get; set; }
        public ICollection<Oferta> Horarios { get; set; }


        public void JogarParaFinalFila(FilaTurma filaTurma)
        {
            Posicoes.Where(p => p.Fila.PosicaoReal > filaTurma.Fila.PosicaoReal).ToList().ForEach(p =>
            {
                p.Fila.PosicaoReal -= 1;
            });
            filaTurma.Fila.PosicaoReal = Posicoes.Max(x => x.Fila.PosicaoReal) + 1;
            OrdenaPosicoes();
        }

        public void OrdenaPosicoes()
        {
            Posicoes = Posicoes.OrderBy(x => x.Fila.PosicaoReal).ToList();
        }

        public bool TurmaPendente()
        {
            return !Posicoes.Any(x => x.StatusAlgoritmo == Util.Enumerators.StatusFila.Atribuida);
        }

        public override bool Equals(object obj)
        {
            Turma turma = obj as Turma;
            return turma != null && Id == turma.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }

    }
}