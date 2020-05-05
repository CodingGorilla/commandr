using System;
using Commandr.Binding;
using Commandr.Results;

namespace Commandr.Samples.Jasper
{
    [CommandRoute("/people/{id}", "PUT")]
    public class UpdatePersonCommand : IRoutableCommand
    {
        [FromRequestUrl("id")] 
        public int Id { get; set; }

        [FromRequestBody] 
        public string FirstName { get; set; }

        [FromRequestBody] 
        public string LastName { get; set; }
    }

    public class UpdatePersonCommandHandler
    {
        private readonly ICommandResultFactory _resultFactory;

        public UpdatePersonCommandHandler(ICommandResultFactory resultFactory)
        {
            _resultFactory = resultFactory;
        }

        public ICommandResult Handle(UpdatePersonCommand command)
        {
            // Pretend we have a database
            var person = GetPersonFromDb(command.Id);
            if(person == null)
                return _resultFactory.NotFound();

            person.FirstName = command.FirstName;
            person.LastName = command.LastName;

            // We should save this back...

            return _resultFactory.Ok(person);
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