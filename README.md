# miriwork

Miriwork is an addon for Asp .Net Core to provide **message-based communication** between client and server. It is based on some thoughts of **domain driven design** and can be used to separate the application logic - the application services - from the technical aspects (e.g. Controller of Asp .Net Core). Further it can help you to organize your application.

## Simple example

First you have to create one or more "bounded contexts". It depends on your application how to divide your domain in self-contained bounded contexts.

```C#
public class SimpleBoundedContext : IBoundedContext
{
    // optional id of bounded context
    public object Id => null;

    // assembly which contains application services
    public ApplicationServicesAssembly ApplicationServicesAssembly => ApplicationServicesAssembly.FromCallingAssembly();

    // dependency injection of bounded context
    public RegistrationResult RegisterDependencies(IServiceCollection services)
    {
        // register some dependencies
        return RegistrationResult.EverythingRegistered();
    }
}
```

Then you write some application services to handle GET-/PUT-/POST-/DELETE-requests. How to organize the services respectively requests depends on your application as well.

```C#
public class SimpleApplicationService
{
    public SimpleResponse Get(SimpleRequest request)
    {
        return new SimpleResponse
        {
            SimpleString = request.SimpleString
        };
    }

    // add Put() oder Post() or Delete() if needed
}

public class SimpleRequest
{
    public string SimpleString { get; set; }
}

public class SimpleResponse
{
    public string SimpleString { get; set; }
}
```

Finally you register Miriwork in the Startup class of your Asp .Net Core application.

```C#
services.AddMvc().AddMiriwork(services, typeof(SimpleBoundedContext));
```

Check out Example1 and Example2 in source code too. There you can find how to configure Miriwork or handle exceptions for instance.

## License

You can use the source code like you want (MIT license). There is no nuget-package currently.
