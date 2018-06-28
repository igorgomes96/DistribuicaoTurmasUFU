using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Interfaces
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {

        ICollection<TEntity> List();
        ICollection<TEntity> Query(Func<TEntity, bool> predicate);
        TEntity Find(params object[] chave);
        TEntity Add(TEntity entity);
        TEntity AddOrUpdate(TEntity entity);
        void AddOrUpdate(ICollection<TEntity> entities);
        void SaveAll(ICollection<TEntity> entities);
        void Update(TEntity entity);
        TEntity Delete(params object[] chave);
        void Delete(Func<TEntity, bool> predicate);
        bool Existe(params object[] key);
        int Count(Func<TEntity, bool> predicate);
        void ExecuteSQLCommand(string sql);
        void LoadReference(TEntity entity, string propertyName);

    }
}
