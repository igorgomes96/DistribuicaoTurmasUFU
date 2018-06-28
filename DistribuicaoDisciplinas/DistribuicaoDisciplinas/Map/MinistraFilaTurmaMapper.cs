using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class MinistraFilaTurmaMapper : ISingleMapper<Ministra, FilaTurma>
    {
        public FilaTurma Map(Ministra source)
        {
            return new FilaTurma
            {
                Fila = new Fila
                {
                    Id = -1,
                    Professor = source.Professor,
                    Disciplina = source.Turma.Disciplina,
                    PosicaoReal = -1,
                    QtdaMaxima = 1,
                    QtdaMinistrada = 0
                },
                PrioridadeReal = -1,
                Turma = source.Turma
            };
        }

        public Ministra Map(FilaTurma destination)
        {
            throw new NotImplementedException();
        }
    }
}