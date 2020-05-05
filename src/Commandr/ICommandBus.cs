using System.Threading.Tasks;

namespace Commandr
{
    /// <summary>
    /// An abstract interface for the command bus which is
    /// used to execute <seealso cref="IRoutableCommand"/>
    /// </summary>
    /// <remarks>
    /// This should be implemented on top of another messaging
    /// or mediator style framework
    /// </remarks>
    public interface ICommandBus
    {
        Task<object> InvokeCommandAsync(IRoutableCommand command);
    }
}