using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Repository;
using Mapping.Interfaces;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class DistribuicaoCargaCenarioService : IDistribuicaoCargaCenarioService
    {
        private readonly IProfessoresService _professoresService;
        private readonly IDistribuicaoCargaCenarioRepository _chsRep;
        private readonly IMapper<DistribuicaoCarga, DistribuicaoCargaEntity> _mapper;

        public DistribuicaoCargaCenarioService(IProfessoresService professoresService,
            IDistribuicaoCargaCenarioRepository chsRep,
            IMapper<DistribuicaoCarga, DistribuicaoCargaEntity> mapper)
        {
            _professoresService = professoresService;
            _chsRep = chsRep;
            _mapper = mapper;
        }


        public int GetCargaProfessor(int codigoCenario, string siape)
        {
            ICollection<DistribuicaoCargaEntity> chs = _chsRep.Query(x => x.IdCenario == codigoCenario && x.Siape.Trim().Equals(siape.Trim()));
            return chs.Sum(x => x.CH);
        }

        public void AtualizaCH(int numCenario, ICollection<DistribuicaoCargaEntity> carga)
        {
            _chsRep.DeleteByCenario(numCenario);
            _chsRep.AddOrUpdate(carga);
        }

        public void CHPadraoPorCenario(int codigoCenario)
        {
            _chsRep.AddOrUpdate(_professoresService.ListAtivos().Select(p => new DistribuicaoCargaEntity
            {
                IdCenario = codigoCenario,
                CH = p.CH,
                Regra = "Padrão",
                Siape = p.Siape
            }).ToList());
            _chsRep.CleanContext();
        }
    }
}