using System;
using System.Runtime.Serialization;

namespace MvcRouteTester.Assertions
{
    [Serializable]
    public class AssertionException : Exception
    {
        internal AssertionException(string message)
            : base(message)
        {
        }

        protected AssertionException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
