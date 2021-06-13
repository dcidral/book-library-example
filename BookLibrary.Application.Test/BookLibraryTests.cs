using NUnit.Framework;
using Moq;
using BookLibrary.Domain.Infrastructure;
using BookLibrary.Domain;
using System.Collections.Generic;
using System.Linq;
using System;
using BookLibrary.Domain.Application.Errors;
using BookLibrary.Domain.Application;

namespace BookLibrary.Application.Test
{
    public class BookLibraryTests
    {
        private Mock<IBookLoanStorage> loanStorageMock;
        private Mock<IBookStorage> bookStorageMock;
        private BookLibrary bookLibrary;
        private Book book1 = new Book
        {
            Number = 1,
            Author = "Carl Sagan",
            Title = "Cosmos",
            CurrentLoan = new BookLoan
            {
                Id = 2,
                BookNumber = 1,
                Borrowed = new DateTime(2021, 3, 1),
                User = "cidral"
            },
        };
        private Book book2 = new Book
        {
            Number = 2,
            Author = "Charles Darwin",
            Title = "On the Origin of Species",
        };
        private BookLoan loan1_book1 = new BookLoan
        {
            Id = 1,
            BookNumber = 1,
            Borrowed = new DateTime(2021, 1, 1),
            Returned = new DateTime(2021, 2, 20),
        };
        private BookLoan active_loan_book1 = new BookLoan
        {
            Id = 2,
            BookNumber = 1,
            Borrowed = new DateTime(2021, 3, 1),
            User = "cidral"
        };
        private BookLoan loan1_book2 = new BookLoan
        {
            Id = 3,
            BookNumber = 2,
            Borrowed = new DateTime(2021, 1, 1),
            Returned = new DateTime(2021, 2, 20),
        };

        [SetUp]
        public void Setup()
        {
            this.bookStorageMock = new Mock<IBookStorage>();
            bookStorageMock.Setup(bs => bs.GetAll()).Returns(new[] { book1, book2, });
            bookStorageMock.Setup(bs => bs.Get(1)).Returns(this.book1);
            bookStorageMock.Setup(bs => bs.Get(2)).Returns(this.book2);
            bookStorageMock.Setup(bs => bs.Get(It.IsNotIn(new[] { 1, 2 }))).Returns((Book)null);

            this.loanStorageMock = new Mock<IBookLoanStorage>();
            loanStorageMock.Setup(ls => ls.FindLoans(1)).Returns(new[] { loan1_book1, active_loan_book1 });
            loanStorageMock.Setup(ls => ls.FindLoans(2)).Returns(new[] { loan1_book2 });
            loanStorageMock.Setup(ls => ls.FindLoans(It.IsNotIn(new[] { 1, 2 }))).Returns(new BookLoan[] { });
            loanStorageMock.Setup(ls => ls.FindActiveLoan(1)).Returns(active_loan_book1);

            this.bookLibrary = new BookLibrary(bookStorageMock.Object, loanStorageMock.Object);
        }

        [Test]
        public void FindBook_with_existing_number_should_return_correct_book()
        {
            Book book = this.bookLibrary.FindBook(1);
            Assert.AreEqual(this.book1, book);
        }

        [Test]
        public void FindBook_with_non_existing_number_should_return_null()
        {
            Book book = this.bookLibrary.FindBook(999);
            Assert.IsNull(book);
        }

        [Test]
        public void FindAllBooks_should_return_all_the_books_from_the_storage()
        {
            IEnumerable<Book> books = this.bookLibrary.FindAllBooks();
            Assert.True(books.All(b => b == book1 || b == book2));
        }

        [Test]
        public void FindLoans_should_return_all_the_loans_of_a_book_from_the_storage()
        {
            IEnumerable<BookLoan> loans = this.bookLibrary.FindLoans(1);
            Assert.True(loans.All(l => l == this.loan1_book1 || l == this.active_loan_book1));
        }

        [Test]
        public void FindLoans_from_invalid_book_should_throw_BookNotFoundException()
        {
            Assert.Throws<BookNotFoundException>(() =>
            {
                IEnumerable<BookLoan> loans = this.bookLibrary.FindLoans(999);
            });
        }

        [Test]
        public void InsertNewBook_should_record_book_at_the_storage()
        {
            Book newBook = new Book
            {
                Number = 3,
                Title = "Clean Code",
                Author = "Robert C. Martin",
            };
            this.bookLibrary.InsertNewBook(newBook);
            this.bookStorageMock.Verify((bs) => bs.Save(newBook));
        }

        [Test]
        public void InsertNewBook_with_existing_number_should_throw_DuplicatedBookException()
        {
            Book newBook = new Book
            {
                Number = 1,
                Title = "Clean Code",
                Author = "Robert C. Martin",
            };
            Assert.Throws<DuplicatedBookException>(() =>
            {
                this.bookLibrary.InsertNewBook(newBook);
            });
        }

        [Test]
        public void LendBook_should_save_loan_on_database()
        {
            BookLoan loan = new BookLoan
            {
                BookNumber = 2,
                Borrowed = new DateTime(2021, 1, 1),
                User = "user",
            };
            this.bookLibrary.LendBook(loan);
            this.loanStorageMock.Verify(ls => ls.Save(loan));
        }

        [Test]
        public void LendBook_with_invalid_book_number_should_throw_BookNotFoundException()
        {
            BookLoan loan = new BookLoan
            {
                BookNumber = 999,
                Borrowed = new DateTime(2021, 1, 1),
                User = "user",
            };
            Assert.Throws<BookNotFoundException>(() =>
            {
                this.bookLibrary.LendBook(loan);
            });
        }

        [Test]
        public void LendBook_with_lent_book_should_throw_BookCurrentlyLentException()
        {
            BookLoan loan = new BookLoan
            {
                BookNumber = 1,
                Borrowed = new DateTime(2021, 1, 1),
                User = "user",
            };
            Assert.Throws<BookCurrentlyLentException>(() =>
            {
                this.bookLibrary.LendBook(loan);
            });
        }

        [Test]
        public void UpdateBook_should_save_book_at_storage()
        {
            Book book = new Book
            {
                Number = 1,
                Title = "Clean Code",
                Author = "Robert C. Martin",
            };
            this.bookLibrary.UpdateBook(book);
            this.bookStorageMock.Verify(bs => bs.Save(book));
        }

        [Test]
        public void UpdateBook_with_invalid_book_number_should_throw_BookNotFoundException()
        {
            Book book = new Book
            {
                Number = 999,
                Title = "Clean Code",
                Author = "Robert C. Martin",
            };
            Assert.Throws<BookNotFoundException>(() =>
            {
                this.bookLibrary.UpdateBook(book);
            });
        }

        [Test]
        public void SearchBooks_should_return_same_books_from_bookstorage_search()
        {
            Book[] allBooks = new[] { book1, book2 };
            this.bookStorageMock.Setup(bs => bs.Search(It.IsAny<IBookStorage.Filters>())).Returns(allBooks);
            BookSearchFilters filters = new BookSearchFilters();
            IEnumerable<Book> books = this.bookLibrary.SearchBooks(filters);
            Assert.True(allBooks.All(b => books.Contains(b)));
        }

        [Test]
        public void SearchBooks_should_seach_database_with_same_filters()
        {
            Book[] allBooks = new[] { book1, book2 };
            this.bookStorageMock.Setup(bs => bs.Search(It.IsAny<IBookStorage.Filters>())).Returns(allBooks);
            BookSearchFilters filters = new BookSearchFilters()
            {
                Author = "Author",
                Number = 123,
                Title = "title",
            };
            IEnumerable<Book> books = this.bookLibrary.SearchBooks(filters);
            this.bookStorageMock.Verify(bs => bs.Search(Match.Create<IBookStorage.Filters>(
                f => f.author == filters.Author && f.title == filters.Title && f.number == filters.Number)));
        }
    }
}