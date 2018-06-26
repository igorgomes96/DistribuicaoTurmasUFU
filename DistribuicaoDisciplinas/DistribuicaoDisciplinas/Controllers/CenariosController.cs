using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Exceptions;
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
    public class CenariosController : ApiController
    {
        private readonly ICenariosService _cenariosService;
        private readonly IMapper<Cenario, CenarioDto> _cenarioMapper;

        public CenariosController(ICenariosService cenariosService, IMapper<Cenario, CenarioDto> cenarioMapper)
        {
            _cenariosService = cenariosService;
            _cenarioMapper = cenarioMapper;
        }

        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_cenarioMapper.Map(_cenariosService.List()));
            } catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult DeleteCenario(int id)
        {
            try
            {
                _cenariosService.DeleteCenario(id);
                return Ok();
            } catch (CenarioNaoEncontradoException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [HttpDelete]
        [Route("api/Cenarios/Limpar/{id}")]
        public IHttpActionResult LimparCenario(int id)
        {
            try
            {
                _cenariosService.LimparCenario(id);
                return Ok();
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        public IHttpActionResult Post(Cenario cenario)
        {
            try
            {
                return Ok(_cenarioMapper.Map(_cenariosService.NovoCenario(cenario)));
            } catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}
