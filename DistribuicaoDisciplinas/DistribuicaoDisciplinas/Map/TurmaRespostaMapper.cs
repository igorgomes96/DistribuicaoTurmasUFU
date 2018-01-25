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
        private readonly ISingleMapper<Turma, TurmaDto> _turmaMapper;
        
        public TurmaRespostaMapper(ISingleMapper<Turma, TurmaDto> turmaMapper)
        {
            _turmaMapper = turmaMapper;
        }

        public TurmaRespostaDto Map(FilaTurma source)
        {
            return new TurmaRespostaDto {
                Turma = _turmaMapper.Map(source.Turma),
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