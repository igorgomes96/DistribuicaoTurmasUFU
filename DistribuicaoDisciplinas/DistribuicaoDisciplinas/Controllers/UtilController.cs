using DistribuicaoDisciplinas.Entities;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DistribuicaoDisciplinas.Controllers
{
    public class UtilController : ApiController
    {
        private readonly IGenericRepository<CenarioEntity> _rep;

        public UtilController(IGenericRepository<CenarioEntity> rep)
        {
            _rep = rep;
        }

        [Route("api/Util/LimparCache")]
        public IHttpActionResult PostLimparCache()
        {
            try
            {
                _rep.CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
                return Ok();
            } catch (Exception e)
            {
                return Content(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
