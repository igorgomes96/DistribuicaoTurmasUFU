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
        TEntity Add(TEntity entidade);
        void Update(TEntity entidade);
        TEntity Delete(params object[] chave);
        void Delete(Func<TEntity, bool> predicate);
        bool Existe(params object[] chave);


    }
}
