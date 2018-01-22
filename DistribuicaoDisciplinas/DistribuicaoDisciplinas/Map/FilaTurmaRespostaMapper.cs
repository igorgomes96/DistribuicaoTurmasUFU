using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class FilaTurmaRespostaMapper : ISingleMapper<FilaTurma, FilaTurmaRespostaDto>
    {
        public FilaTurmaRespostaDto Map(FilaTurma source)
        {
            return new FilaTurmaRespostaDto {
                Siape = source.Fila.Professor.Siape,
                Professor = source.Fila.Professor.Nome,
                IdTurma = source.Turma.Id,
                Turma = source.Turma.LetraTurma,
                CodigoDisc = source.Turma.CodigoDisc,
                Posicao = source.Fila.Posicao,
                Prioridade = source.Prioridade,
                Status = source.StatusAlgoritmo
            };
        }

        public FilaTurma Map(FilaTurmaRespostaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}