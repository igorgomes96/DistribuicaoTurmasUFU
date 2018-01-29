using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class FilaDtoMapper : ISingleMapper<Fila, FilaDto>
    {

        public FilaDto Map(Fila source)
        {
            return new FilaDto
            {
                Id = source.Id,
                Siape = source.Professor.Siape,
                Posicao = source.Posicao,
                CodigoDisc = source.Disciplina.Codigo,
                QtdaMaxima = source.QtdaMaxima,
                QtdaMinistrada = source.QtdaMinistrada,
                QtdaMaximaJaMinistrada = source.QtdaMaximaJaMinistrada
            };
        }

        public Fila Map(FilaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}