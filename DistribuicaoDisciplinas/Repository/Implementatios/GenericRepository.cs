using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity.Migrations;
using System.Transactions;
using System.Web;

namespace Repository.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        protected readonly DbContext _db;

        public GenericRepository(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(DbContext));
        }

        public void ExecuteSQLCommand(string sql)
        {
            _db.Database.ExecuteSqlCommand(sql);
            _db.SaveChanges();
        }

        public int Count(Func<TEntity, bool> predicate)
        {
            return _db.Set<TEntity>().Count(predicate);
        }

        public TEntity Add(TEntity entidade)
        {
            entidade = _db.Set<TEntity>().Add(entidade);
            _db.SaveChanges();
            return entidade;
        }

        public TEntity Delete(params object[] chave)
        {
            TEntity entidade = Find(chave);
            _db.Set<TEntity>().Remove(entidade);
            _db.SaveChanges();
            return entidade;
        }

        public void Delete(Func<TEntity, bool> predicate)
        {
            _db.Set<TEntity>().RemoveRange(_db.Set<TEntity>().Where(predicate));
            _db.SaveChanges();
        }

        public bool Existe(params object[] chave)
        {
            return Find(chave) == null ? false : true;
        }

        public TEntity Find(params object[] chave)
        {
            return _db.Set<TEntity>().Find(chave);
        }

        public ICollection<TEntity> List()
        {
            return _db.Set<TEntity>().ToList();
        }

        public ICollection<TEntity> Query(Func<TEntity, bool> predicate)
        {
            return _db.Set<TEntity>().ToList().Where(predicate).ToList();
        }

        public void SaveAll(ICollection<TEntity> entities)
        {
            _db.Set<TEntity>().AddRange(entities);
            _db.SaveChanges();
        }

        public void Update(TEntity entidade)
        {
            _db.Entry(entidade).State = EntityState.Modified;
            _db.SaveChanges();
        }

        public TEntity AddOrUpdate(TEntity entidade)
        {
            _db.Set<TEntity>().AddOrUpdate(entidade);
            _db.SaveChanges();
            return entidade;
        }

        protected void CleanContext(params EntityState[] entityStates)
        {
            foreach (EntityState entityState in entityStates)
            {
                var objectStateEntries = ((IObjectContextAdapter)_db).ObjectContext
                             .ObjectStateManager
                             .GetObjectStateEntries(entityState);

                foreach (var objectStateEntry in objectStateEntries)
                {
                    ((IObjectContextAdapter)_db).ObjectContext.Detach(objectStateEntry.Entity);
                }
            }
            
        }

        public void AddOrUpdate(ICollection<TEntity> entities)
        {
            entities.ToList()
                .ForEach(x => _db.Set<TEntity>().AddOrUpdate(x));
            _db.SaveChanges();
        }

    }
}