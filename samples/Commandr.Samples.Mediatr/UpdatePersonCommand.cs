using System;
using System.Threading;
using System.Threading.Tasks;
using Commandr.Binding;
using Commandr.Results;
using MediatR;

namespace Commandr.Samples.Mediatr
{
    [CommandRoute("/people/{id}", "PUT")]
    public class UpdatePersonCommand : IRoutableCommand, IRequest<ICommandResult>
    {
        [FromUrlTemplate("id")] 
        public int Id { get; set; }

        [FromJsonToken] 
        public string FirstName { get; set; }

        [FromJsonToken] 
        public string LastName { get; set; }
    }

    public class UpdatePersonCommandHandler : IRequestHandler<UpdatePersonCommand, ICommandResult>
    {
        private readonly ICommandResultFactory _resultFactory;

        public UpdatePersonCommandHandler(ICommandResultFactory resultFactory)
        {
            _resultFactory = resultFactory;
        }

        public Task<ICommandResult> Handle(UpdatePersonCommand command, CancellationToken token)
        {
            // Pretend we have a database
            var person = GetPersonFromDb(command.Id);
            if(person == null)
                return Task.FromResult(_resultFactory.NotFound());

            person.FirstName = command.FirstName;
            person.LastName = command.LastName;

            // We should save this back...

            return Task.FromResult(_resultFactory.Ok(person));
        }

        private Person GetPersonFromDb(in int personId)
        {
            if(personId != 12)
                return null;

            return new Person
                   {
                       Id = 12,
                       FirstName = "George",
                       LastName = "Castanza"
                   };
        }
    }

    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}