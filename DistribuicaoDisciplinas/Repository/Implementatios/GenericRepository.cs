using Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace Repository.Implementations
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {

        private readonly DbContext _db;

        public GenericRepository(DbContext db)
        {
            _db = db ?? throw new ArgumentNullException(nameof(DbContext));
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
            _db.Set<TEntity>().Where(predicate).ToList().ForEach(entidade =>
            {
                _db.Set<TEntity>().Remove(entidade);
            });
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
            foreach (TEntity entity in entities)
            {
                _db.Set<TEntity>().Add(entity);
            }
            _db.SaveChangesAsync();
        }

        public void Update(TEntity entidade)
        {
            _db.Entry(entidade).State = EntityState.Modified;
            _db.SaveChanges();
        }
    }
}