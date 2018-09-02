using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Exceptions;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Repository;
using DistribuicaoDisciplinas.Services;
using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        private readonly IDistribuicaoCargaCenarioService _distribuicaoCargaService;
        private readonly DbContext _db;

        public CenariosController(ICenariosService cenariosService, IMapper<Cenario, CenarioDto> cenarioMapper,
            IDistribuicaoCargaCenarioService distribuicaoCargaService,
            DbContext db)
        {
            _cenariosService = cenariosService;
            _cenarioMapper = cenarioMapper;
            _distribuicaoCargaService = distribuicaoCargaService;
            _db = db;
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
            {;
                CenarioDto retorno = _cenarioMapper.Map(_cenariosService.NovoCenario(cenario));
                _distribuicaoCargaService.CHPadraoPorCenario(retorno.NumCenario);
                return Ok(retorno);
            } catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Cenarios/{numCenario}/CHs")]
        public IHttpActionResult PostCargaHoraria(int numCenario, ICollection<DistribuicaoCargaEntity> carga)
        {
            try
            {
                Cenario cenario = _cenariosService.Find(numCenario);
                if (cenario == null) throw new CenarioNaoEncontradoException("Cenário não encontrado!");

                foreach (DistribuicaoCargaEntity d in carga)
                {
                    d.IdCenario = numCenario;
                }

                _distribuicaoCargaService.AtualizaCH(numCenario, carga);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

    }
}
