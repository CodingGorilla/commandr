## commandr

# WARNING: This project is still highly experimental, use at your own risk

### What is commandr

For year we have been using the outdated notion of controllers in ASP.NET to build our web APIs.  However, the controller itself doesn't serve much of a purpose, other than to group together a bunch of methods that maybe share some bits of routing information.  Recently, the [mediator pattern](https://en.wikipedia.org/wiki/Mediator_pattern) has caught on, causing us to write a lot of boilerplate code like:

```c#
public class MyController : ApiController
{
    private readonly ICommandBus _commandBus;

    public MyController(ICommandBus commandBus)
    {
        _commandBus = commandBus;
    }

    [HttpGet("/something/{id}")]
    public IActionResult GetSomethingApi(int id)
    {
        var command = new GetSomethingCommand { Id = id };
        return _commandBus.Invoke(command);
    }
}
```

Assuming your API has more than one endpoint/command, that's a lot of boilerplate code just to fire off commands and return the result.  Commandr aims to solve that problem by extending ASP.NET Core's endpoint routing feature to bind directly to a command, and then execute the command for you.  No more controllers, ever.

The result is a project that is smaller, cleaner, and in my opinion a lot easier to test because you can simply write unit tests for your command handlers and not worry about creating controllers and all the ceremony therein.

I've been a fan of [Jasper](https://jasperfx.github.io), and I have produced a [simple example](https://github.com/CodingGorilla/commandr/tree/master/samples/Commandr.Samples.Jasper) that ends up being very little code compared to a similar project using MVC controllers.  Commandr, however, aims to be agnostic as to which of the messaging/command stacks you choose.  Commandr could just as easily work with [Mediatr](https://github.com/jbogard/MediatR), or you could even write your own simple executor if you want.

As stated above, Commandr is in it's infancy, so there's still a lot of work to do.  The ultimate goal is for it to be a drop-in replacement for ASP.NET MVC for web APIs (it will never try to replace Razor/Blazor).

Would love to hear your thoughts and ideas!