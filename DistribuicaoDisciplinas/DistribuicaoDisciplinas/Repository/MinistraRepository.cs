using DistribuicaoDisciplinas.Entities;
using Npgsql;
using Repository.Implementations;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DistribuicaoDisciplinas.Repository
{
    public class MinistraRepository : GenericRepository<MinistraEntity>, IMinistraRepository
    {

        public MinistraRepository(DbContext db) : base(db) { }

        public void DeleteTurmasComFilaBySemestre(int ano, int semestre)
        {

            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            _db.Database.BeginTransaction();

            try
            {
                _db.Database.ExecuteSqlCommand(
                    @"delete from ministra a
                      using turma b, fila_turma_new c
                      where a.id_turma = b.id and a.id_turma = c.id_turma
                      and b.ano = @ano and b.semestre = @semestre",
                    new NpgsqlParameter("@ano", ano),
                    new NpgsqlParameter("@semestre", semestre)
                );
                _db.Database.CurrentTransaction.Commit();
                CleanContext(EntityState.Deleted);
            } catch
            {
                _db.Database.CurrentTransaction.Rollback();
            }
        }

        public void SalvarDistribuicao(ICollection<MinistraEntity> distribuicao)
        {
            CleanContext(EntityState.Added, EntityState.Deleted, EntityState.Modified, EntityState.Unchanged);
            _db.Set<MinistraEntity>().AddRange(distribuicao);
            _db.SaveChanges();
            CleanContext(EntityState.Added);
        }
    }
}