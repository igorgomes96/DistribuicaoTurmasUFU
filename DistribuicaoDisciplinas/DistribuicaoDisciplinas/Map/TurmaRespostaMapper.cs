using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class TurmaRespostaMapper : ISingleMapper<FilaTurma, TurmaRespostaDto>
    {
        public TurmaRespostaDto Map(FilaTurma source)
        {
            return new TurmaRespostaDto {
                IdTurma = source.Turma.Id,
                Turma = source.Turma.LetraTurma,
                CodigoDisc = source.Turma.CodigoDisc,
                NomeDisciplina  = source.Turma.Disciplina.Nome,
                CH = source.Turma.CH,
                Prioridade = source.Prioridade,
                Posicao = source.Fila.Posicao,
                QtdaMaxima = source.Fila.QtdaMaxima,
                QtdaMinistrada = source.Fila.QtdaMinistrada,
                Status = source.StatusAlgoritmo
            };
        }

        public FilaTurma Map(TurmaRespostaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}