using DistribuicaoDisciplinas.Entities;
using Npgsql;
using Repository.Implementations;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Repository
{
    public class CenariosFilasTurmasRepository : GenericRepository<CenarioFilaTurmaEntity>, ICenariosFilasTurmasRepository
    {
        public CenariosFilasTurmasRepository(DbContext db) : base(db) { }

        public void DeleteByCenario(int numCenario)
        {
            //Modelo dbDisposable = new Modelo();
            //dbDisposable.Database.BeginTransaction();
            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            _db.Database.BeginTransaction();
            try {
                _db.Database.ExecuteSqlCommand(
                    "delete from cenario_fila_turma where num_cenario = @numCenario", 
                    new NpgsqlParameter("@numCenario", numCenario));
                _db.Database.CurrentTransaction.Commit();
                CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            } catch
            {
                _db.Database.CurrentTransaction.Rollback();
            }
            //_db.Dispose();
        }

        public void SaveDistribuicao(ICollection<CenarioFilaTurmaEntity> distribuicao)
        {
            //Modelo dbDisposable = new Modelo();
            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            _db.Set<CenarioFilaTurmaEntity>().AddRange(distribuicao);
            _db.SaveChanges();
            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            //dbDisposable.Dispose();
        }
    }
}