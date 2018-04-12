using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class CenariosFilasTurmasService : ICenariosFilasTurmasService
    {
        private readonly IGenericRepository<CenarioFilaTurmaEntity> _repository;
        private readonly IMapper<CenarioFilaTurma, CenarioFilaTurmaEntity> _map;

        public CenariosFilasTurmasService(
            IGenericRepository<CenarioFilaTurmaEntity> repository,
            IMapper<CenarioFilaTurma, CenarioFilaTurmaEntity> map)
        {
            _repository = repository;
            _map = map;
        }

        public void Delete(CenarioFilaTurma cenarioFilaTurma)
        {
            _repository.Delete(new[] { cenarioFilaTurma.Cenario.NumCenario, cenarioFilaTurma.FilaTurma.Turma.Id, cenarioFilaTurma.FilaTurma.Fila.Id });
        }


        public int Count(int numCenario)
        {
            return _repository.Count(x => x.num_cenario == numCenario);
        }

        public ICollection<CenarioFilaTurma> List(int numCenario)
        {
            return _map.Map(_repository.Query(x => x.num_cenario == numCenario));

        }
    }
}