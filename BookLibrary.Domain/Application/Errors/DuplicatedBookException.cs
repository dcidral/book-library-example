using System;
using System.Runtime.Serialization;

namespace BookLibrary.Domain.Application.Errors
{
    public class DuplicatedBookException : LibraryException
    {
        public DuplicatedBookException()
        {
        }

        public DuplicatedBookException(string message) : base(message)
        {
        }

        public DuplicatedBookException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DuplicatedBookException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}