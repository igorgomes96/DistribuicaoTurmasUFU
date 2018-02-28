namespace DistribuicaoDisciplinas.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Linq;
    using System.Reflection;

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
            modelBuilder.Conventions.Add(new NonPublicColumnAttributeConvention());

            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            //modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Curso> Cursos { get; set; }
        public virtual DbSet<Disciplina> Disciplinas { get; set; }
        public virtual DbSet<Fila> Filas { get; set; }
        public virtual DbSet<FilaTurma> FilasTurmas { get; set; }
        public virtual DbSet<Oferta> Ofertas { get; set; }
        public virtual DbSet<Professor> Professores { get; set; }
        public virtual DbSet<Restricao> Restricoes { get; set; }
        public virtual DbSet<Turma> Turmas { get; set; }
        public virtual DbSet<Cenario> Cenarios { get; set; }
        public virtual DbSet<Ministra> Ministra { get; set; }
    }
    /// <summary>
    /// Convention to support binding private or protected properties to EF columns.
    /// </summary>
    public sealed class NonPublicColumnAttributeConvention : Convention
    {

        public NonPublicColumnAttributeConvention()
        {
            Types().Having(NonPublicProperties)
                   .Configure((config, properties) =>
                   {
                       foreach (PropertyInfo prop in properties)
                       {
                           config.Property(prop);
                       }
                   });
        }

        private IEnumerable<PropertyInfo> NonPublicProperties(Type type)
        {
            var matchingProperties = type.GetProperties(BindingFlags.SetProperty | BindingFlags.GetProperty | BindingFlags.NonPublic | BindingFlags.Instance)
                                         .Where(propInfo => propInfo.GetCustomAttributes(typeof(ColumnAttribute), true).Length > 0)
                                         .ToArray();
            return matchingProperties.Length == 0 ? null : matchingProperties;
        }
    }

}