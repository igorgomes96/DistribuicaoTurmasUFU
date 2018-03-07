using DistribuicaoDisciplinas.Dto;
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

        public IHttpActionResult GetDistribuir(int id)
        {
            try
            {
                return Ok(_distService.Distribuir(id, null));
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
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }

        [Route("api/Distribuicao/Salvar/{ano}/{semestre}")]
        public IHttpActionResult PostSalvarDistribuicao(int ano, int semestre, ICollection<FilaTurmaDto> filasTurmas)
        {
            try
            {
                _distService.SalvarDistribuicao(filasTurmas);
                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }

        }

    }
}