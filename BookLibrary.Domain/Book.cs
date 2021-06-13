namespace BookLibrary.Domain
{
    public class Book
    {
        public int Number { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public BookLoan CurrentLoan { get; set; }
        public int LoansCount { get; set; }
    }
}