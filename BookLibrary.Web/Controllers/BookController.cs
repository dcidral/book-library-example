using BookLibrary.Domain;
using BookLibrary.Domain.Application;
using BookLibrary.Domain.Application.Errors;
using BookLibrary.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookLibrary.Web.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookLibrary library;

        public BookController(IBookLibrary library)
        {
            this.library = library;
        }

        [HttpGet]
        [Route("{number}")]
        public Book Get(int number)
        {
            return library.FindBook(number);
        }

        [HttpPost]
        public IActionResult Post(Book book)
        {
            try
            {
                library.InsertNewBook(book);
                return base.Created($"book/{book.Number}", book);
            }
            catch (DuplicatedBookException e)
            {
                return base.Conflict(new ErrorModel
                {
                    Message = $"A book with number {book.Number} already exists.",
                    AditionalInfo = e.Message,
                });
            }
        }

        [HttpPut]
        [Route("{number}")]
        public IActionResult Put(int number, [FromBody] Book book)
        {
            if (number != book.Number)
                return base.BadRequest(new ErrorModel { Message = $"Wrong book number." });
            try
            {
                library.UpdateBook(book);
                return base.Ok();
            }
            catch (BookNotFoundException e)
            {
                return base.NotFound(new ErrorModel
                {
                    Message = $"No book with number {book.Number} exists.",
                    AditionalInfo = e.Message,
                });
            }
        }

        [HttpGet]
        [Route("all")]
        public IEnumerable<Book> GetAll()
        {
            return library.FindAllBooks();
        }

        [HttpGet]
        [Route("search")]
        public IEnumerable<Book> Search([FromQuery] BookSearchFilters filters)
        {
            return library.SearchBooks(filters);
        }

        [HttpGet]
        [Route("{bookNumber}/loan/all")]
        public IEnumerable<BookLoan> FindAllLoansFromBook(int bookNumber)
        {
            return library.FindLoans(bookNumber);
        }

        [HttpPost]
        [Route("{bookNumber}/loan")]
        public IActionResult LendBook(int bookNumber, [FromBody] LoanRequestModel loanRequest)
        {
            BookLoan loan = new BookLoan
            {
                BookNumber = bookNumber,
                Borrowed = loanRequest.Borrowed,
                User = loanRequest.User,
            };
            try
            {
                this.library.LendBook(loan);
                return Ok();
            }
            catch (BookNotFoundException)
            {
                return base.NotFound(new ErrorModel
                {
                    Message = $"No book with number {bookNumber} exists.",
                });
            }
            catch (BookCurrentlyLentException)
            {
                return base.Conflict(new ErrorModel
                {
                    Message = $"The book with number {bookNumber} is currently loaned.",
                });
            }
        }
    }
}