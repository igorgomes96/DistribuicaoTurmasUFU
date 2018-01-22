using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapping.Interfaces
{
    public interface ISingleMapper<TSource, TDestination>
    {
        TDestination Map(TSource source);
        TSource Map(TDestination destination);
    }
}
