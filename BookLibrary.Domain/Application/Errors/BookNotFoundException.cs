using System;
using System.Runtime.Serialization;

namespace BookLibrary.Domain.Application.Errors
{
    public class BookNotFoundException : LibraryException
    {
        public BookNotFoundException()
        {
        }

        public BookNotFoundException(string message) : base(message)
        {
        }

        public BookNotFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected BookNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
