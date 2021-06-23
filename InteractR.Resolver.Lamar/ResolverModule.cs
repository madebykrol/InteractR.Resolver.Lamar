using Lamar;

namespace InteractR.Resolver.Lamar
{
    public class ResolverModule : ServiceRegistry
    {
        public ResolverModule()
        {
            For<IResolver>().Use(context => new LamarResolver(context));
            For<IInteractorHub>().Use<Hub>();
        }
    }
}
