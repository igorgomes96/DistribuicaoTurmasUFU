using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    [Table("oferta")]
    public class Oferta
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }
        [Column("id_turma")]
        public int IdTurma { get; set; }
        [Column("dia")]
        public string Dia { get; set; }
        [Column("letra")]
        public string Letra { get; set; }

        [ForeignKey("IdTurma")]
        public Turma Turma { get; set; }
    }
}