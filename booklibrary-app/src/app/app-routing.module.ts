import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { BookListComponent } from './books/book-list/book-list.component';
import { SaveBookComponent } from './books/save-book/save-book.component';

const routes: Routes = [
  { path: '', component: BookListComponent },
  { path: 'edit-book', component: SaveBookComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
