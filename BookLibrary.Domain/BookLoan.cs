using System;

namespace BookLibrary.Domain
{
    public class BookLoan
    {
        public int Id { get; set; }
        public int BookNumber { get; set; }
        public DateTime Borrowed { get; set; }
        public DateTime? Returned { get; set; }
        public string User { get; set; }
    }
}