namespace DistribuicaoDisciplinas.Entities
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class Modelo : DbContext
    {
        // Your context has been configured to use a 'Modelo' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'DistribuicaoDisciplinas.Entities.Modelo' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'Modelo' 
        // connection string in the application configuration file.
        public Modelo()
            : base("Modelo")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //if (Database.Connection.ToString().Contains("Npgsql"))
            modelBuilder.HasDefaultSchema("public");

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<CursoEntity> Cursos { get; set; }
        public virtual DbSet<DisciplinaEntity> Disciplinas { get; set; }
        public virtual DbSet<FilaEntity> Filas { get; set; }
        public virtual DbSet<FilaTurmaEntity> FilasTurmas { get; set; }
        public virtual DbSet<OfertaEntity> Ofertas { get; set; }
        public virtual DbSet<ProfessorEntity> Professores { get; set; }
        public virtual DbSet<RestricaoEntity> Restricoes { get; set; }
        public virtual DbSet<TurmaEntity> Turmas { get; set; }
        public virtual DbSet<CenarioEntity> Cenarios { get; set; }
        public virtual DbSet<MinistraEntity> Ministra { get; set; }
        public virtual DbSet<CenarioFilaTurmaEntity> CenariosFilasTurmas { get; set; }
        public virtual DbSet<DistribuicaoCargaEntity> CHsPorCenarios { get; set; }
        public virtual DbSet<AtribuicaoManualEntity> AtribuicoesManuais { get; set; }
    }

}