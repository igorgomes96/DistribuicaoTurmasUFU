using DistribuicaoDisciplinas.Exceptions;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Services;
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

        public CenariosController(ICenariosService cenariosService)
        {
            _cenariosService = cenariosService;
        }

        public IHttpActionResult Get()
        {
            try
            {
                return Ok(_cenariosService.List());
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

        [Route("api/Cenarios/{numCenario}/Duplica")]
        public IHttpActionResult PostDuplica(int numCenario, Cenario cenario)
        {
            try
            {
                return Ok(_cenariosService.DuplicarCenario(numCenario, cenario));
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}
