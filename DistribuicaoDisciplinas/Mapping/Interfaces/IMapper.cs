using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.Interfaces
{
    public interface IMapper<TSource, TDestination> : ISingleMapper<TSource, TDestination>, ICollectionMapper<TSource, TDestination>
    {

    }
}
