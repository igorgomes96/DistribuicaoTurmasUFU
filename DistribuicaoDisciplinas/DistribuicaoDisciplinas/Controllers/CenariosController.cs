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
    }
}
