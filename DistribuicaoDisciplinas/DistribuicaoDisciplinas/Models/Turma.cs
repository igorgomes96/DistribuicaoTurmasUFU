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


        public void OrdenaPosicoes()
        {
            Posicoes = Posicoes.OrderBy(x => x.Fila.Posicao).ToList();
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