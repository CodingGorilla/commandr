using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Commandr
{
    public interface ICommandDispatcher
    {
        Task Dispatch(HttpContext context);
    }
}