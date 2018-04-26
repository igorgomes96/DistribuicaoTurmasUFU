using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Exceptions;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace DistribuicaoDisciplinas.Controllers
{
    public class DistribuicaoController : ApiController
    {
        private readonly IDistribuicaoService _distService;

        public DistribuicaoController(IDistribuicaoService distService)
        {
            _distService = distService;
        }

        [Route("api/Distribuicao/{id}")]
        [HttpGet]
        public IHttpActionResult GetDistribuir(int id)
        {
            try
            {
                return Ok(_distService.CarregaDistribuicao(id));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/{id}")]
        public IHttpActionResult PostDistribuir(int id, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                return Ok(_distService.Distribuir(id, filasTurmas));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/AtribuicaoManual/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostAtribuirTurmaManualmente(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                return Ok(_distService.Atribuir(cenario, siape, idTurma, filasTurmas));
            }
            catch (FilaTurmaNaoEncontradaException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (ChoqueHorarioException e)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (ChoquePeriodoException e)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (JaAtribuidaException e)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (RestricaoHorarioException e)
            {
                return Content(System.Net.HttpStatusCode.BadRequest, e.Message);
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/RemocaoManual/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostRemoverTurmaManualmente(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                return Ok(_distService.Remover(cenario, siape, idTurma, filasTurmas));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (FilaTurmaNaoEncontradaException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/UltimaPrioridade/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostUltimaPrioridade(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                return Ok(_distService.UltimaPrioridade(cenario, siape, idTurma, filasTurmas));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (FilaTurmaNaoEncontradaException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/FinalFila/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostFinalFila(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                return Ok(_distService.FinalFila(cenario, siape, idTurma, filasTurmas));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (FilaTurmaNaoEncontradaException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/Salvar/{cenario}")]
        public IHttpActionResult PostSalvarDistribuicao(int cenario, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                _distService.SalvarDistribuicao(cenario, filasTurmas);
                return Ok();
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [Route("api/Distribuicao/Duplicar/{cenario}")]
        public IHttpActionResult PostDuplicarDistribuicao(int cenario, Cenario novoCenario)
        {
            try
            {
                return Ok(_distService.DuplicarDistribuicao(cenario, novoCenario));
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

        [Route("api/Distribuicao/Oficializar/{cenario}")]
        public IHttpActionResult PostOficializarDistribuicao(int cenario)
        {
            try
            {
                _distService.OficializarDistribuicao(cenario);
                return Ok();
            }
            catch (CenarioNaoEncontradoException e)
            {
                return Content(System.Net.HttpStatusCode.NotFound, e.Message);
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }
    }
}