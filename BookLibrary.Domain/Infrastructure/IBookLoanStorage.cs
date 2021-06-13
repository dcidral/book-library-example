using System.Collections.Generic;

namespace BookLibrary.Domain.Infrastructure
{
    public interface IBookLoanStorage
    {
        void Save(BookLoan loan);
        BookLoan Get(int id);
        IEnumerable<BookLoan> FindLoans(int bookNumber);
        BookLoan FindActiveLoan(int bookNumber);
    }
}