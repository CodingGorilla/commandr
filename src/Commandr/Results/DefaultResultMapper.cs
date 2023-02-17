using System;
using System.Collections.Immutable;
using System.Reflection;

namespace Commandr.Results
{
    public class DefaultResultMapper : IResultMapper
    {
        public TDest MapResult<TDest>(object source)
            => (TDest)MapResult(source, typeof(TDest));

        public object MapResult(object source, Type destType)
        {
            var dest = Activator.CreateInstance(destType);

            var sourceType = source.GetType();
            var sourceProps = sourceType.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToImmutableDictionary(x => x.Name, y => y);

            var destProps = destType.GetProperties(BindingFlags.Instance | BindingFlags.Public);

            foreach(var destProp in destProps)
            {
                if(sourceProps.TryGetValue(destProp.Name, out var sourceProp))
                    destProp.SetValue(dest, sourceProp.GetValue(source));
            }

            return dest;
        }
    }
}