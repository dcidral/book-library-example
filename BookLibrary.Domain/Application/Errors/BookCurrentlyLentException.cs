using System;
using System.Runtime.Serialization;

namespace BookLibrary.Domain.Application.Errors
{
    public class BookCurrentlyLentException : LibraryException
    {
        public BookCurrentlyLentException()
        {
        }

        public BookCurrentlyLentException(string message) : base(message)
        {
        }

        public BookCurrentlyLentException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookCurrentlyLentException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
