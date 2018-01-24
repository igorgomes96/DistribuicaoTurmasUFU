﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Models
{
    public class Turma
    {
        private string letraTurma;
        public int Id { get; set; }
        public string CodigoDisc { get; set; }
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

        public Disciplina Disciplina { get; set; }

        public ICollection<FilaTurma> Posicoes { get; set; } = new List<FilaTurma>();
        public ICollection<Oferta> Horarios { get; set; } = new List<Oferta>();


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