using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.Interfaces
{
    public interface ICollectionMapper<TSource, TDestination>
    {
        ICollection<TDestination> Map(ICollection<TSource> source);
        ICollection<TSource> Map(ICollection<TDestination> destination);
    }
}
