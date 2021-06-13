using System;
using System.Collections.Generic;
using System.Text;

namespace BookLibrary.Domain.Application
{
    public interface IBookLibrary
    {
        void InsertNewBook(Book book);
        void UpdateBook(Book book);
        Book FindBook(int number);
        IEnumerable<Book> SearchBooks(BookSearchFilters filters);
        IEnumerable<Book> FindAllBooks();
        IEnumerable<BookLoan> FindLoans(int bookNumber);
        void LendBook(BookLoan loan);
    }
}