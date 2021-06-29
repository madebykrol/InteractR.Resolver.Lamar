using System;
using System.Threading;
using System.Threading.Tasks;
using InteractR.Interactor;
using InteractR.Resolver.Lamar.Tests.Mocks;
using Lamar;
using NSubstitute;
using NUnit.Framework;


namespace InteractR.Resolver.Lamar.Tests
{
    [TestFixture]
    public class LamarTests
    {
        private IInteractorHub _interactorHub;
        private IInteractor<MockUseCase, IMockOutputPort> _useCaseInteractor;

        private IMiddleware<MockUseCase, IMockOutputPort> _middleware1;
        private IMiddleware<MockUseCase, IMockOutputPort> _middleware2;

        [SetUp]
        public void Setup()
        {
            _useCaseInteractor = Substitute.For<IInteractor<MockUseCase, IMockOutputPort>>();

            _middleware1 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware1.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));

            _middleware2 = Substitute.For<IMiddleware<MockUseCase, IMockOutputPort>>();
            _middleware2.Execute(
                    Arg.Any<MockUseCase>(),
                    Arg.Any<IMockOutputPort>(),
                    d => Task.FromResult(new UseCaseResult(true)),
                    Arg.Any<CancellationToken>())
                .ReturnsForAnyArgs(x => new UseCaseResult(true))
                .AndDoes(x => x.Arg<Func<MockUseCase, Task<UseCaseResult>>>().Invoke(x.Arg<MockUseCase>()));


            var container = new Container(c =>
            {
                c.For<IInteractor<MockUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<MockUseCase, IMockOutputPort>>().Use(_middleware1);
                c.For<IMiddleware<MockUseCase, IMockOutputPort>>().Use(_middleware2);
                c.IncludeRegistry<ResolverModule>();
            });

            _interactorHub = container.GetInstance<IInteractorHub>();
        }

        [Test]
        public async Task Test_Lamar_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Test_Pipeline()
        {
            await _interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            await _middleware2.ReceivedWithAnyArgs().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<Func<MockUseCase, Task<UseCaseResult>>>(),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public void Test_GenericMiddleware_InterfaceOnClass()
        {
            var container = new Container(c =>
            {
                c.For<IInteractor<MockUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<IHasPolicy>>().Use<MockMiddleWare>();
                c.IncludeRegistry<ResolverModule>();
            });

            var interactorHub = container.GetInstance<IInteractorHub>();
            Assert.ThrowsAsync<MockException>(async () =>
            {
                await interactorHub.Execute(new MockUseCase(), (IMockOutputPort)new MockOutputPort());
            });
        }

        [Test]
        public void Test_GenericMiddleware_InterfaceOnBaseClass()
        {
            var container = new Container(c =>
            {
                c.For<IInteractor<MockSubUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<IHasPolicy>>().Use<MockMiddleWare>();
                c.IncludeRegistry<ResolverModule>();
            });

            var interactorHub = container.GetInstance<IInteractorHub>();
            Assert.ThrowsAsync<MockException>(async () =>
            {
                await interactorHub.Execute(new MockSubUseCase(), (IMockOutputPort)new MockOutputPort());
            });
        }
    }
}
