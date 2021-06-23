using System.Threading;
using System.Threading.Tasks;
using InteractR.Interactor;

namespace InteractR.Resolver.Lamar.Tests.Mocks
{
    public class MockInteractor : IInteractor<MockUseCase, IMockOutputPort>
    {
        public Task<UseCaseResult> Execute(MockUseCase usecase, IMockOutputPort outputPort, CancellationToken cancellationToken)
        {
            return Task.FromResult(new UseCaseResult(true));
        }
    }
}
