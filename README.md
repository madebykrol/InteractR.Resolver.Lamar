# InteractR.Resolver.Lamar
For documentation see [InteractR](https://github.com/madebykrol/InteractR)

Built for Lamar 8.1.0 for other versions please use this repository for reference and roll your own.


Install from nuget.
```PowerShell
PM > Install-Package InteractR.Resolver.Lamar -Version 8.0.0
```

# Usage 
Either you can register the resolver yourself.
```Csharp
For<IResolver>().Use(context => new StructureMapResolver(context));
For<IInteractorHub>().Use<Hub>();
```

Or you can use the provided ServiceRegistry extesion method
```Csharp
services.AddInteractr()
```

# Breaking changes
## 4.0.0
This package no longer ships with a ResolverModule - Instead use 'AddInteractr' extension method
