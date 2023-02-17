using System;

namespace Commandr
{
    public interface IResultMapper
    {
        public TDest MapResult<TDest>(object source);
        public object MapResult(object source, Type destType);
    }
}