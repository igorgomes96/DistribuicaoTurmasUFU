using DistribuicaoDisciplinas.Dto;
using DistribuicaoDisciplinas.Entities;
using DistribuicaoDisciplinas.Map;
using DistribuicaoDisciplinas.Models;
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

            //Mapper
            container.RegisterSingleton<ISingleMapper<Curso, CursoEntity>, CursoMapper>();
            container.RegisterSingleton<ISingleMapper<Disciplina, DisciplinaEntity>, DisciplinaMapper>();
            container.RegisterSingleton<ISingleMapper<Fila, FilaEntity>, FilaMapper>();
            container.RegisterSingleton<ISingleMapper<FilaTurma, FilaTurmaEntity>, FilaTurmaMapper>();
            container.RegisterSingleton<ISingleMapper<Oferta, OfertaEntity>, OfertaMapper>();
            container.RegisterSingleton<ISingleMapper<Professor, ProfessorEntity>, ProfessorMapper>();
            container.RegisterSingleton<ISingleMapper<Restricao, RestricaoEntity>, RestricaoMapper>();
            container.RegisterSingleton<ISingleMapper<Turma, TurmaEntity>, TurmaMapper>();
            container.RegisterSingleton<ISingleMapper<Cenario, CenarioEntity>, CenarioMapper>();
            container.RegisterSingleton<ISingleMapper<Turma, TurmaRespostaDto>, TurmaRespostaMapper>();
            container.RegisterSingleton<ISingleMapper<Ministra, MinistraEntity>, MinistraMapper>();
            container.RegisterSingleton<ISingleMapper<Professor, ProfessorRespostaDto>, ProfessorRespostaMapper>();
            container.RegisterSingleton(typeof(IMapper<,>), typeof(Mapper<,>));

            //Services
            container.RegisterSingleton<ICenariosService, CenariosService>();
            container.RegisterSingleton<IFilasTurmasService, FilasTurmasService>();
            container.RegisterSingleton<IProfessoresService, ProfessoresService>();
            container.RegisterSingleton<IRestricoesService, RestricoesService>();
            container.RegisterSingleton<ITurmasService, TurmasService>();
            container.RegisterSingleton<IDistribuicaoService, DistribuicaoService>();

            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
            
        }
    }
}