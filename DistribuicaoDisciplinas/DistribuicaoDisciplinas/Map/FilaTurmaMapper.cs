using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Map
{
    public class FilaTurmaMapper : ISingleMapper<FilaTurma, FilaTurmaEntity>
    {
        private readonly IMapper<Fila, FilaEntity> _filaMapper;
        private readonly IMapper<Turma, TurmaEntity> _turmaMapper;

        public FilaTurmaMapper(IMapper<Fila, FilaEntity> filaMapper,
            IMapper<Turma, TurmaEntity> turmaMapper)
        {
            _filaMapper = filaMapper;
            _turmaMapper = turmaMapper;
        }

        public FilaTurmaEntity Map(FilaTurma source)
        {
            throw new NotImplementedException();
        }

        public FilaTurma Map(FilaTurmaEntity destination)
        {
            return destination == null ? null : new FilaTurma
            {
                Fila = _filaMapper.Map(destination.Fila),
                Turma = _turmaMapper.Map(destination.Turma),
                PrioridadeBanco = destination.prioridade.Value,
                PrioridadeReal = destination.prioridade.Value,
                StatusAlgoritmo = Util.Enumerators.StatusFila.NaoAnalisadaAinda
            };

        }
    }
}