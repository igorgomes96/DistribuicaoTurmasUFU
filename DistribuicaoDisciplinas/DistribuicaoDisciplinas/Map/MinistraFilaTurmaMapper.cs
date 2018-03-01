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
                    Professor = source.Professor,
                    Disciplina = source.Turma.Disciplina,
                    Posicao = -1,
                    QtdaMaxima = 1,
                    QtdaMinistrada = 0
                },
                Prioridade = -1,
                Turma = source.Turma
            };
        }

        public Ministra Map(FilaTurma destination)
        {
            throw new NotImplementedException();
        }
    }
}