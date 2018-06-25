using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class FilaTurmaDtoMapper : ISingleMapper<FilaTurma, FilaTurmaDto>
    {
        private readonly ISingleMapper<Fila, FilaDto> _filaMap;

        public FilaTurmaDtoMapper(ISingleMapper<Fila, FilaDto> filaMap)
        {
            _filaMap = filaMap;
        }

        public FilaTurmaDto Map(FilaTurma source)
        {
            return new FilaTurmaDto
            {
                Fila = _filaMap.Map(source.Fila),
                IdTurma = source.Turma.Id,
                PrioridadeBanco = source.PrioridadeBanco,
                PrioridadeReal = source.PrioridadeReal,
                AtribuicaoFixa = source.AtribuicaoFixa,
                Status = source.StatusAlgoritmo
            };
        }

        public FilaTurma Map(FilaTurmaDto destination)
        {
            throw new NotImplementedException();
        }
    }
}