using System;
using System.Runtime.Serialization;

namespace InteractR.Resolver.Lamar.Tests.Mocks
{
    public class MockException : Exception
    {
        public MockException()
        {
        }

        protected MockException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public MockException(string? message) : base(message)
        {
        }

        public MockException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
