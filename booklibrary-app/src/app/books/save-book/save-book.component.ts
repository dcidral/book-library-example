import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Book } from 'src/app/models/book';
import { LibraryApiService } from 'src/app/services/library-api.service';

@Component({
  selector: 'app-save-book',
  templateUrl: './save-book.component.html',
  styleUrls: ['./save-book.component.scss']
})
export class SaveBookComponent implements OnInit {

  book: Book = new Book();
  isNewBook = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private api: LibraryApiService) { }

  ngOnInit(): void {
    this.route.queryParams.subscribe(params => {
        var bookNumber = new Number(params['id']);
        console.log(bookNumber);
        if (bookNumber.valueOf() > 0)
          this.api.getBook(bookNumber).then(b => this.book = b);
        else
          this.isNewBook = true;
    });
  }

  save(): void {
    this.api.saveBook(this.book, this.isNewBook).then((r) => {
      this.router.navigate(['/'])
    }).catch((e) => {
      console.log(e);
    });
  }
}
