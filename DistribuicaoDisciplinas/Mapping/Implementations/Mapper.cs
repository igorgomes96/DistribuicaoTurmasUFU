using Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Mapping.Implementations
{
    public class Mapper<TSource, TDestination> : IMapper<TSource, TDestination>
    {
        private readonly ISingleMapper<TSource, TDestination> _mapper;

        public Mapper(ISingleMapper<TSource, TDestination> mapper)
        {
            _mapper = mapper;
        }

        public ICollection<TDestination> Map(ICollection<TSource> source)
        {
            return source.Select(x => _mapper.Map(x)).ToList();
        }

        public ICollection<TSource> Map(ICollection<TDestination> destination)
        {
            return destination.Select(x => _mapper.Map(x)).ToList();
        }

        public TDestination Map(TSource source)
        {
            return _mapper.Map(source);
        }

        public TSource Map(TDestination destination)
        {
            return _mapper.Map(destination);
        }
    }
}