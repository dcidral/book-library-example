import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { Book } from 'src/app/models/book';
import { LoanRequest } from 'src/app/models/loan-request';
import { LibraryApiService } from 'src/app/services/library-api.service';

@Component({
  selector: 'app-book-list',
  templateUrl: './book-list.component.html',
  styleUrls: ['./book-list.component.scss']
})
export class BookListComponent implements OnInit {
  displayedColumns = [
    'number',
    'title',
    'author',
    'loansCount',
    'actions',
  ];

  books: Book[] = [];

  constructor(
    private api: LibraryApiService,
    private router: Router) { }

  ngOnInit(): void {
    this.api.getAllBooks().then(result => this.books = result);
  }

  editBook(book: Book) {
    this.router.navigateByUrl('/edit-book?id=' + book.number);
  }

  loanBook(book: Book) {
    var loan = new LoanRequest();
    loan.user = 'user';
    loan.borrowed = new Date();
    this.api.loanBook(book, loan).then((r) => {
      this.router.navigateByUrl('/');
    });
  }
}
