﻿using InteractR.Interactor;

namespace InteractR.Resolver.Lamar.Tests.Mocks
{
    public class MockUseCase : IUseCase<IMockOutputPort>, IHasPolicy
    {
        public string Policy => "MockPolicy";
    }
}
