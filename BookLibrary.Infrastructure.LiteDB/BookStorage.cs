using BookLibrary.Domain;
using BookLibrary.Domain.Infrastructure;
using LiteDB;
using System.Collections.Generic;
using System.Linq;

namespace BookLibrary.Infrastructure.LiteDB
{
    public class BookStorage : IBookStorage
    {
        private const string collectionName = "books";
        private readonly string dbFilePath;
        private readonly BookLoanStorage loanStorage;

        public BookStorage(string dbFilePath, BookLoanStorage loanStorage)
        {
            this.dbFilePath = dbFilePath;
            this.loanStorage = loanStorage;
        }

        public Book Get(int number)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<Book> books = db.GetCollection<Book>(collectionName);
            Book book = books.Find(b => b.Number == number).FirstOrDefault();
            if (book != null)
                book.CurrentLoan = this.loanStorage.FindActiveLoan(number, db);
            return book;
        }

        public void Save(Book book)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<Book> books = db.GetCollection<Book>(collectionName);
            if (books.Exists(b => b.Number == book.Number))
                books.Update(book);
            else
                books.Insert(book);
        }

        public IEnumerable<Book> Search(IBookStorage.Filters filters)
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<Book> books = db.GetCollection<Book>(collectionName);
            IEnumerable<Book> results = books.Find(b =>
                (filters.number == 0 || b.Number == filters.number)
                && (string.IsNullOrEmpty(filters.author) || b.Author.Contains(filters.author))
                && (string.IsNullOrEmpty(filters.title) || b.Title.Contains(filters.title))
            );
            this.LoadLoans(results, db);
            return results.ToList();
        }

        public IEnumerable<Book> GetAll()
        {
            using LiteDatabase db = new LiteDatabase(this.dbFilePath);
            ILiteCollection<Book> books = db.GetCollection<Book>(collectionName);
            IEnumerable<Book> allbooks = books.FindAll();
            this.LoadLoans(allbooks, db);
            return allbooks.ToList();
        }

        private void LoadLoans(IEnumerable<Book> books, LiteDatabase db)
        {
            foreach (Book book in books)
            {
                book.CurrentLoan = this.loanStorage.FindActiveLoan(book.Number, db);
                book.LoansCount = this.loanStorage.GetLoanCount(book.Number, db);
            }
        }
    }
}