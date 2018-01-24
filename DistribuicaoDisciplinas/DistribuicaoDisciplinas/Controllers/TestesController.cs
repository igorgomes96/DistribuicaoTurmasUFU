using DistribuicaoDisciplinas.Entities;
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
        private readonly IGenericRepository<CursoEntity> _cursoRep;
        private readonly IGenericRepository<CenarioEntity> _cenarioRep;
        private readonly IGenericRepository<MinistraEntity> _ministraRep;

        //Mapper
        private readonly IMapper<Curso, CursoEntity> _cursoMap;
        private readonly IMapper<Disciplina, DisciplinaEntity> _discMap;
        private readonly IMapper<Fila, FilaEntity> _filaMap;
        private readonly IMapper<FilaTurma, FilaTurmaEntity> _filaTurmaMap;
        private readonly IMapper<Oferta, OfertaEntity> _ofertaMap;
        private readonly IMapper<Professor, ProfessorEntity> _profMap;
        private readonly IMapper<Restricao, RestricaoEntity> _restricaoMap;
        private readonly IMapper<Turma, TurmaEntity> _turmaMap;
        private readonly IMapper<Cenario, CenarioEntity> _cenariosMap;
        private readonly IMapper<Ministra, MinistraEntity> _ministraMap;

        public TestesController(DbContext db,
            IGenericRepository<CursoEntity> cursoRep,
            IGenericRepository<CenarioEntity> cenarioRep,
            IGenericRepository<MinistraEntity> ministraRep,
            IMapper<Curso, CursoEntity> cursoMap,
            IMapper<Disciplina, DisciplinaEntity> discMap,
            IMapper<Fila, FilaEntity> filaMap,
            IMapper<FilaTurma, FilaTurmaEntity> filaTurmaMap,
            IMapper<Oferta, OfertaEntity> ofertaMap,
            IMapper<Professor, ProfessorEntity> profMap,
            IMapper<Restricao, RestricaoEntity> restricaoMap,
            IMapper<Turma, TurmaEntity> turmaMap,
            IMapper<Cenario, CenarioEntity> cenariosMap,
            IDistribuicaoService distService,
            IMapper<Ministra, MinistraEntity> ministraMap
        )
        {
            _db = db;
            _cursoRep = cursoRep;
            _cenarioRep = cenarioRep;
            _ministraRep = ministraRep;

            _cursoMap = cursoMap;
            _discMap = discMap;
            _filaMap = filaMap;
            _filaTurmaMap = filaTurmaMap;
            _ofertaMap = ofertaMap;
            _profMap = profMap;
            _restricaoMap = restricaoMap;
            _turmaMap = turmaMap;
            _cenariosMap = cenariosMap;
            _ministraMap = ministraMap;

            _distService = distService;

        }

        [Route("api/Testes/Distribuir/{id}")]
        public IHttpActionResult GetDistribuir(int id)
        {
            return Ok(_distService.Distribuir(id));
        }

        [Route("api/Testes/Cursos")]
        public IHttpActionResult GetCursos()
        {
            return Ok(_cursoMap.Map(_cursoRep.List()));
        }

        [Route("api/Testes/Ministra")]
        public IHttpActionResult GetMinistra()
        {
            return Ok(_ministraMap.Map(_ministraRep.List()));
        }

        [Route("api/Testes/Cenarios")]
        public IHttpActionResult GetCenarios()
        {
            return Ok(_cenariosMap.Map(_cenarioRep.List()));
        }

        [Route("api/Testes/Disciplinas")]
        public IHttpActionResult GetDisciplinas()
        {
            return Ok(_discMap.Map(_db.Set<DisciplinaEntity>().ToList()));
        }

        [Route("api/Testes/Filas")]
        public IHttpActionResult GetFilas()
        {
            return Ok(_filaMap.Map(_db.Set<FilaEntity>().ToList()));
        }

        [Route("api/Testes/FilasTurmas")]
        public IHttpActionResult GetFilasTurmas()
        {
            return Ok(_filaTurmaMap.Map(_db.Set<FilaTurmaEntity>().Where(x => x.id_turma == 878).ToList()));
        }

        [Route("api/Testes/Ofertas")]
        public IHttpActionResult GetOfertas()
        {
            return Ok(_ofertaMap.Map(_db.Set<OfertaEntity>().ToList()));
        }

        [Route("api/Testes/Professores")]
        public IHttpActionResult GetProfessores()
        {
            return Ok(_profMap.Map(_db.Set<ProfessorEntity>().ToList()));
        }

        [Route("api/Testes/Restricoes")]
        public IHttpActionResult GetRestricoes()
        {
            return Ok(_restricaoMap.Map(_db.Set<RestricaoEntity>().ToList()));
        }

        [Route("api/Testes/Turmas")]
        public IHttpActionResult GetTurmas()
        {
            return Ok(_turmaMap.Map(_db.Set<TurmaEntity>().ToList()));
        }
    }
}
