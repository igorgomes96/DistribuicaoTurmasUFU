using DistribuicaoDisciplinas.Entities;
using Npgsql;
using Repository.Implementations;
using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Repository
{
    public class DistribuicaoCargaCenarioRepository : GenericRepository<DistribuicaoCargaEntity>, IDistribuicaoCargaCenarioRepository
    {
        public DistribuicaoCargaCenarioRepository(DbContext db) : base(db) { }

        public void DeleteByCenario(int numCenario)
        {
            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            _db.Database.BeginTransaction();
            try
            {
                _db.Database.ExecuteSqlCommand(
                    "delete from distribuicao_carga where cenario = @numCenario",
                    new NpgsqlParameter("@numCenario", numCenario));
                _db.Database.CurrentTransaction.Commit();
                CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            }
            catch
            {
                _db.Database.CurrentTransaction.Rollback();
            }
        }

        public void CleanContext()
        {
            base.CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
        }

    }
}