using System.Collections.Generic;
using System.Linq;
using InteractR.Interactor;
using Lamar;

namespace InteractR.Resolver.Lamar
{
    public class LamarResolver : IResolver
    {
        private readonly IServiceContext _context;
        public LamarResolver(IServiceContext context)
        {
            _context = context;
        }

        private T Resolve<T>() => _context.GetInstance<T>();

        public IInteractor<TUseCase, TOutputPort> ResolveInteractor<TUseCase, TOutputPort>(TUseCase useCase) 
            where TUseCase : IUseCase<TOutputPort> 
                => Resolve<IInteractor<TUseCase, TOutputPort>>();

        public IReadOnlyList<IMiddleware<TUseCase, TOutputPort>> ResolveMiddleware<TUseCase, TOutputPort>(TUseCase useCase) 
            where TUseCase : IUseCase<TOutputPort> 
                => _context.GetAllInstances<IMiddleware<TUseCase, TOutputPort>>().ToList();

        public IReadOnlyList<IMiddleware> ResolveGlobalMiddleware() 
            => _context.GetAllInstances<IMiddleware>().ToList();
    }
}
