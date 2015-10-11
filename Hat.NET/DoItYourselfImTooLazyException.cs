using System;
using System.Runtime.Serialization;

namespace Hat.NET
{
    [Serializable]
    internal class DoItYourselfImTooLazyException : Exception
    {
        public DoItYourselfImTooLazyException()
        {
        }

        public DoItYourselfImTooLazyException(string message) : base(message)
        {
        }

        public DoItYourselfImTooLazyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DoItYourselfImTooLazyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}