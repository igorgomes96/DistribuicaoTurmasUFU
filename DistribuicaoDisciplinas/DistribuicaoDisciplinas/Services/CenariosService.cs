using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Exceptions;
using DistribuicaoDisciplinas.Models;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class CenariosService : ICenariosService
    {
        private readonly IGenericRepository<CenarioEntity> _rep;
        private readonly IGenericRepository<DistribuicaoCargaEntity> _chsRep;
        private readonly IGenericRepository<CenarioFilaTurmaEntity> _cenarioFilaTurmaRep;
        private readonly IGenericRepository<AtribuicaoManualEntity> _atribuicaoManualRep;
        private readonly IMapper<Cenario, CenarioEntity> _map;
        private readonly IProfessoresService _professoresService;

        public CenariosService(IGenericRepository<CenarioEntity> rep,
            IGenericRepository<DistribuicaoCargaEntity> chsRep,
            IGenericRepository<CenarioFilaTurmaEntity> cenarioFilaTurmaRep,
            IGenericRepository<AtribuicaoManualEntity> atribuicaoManualRep,
            IMapper<Cenario, CenarioEntity> map,
            IProfessoresService professoresService)
        {
            _rep = rep;
            _chsRep = chsRep;
            _cenarioFilaTurmaRep = cenarioFilaTurmaRep;
            _atribuicaoManualRep = atribuicaoManualRep;
            _map = map;
            _professoresService = professoresService;
        }

        public void DeleteCenario(int idCenario)
        {
            CenarioEntity cenarioEntity = _rep.Find(idCenario);
            if (cenarioEntity == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            if (_chsRep.Count(x => x.IdCenario == idCenario) > 0)
                _chsRep.Delete(x => x.IdCenario == idCenario);
            _atribuicaoManualRep.Delete(x => x.num_cenario == idCenario);
            _cenarioFilaTurmaRep.Delete(x => x.num_cenario == idCenario);
            _rep.Delete(idCenario);

        }

        public void LimparCenario(int idCenario)
        {
            CenarioEntity cenarioEntity = _rep.Find(idCenario);
            if (cenarioEntity == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            _cenarioFilaTurmaRep.Delete(x => x.num_cenario == idCenario);
            _atribuicaoManualRep.Delete(x => x.num_cenario == idCenario);
        }

        public Cenario DuplicarCenario(int cenarioBase, Cenario novoCenario)
        {
            Cenario novo = NovoCenario(novoCenario);
            CenarioEntity old = _rep.Find(cenarioBase);

            if (old == null)
                throw new CenarioNaoEncontradoException("Cenário não encontrado!");

            ICollection<DistribuicaoCargaEntity> chs = _chsRep
                .Query(x => x.IdCenario == cenarioBase)
                .Select(x => new DistribuicaoCargaEntity
                {
                    IdCenario = novo.NumCenario,
                    Regra = x.Regra,
                    Siape = x.Siape,
                    CH = x.CH
                }).ToList();

            _chsRep.SaveAll(chs);

            return novo;

        }

        public Cenario Find(int num)
        {
            return _map.Map(_rep.Find(num));
        }

        public ICollection<Cenario> List()
        {
            return _map.Map(_rep.List());
        }

        public Cenario NovoCenario(Cenario cenario)
        {
            Cenario c = _map.Map(_rep.Add(_map.Map(cenario)));
            return c;
        }

    }
}