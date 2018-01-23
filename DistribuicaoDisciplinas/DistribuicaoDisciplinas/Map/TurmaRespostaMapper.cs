using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class TurmaRespostaMapper : ISingleMapper<Turma, TurmaRespostaDto>
    {
        public TurmaRespostaDto Map(Turma source)
        {
            return new TurmaRespostaDto {
                IdTurma = source.Id,
                Turma = source.LetraTurma,
                CodigoDisc = source.CodigoDisc
            };
        }

        public Turma Map(TurmaRespostaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}