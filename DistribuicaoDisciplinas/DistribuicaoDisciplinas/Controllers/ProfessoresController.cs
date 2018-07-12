using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Services;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DistribuicaoDisciplinas.Controllers
{
    public class ProfessoresController : ApiController
    {
        private readonly IProfessoresService _professoresService;
        private readonly IMapper<Professor, ProfessorDto> _profMapper;
        public ProfessoresController(IProfessoresService professoresService,
            IMapper<Professor, ProfessorDto> profMapper)
        {
            _professoresService = professoresService;
            _profMapper = profMapper;
        }

        [Route("api/Professores/Ativos")]
        public IHttpActionResult GetAtivos()
        {
            try
            {
                return Ok(_profMapper.Map(_professoresService.ListAtivos()));
            } catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
