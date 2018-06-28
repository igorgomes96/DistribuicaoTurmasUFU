using DistribuicaoDisciplinas.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DistribuicaoDisciplinas.Controllers
{
    public class TesteController : ApiController
    {
        private readonly IGenericRepository<AtribuicaoManualEntity> _rep;
        private readonly IGenericRepository<TurmaEntity> _rep2;
        private readonly IGenericRepository<ProfessorEntity> _rep3;
        private readonly IGenericRepository<CenarioEntity> _rep4;

        public TesteController(IGenericRepository<AtribuicaoManualEntity> rep, IGenericRepository<TurmaEntity> rep2,
            IGenericRepository<ProfessorEntity> rep3, IGenericRepository<CenarioEntity> rep4)
        {
            _rep = rep;
            _rep2 = rep2;
            _rep3 = rep3;
            _rep4 = rep4;
        }

    }
}
