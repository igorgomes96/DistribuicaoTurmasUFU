using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class ProfessorDtoMapper : ISingleMapper<Professor, ProfessorDto>
    {
        public ProfessorDto Map(Professor source)
        {
            return new ProfessorDto {
                Siape = source.Siape,
                Nome = source.Nome,
                CH =source.CH,
                DataIngresso = source.DataIngresso,
                CHAtribuida = source.CHAtribuida()
            };
        }

        public Professor Map(ProfessorDto destination)
        {
            throw new NotImplementedException();
        }
    }
}