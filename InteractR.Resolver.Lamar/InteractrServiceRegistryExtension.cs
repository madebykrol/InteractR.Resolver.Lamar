﻿using Lamar;

namespace InteractR.Resolver.Lamar
{
    public static class InteractrServiceRegistryExtension
    {
        public static ServiceRegistry AddInteractr(this ServiceRegistry services)
        {
            services.For<IResolver>().Use(context => new LamarResolver(context));
            services.For<IInteractorHub>().Use<Hub>();

            return services;
        }
    }
}
