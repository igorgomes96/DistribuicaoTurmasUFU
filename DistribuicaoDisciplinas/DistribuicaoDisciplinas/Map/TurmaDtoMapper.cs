using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class TurmaDtoMapper : ISingleMapper<Turma, TurmaDto>
    {

        public TurmaDto Map(Turma source)
        {
            return new TurmaDto
            {
                Id = source.Id,
                CodigoDisc = source.CodigoDisc,
                LetraTurma = source.LetraTurma,
                NomeDisciplina = source.Disciplina.Nome,
                CH = source.CH,
                Disciplina = source.Disciplina
            };
        }

        public Turma Map(TurmaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}