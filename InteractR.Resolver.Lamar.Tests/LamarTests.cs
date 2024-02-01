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
                c.AddInteractr();
            });

            _interactorHub = container.GetInstance<IInteractorHub>();
        }

        [Test]
        public async Task Test_Lamar_Resolver()
        {
            await _interactorHub.Execute(new MockUseCase(), new MockOutputPort());
            await _useCaseInteractor.Received().Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Test_Pipeline()
        {
            await _interactorHub.Execute(new MockUseCase(), new MockOutputPort());
            await _middleware2.ReceivedWithAnyArgs(1).Execute(Arg.Any<MockUseCase>(), Arg.Any<IMockOutputPort>(), Arg.Any<Func<MockUseCase, Task<UseCaseResult>>>(),
                Arg.Any<CancellationToken>());
        }

        [Test]
        public async Task Test_GenericMiddleware_InterfaceOnClass()
        {
            var middleware = new MockMiddleWare();
            var container = new Container(c =>
            {
                c.For<IInteractor<MockUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<IHasPolicy>>().Use(middleware).Singleton();
                c.AddInteractr();
            });

            var interactorHub = container.GetInstance<IInteractorHub>();

            _ = await interactorHub.Execute(new MockUseCase(), new MockOutputPort());

            Assert.That(middleware.Run, Is.EqualTo(1));
        }

        [Test]
        public async Task Test_GenericMiddleware_InterfaceOnBaseClass()
        {
            var middleware = new MockMiddleWare();
            var container = new Container(c =>
            {
                c.For<IInteractor<MockSubUseCase, IMockOutputPort>>().Use(_useCaseInteractor);
                c.For<IMiddleware<IHasPolicy>>().Use(middleware).Singleton();
                c.AddInteractr();
            });

            var interactorHub = container.GetInstance<IInteractorHub>();

            _ = await interactorHub.Execute(new MockSubUseCase(), new MockOutputPort());

            Assert.That(middleware.Run, Is.EqualTo(1));
        }
    }
}
