using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Repository;
using Repository.Interfaces;

namespace DistribuicaoDisciplinas.Services
{
    public class DistribuicaoCargaCenarioService : IDistribuicaoCargaCenarioService
    {
        private readonly IProfessoresService _professoresService;
        private readonly IDistribuicaoCargaCenarioRepository _chsRep;

        public DistribuicaoCargaCenarioService(IProfessoresService professoresService,
            IDistribuicaoCargaCenarioRepository chsRep)
        {
            _professoresService = professoresService;
            _chsRep = chsRep;
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
        }
    }
}