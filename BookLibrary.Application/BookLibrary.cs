using BookLibrary.Domain;
using BookLibrary.Domain.Application;
using BookLibrary.Domain.Application.Errors;
using BookLibrary.Domain.Infrastructure;
using System.Collections.Generic;

namespace BookLibrary.Application
{
    public class BookLibrary : IBookLibrary
    {
        private readonly IBookStorage bookStorage;
        private readonly IBookLoanStorage loanStorage;

        public BookLibrary(IBookStorage bookStorage, IBookLoanStorage loanStorage)
        {
            this.bookStorage = bookStorage;
            this.loanStorage = loanStorage;
        }

        public IEnumerable<Book> FindAllBooks()
        {
            return this.bookStorage.GetAll();
        }

        public Book FindBook(int number)
        {
            return bookStorage.Get(number);
        }

        public IEnumerable<BookLoan> FindLoans(int bookNumber)
        {
            if (this.bookStorage.Get(bookNumber) == null)
                throw new BookNotFoundException($"Book not found. Book number = {bookNumber}");
            return this.loanStorage.FindLoans(bookNumber);
        }

        public void InsertNewBook(Book book)
        {
            if (this.bookStorage.Get(book.Number) != null)
                throw new DuplicatedBookException($"Book number already in database. Book number = {book.Number}");
            this.bookStorage.Save(book);
        }

        public void LendBook(BookLoan loan)
        {
            Book book = this.bookStorage.Get(loan.BookNumber);
            if (book == null)
                throw new BookNotFoundException($"Book not found. Book number = {loan.BookNumber}");
            if (book.CurrentLoan != null)
                throw new BookCurrentlyLentException($"Book currently lent. Book number = {book.Number}");
            this.loanStorage.Save(loan);
        }

        public void UpdateBook(Book book)
        {
            if (this.bookStorage.Get(book.Number) == null)
                throw new BookNotFoundException($"Book not found. Book number = {book.Number}");
            this.bookStorage.Save(book);
        }

        public IEnumerable<Book> SearchBooks(BookSearchFilters filters)
        {
            return this.bookStorage.Search(new IBookStorage.Filters
            {
                number = filters.Number,
                author = filters.Author,
                title = filters.Title,
            });
        }
    }
}
