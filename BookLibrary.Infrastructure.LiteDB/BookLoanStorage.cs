using BookLibrary.Domain;
using BookLibrary.Domain.Infrastructure;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Infrastructure.LiteDB
{
    public class BookLoanStorage : IBookLoanStorage
    {
        private const string collectionName = "bookloans";
        private readonly string dbFilePath;

        public BookLoanStorage(string dbFilePath)
        {
            this.dbFilePath = dbFilePath;
        }

        public BookLoan FindActiveLoan(int bookNumber)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            return FindActiveLoan(bookNumber, db);
        }

        internal BookLoan FindActiveLoan(int bookNumber, LiteDatabase db)
        {
            ILiteCollection<BookLoan> loans = db.GetCollection<BookLoan>(collectionName);
            return loans.Find(l => l.BookNumber == bookNumber && l.Returned == null).FirstOrDefault();
        }

        public IEnumerable<BookLoan> FindLoans(int bookNumber)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<BookLoan> loans = db.GetCollection<BookLoan>(collectionName);
            return loans.Find(l => l.BookNumber == bookNumber).OrderBy(l => l.Borrowed).ToList();
        }

        public BookLoan Get(int id)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<BookLoan> loans = db.GetCollection<BookLoan>(collectionName);
            return loans.Find(l => l.Id == id).FirstOrDefault();
        }

        public void Save(BookLoan loan)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<BookLoan> loans = db.GetCollection<BookLoan>(collectionName);
            loans.Upsert(loan);
        }

        internal int GetLoanCount(int bookNumber, LiteDatabase db)
        {
            ILiteCollection<BookLoan> loans = db.GetCollection<BookLoan>(collectionName);
            return loans.Count(l => l.BookNumber == bookNumber);
        }
    }
}
