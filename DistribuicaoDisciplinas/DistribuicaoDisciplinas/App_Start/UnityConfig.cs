using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Map;
using DistribuicaoDisciplinas.Models;
using DistribuicaoDisciplinas.Repository;
using DistribuicaoDisciplinas.Services;
using Mapping.Implementations;
using Mapping.Interfaces;
using Repository.Implementations;
using Repository.Interfaces;
using System.Data.Entity;
using System.Web.Http;
using Unity;
using Unity.WebApi;

namespace DistribuicaoDisciplinas
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            //DbContext
            container.RegisterSingleton<DbContext, Modelo>();

            //Repository
            container.RegisterSingleton(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            container.RegisterSingleton<IMinistraRepository, MinistraRepository>();
            container.RegisterSingleton<ICenariosFilasTurmasRepository, CenariosFilasTurmasRepository>();
            container.RegisterSingleton<IDistribuicaoCargaCenarioRepository, DistribuicaoCargaCenarioRepository>();

            //Mapper
            //Entity
            container.RegisterSingleton<ISingleMapper<Curso, CursoEntity>, CursoMapper>();
            container.RegisterSingleton<ISingleMapper<Disciplina, DisciplinaEntity>, DisciplinaMapper>();
            container.RegisterSingleton<ISingleMapper<Fila, FilaEntity>, FilaMapper>();
            container.RegisterSingleton<ISingleMapper<FilaTurma, FilaTurmaEntity>, FilaTurmaMapper>();
            container.RegisterSingleton<ISingleMapper<Oferta, OfertaEntity>, OfertaMapper>();
            container.RegisterSingleton<ISingleMapper<Professor, ProfessorEntity>, ProfessorMapper>();
            container.RegisterSingleton<ISingleMapper<Restricao, RestricaoEntity>, RestricaoMapper>();
            container.RegisterSingleton<ISingleMapper<Turma, TurmaEntity>, TurmaMapper>();
            container.RegisterSingleton<ISingleMapper<CenarioFilaTurma, CenarioFilaTurmaEntity>, CenarioFilaTurmaMapper>();
            container.RegisterSingleton<ISingleMapper<Cenario, CenarioEntity>, CenarioMapper>();
            container.RegisterSingleton<ISingleMapper<Ministra, MinistraEntity>, MinistraMapper>();
            container.RegisterSingleton<ISingleMapper<DistribuicaoCarga, DistribuicaoCargaEntity>, DistribuicaoCargaMapper>();
            container.RegisterSingleton<ISingleMapper<AtribuicaoManual, AtribuicaoManualEntity>, AtribuicaoManualMapper>();

            //Dto
            //container.RegisterSingleton<ISingleMapper<FilaTurma, TurmaRespostaDto>, TurmaRespostaMapper>();
            //container.RegisterSingleton<ISingleMapper<FilaTurma, ProfessorRespostaDto>, ProfessorRespostaMapper>();
            container.RegisterSingleton<ISingleMapper<Cenario, CenarioDto>, CenarioDtoMapper>();
            container.RegisterSingleton<ISingleMapper<Turma, TurmaDto>, TurmaDtoMapper>();
            container.RegisterSingleton<ISingleMapper<Professor, ProfessorDto>, ProfessorDtoMapper>();
            container.RegisterSingleton<ISingleMapper<Bloqueio, BloqueioDto>, BloqueioMapper>();
            container.RegisterSingleton<ISingleMapper<Fila, FilaDto>, FilaDtoMapper>();
            container.RegisterSingleton<ISingleMapper<FilaTurma, FilaTurmaDto>, FilaTurmaDtoMapper>();
            //container.RegisterSingleton<ISingleMapper<Professor, ProfessorPrioridadesDto>, ProfessorPrioridadesMapper>();
            container.RegisterSingleton<ISingleMapper<Ministra, FilaTurma>, MinistraFilaTurmaMapper>();
            container.RegisterSingleton(typeof(IMapper<,>), typeof(Mapper<,>));

            //Services
            container.RegisterSingleton<ICenariosService, CenariosService>();
            container.RegisterSingleton<IFilasTurmasService, FilasTurmasService>();
            container.RegisterSingleton<IProfessoresService, ProfessoresService>();
            container.RegisterSingleton<IRestricoesService, RestricoesService>();
            container.RegisterSingleton<ITurmasService, TurmasService>();
            container.RegisterType<IDistribuicaoService, DistribuicaoService>();
            container.RegisterSingleton<IMinistraService, MinistraService>();
            container.RegisterSingleton<IDistribuicaoCargaCenarioService, DistribuicaoCargaCenarioService>();
            container.RegisterSingleton<ICenariosFilasTurmasService, CenariosFilasTurmasService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            
        }
    }
}