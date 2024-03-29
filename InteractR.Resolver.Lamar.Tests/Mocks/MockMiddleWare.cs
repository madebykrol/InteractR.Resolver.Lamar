﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using InteractR.Interactor;

namespace InteractR.Resolver.Lamar.Tests.Mocks
{
    public class MockMiddleWare : IMiddleware<IHasPolicy>
    {
        public int Run = 0;
        public Task<UseCaseResult> Execute<TUseCase>(TUseCase usecase, Func<TUseCase, Task<UseCaseResult>> next, CancellationToken cancellationToken) where TUseCase : IHasPolicy
        {
            Run++;

            return next.Invoke(usecase);
        }
    }
}
