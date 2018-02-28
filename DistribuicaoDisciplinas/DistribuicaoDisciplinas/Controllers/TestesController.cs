using DistribuicaoDisciplinas.Dto;
//using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Services;
using Mapping.Interfaces;
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
    public class TestesController : ApiController
    {
        private readonly DbContext _db;

        //Services
        private readonly IDistribuicaoService _distService;

        //Repositories
        private readonly IGenericRepository<Curso> _cursoRep;
        private readonly IGenericRepository<Cenario> _cenarioRep;
        private readonly IGenericRepository<Ministra> _ministraRep;

        public TestesController(DbContext db,
            IGenericRepository<Curso> cursoRep,
            IGenericRepository<Cenario> cenarioRep,
            IGenericRepository<Ministra> ministraRep,
            IDistribuicaoService distService
        )
        {
            _db = db;
            _cursoRep = cursoRep;
            _cenarioRep = cenarioRep;
            _ministraRep = ministraRep;

            _distService = distService;

        }

        [Route("api/Testes/Distribuir/{id}")]
        public IHttpActionResult GetDistribuir(int id)
        {
            return Ok(_distService.Distribuir(id, null));
        }

        [Route("api/Testes/Distribuir/{id}")]
        public IHttpActionResult PostDistribuir(int id, ICollection<FilaTurmaDto> filasTurmas)
        {
            return Ok(_distService.Distribuir(id, filasTurmas));
        }

        [Route("api/Testes/Distribuir/AtribuicaoManual/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostAtribuirTurmaManualmente(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            return Ok(_distService.Atribuir(cenario, siape, idTurma, filasTurmas));
        }

        [Route("api/Testes/Distribuir/RemocaoManual/{cenario}/{siape}/{idTurma}")]
        public IHttpActionResult PostRemoverTurmaManualmente(int cenario, string siape, int idTurma, ICollection<FilaTurmaDto> filasTurmas)
        {
            return Ok(_distService.Remover(cenario, siape, idTurma, filasTurmas));
        }

        [Route("api/Testes/Cursos")]
        public IHttpActionResult GetCursos()
        {
            return Ok(_cursoRep.List());
        }

        [Route("api/Testes/Ministra")]
        public IHttpActionResult GetMinistra()
        {
            return Ok(_ministraRep.List());
        }

        [Route("api/Testes/Cenarios")]
        public IHttpActionResult GetCenarios()
        {
            return Ok(_cenarioRep.List());
        }

        [Route("api/Testes/Disciplinas")]
        public IHttpActionResult GetDisciplinas()
        {
            return Ok(_db.Set<Disciplina>().ToList());
        }

        [Route("api/Testes/Filas")]
        public IHttpActionResult GetFilas()
        {
            return Ok(_db.Set<Fila>().ToList());
        }

        [Route("api/Testes/FilasTurmas")]
        public IHttpActionResult GetFilasTurmas()
        {
            return Ok(_db.Set<FilaTurma>().Where(x => x.IdTurma == 878).ToList());
        }

        [Route("api/Testes/Ofertas")]
        public IHttpActionResult GetOfertas()
        {
            return Ok(_db.Set<Oferta>().ToList());
        }

        [Route("api/Testes/Professores")]
        public IHttpActionResult GetProfessores()
        {
            return Ok(_db.Set<Professor>().ToList());
        }

        [Route("api/Testes/Restricoes")]
        public IHttpActionResult GetRestricoes()
        {
            return Ok(_db.Set<Restricao>().ToList());
        }

        [Route("api/Testes/Turmas")]
        public IHttpActionResult GetTurmas()
        {
            return Ok(_db.Set<Turma>().ToList());
        }
    }
}
