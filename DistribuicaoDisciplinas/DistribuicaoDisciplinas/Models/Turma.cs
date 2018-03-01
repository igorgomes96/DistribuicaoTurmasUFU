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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        [NotMapped]
        private string codigoDisc;
        [Column("codigo_disc")]
        public string CodigoDisc {
            get {
                return codigoDisc.Trim();
            }
            set
            {
                codigoDisc = value.Trim();
            }
        }

        [NotMapped]
        private string letraTurma;
        [Column("turma")]
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

        [Column("ch")]
        public int CH { get; set; }
        [Column("ano")]
        public int? Ano { get; set; }
        [Column("semestre")]
        public int? Semestre { get; set; }

        [ForeignKey("CodigoDisc")]
        public Disciplina Disciplina { get; set; }

        public ICollection<FilaTurma> Posicoes { get; set; }
        public ICollection<Oferta> Horarios { get; set; }

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