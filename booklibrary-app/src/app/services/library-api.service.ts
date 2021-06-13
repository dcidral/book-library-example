import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Book } from '../models/book';
import { LoanRequest } from '../models/loan-request';

@Injectable({
  providedIn: 'root'
})
export class LibraryApiService {
  private baseUrl = "http://localhost:53900/api/";

  constructor(private http: HttpClient) { }

  public async getAllBooks(): Promise<Book[]> {
    return await this.http.get<Book[]>(this.baseUrl + 'book/all').toPromise();
  }

  public async getBook(id: any): Promise<Book> {
    return await this.http.get<Book>(this.baseUrl + 'book/' + id).toPromise();
  }

  public async saveBook(book: Book, create: boolean): Promise<any> {
    if (create)
      return await this.http.post<any>(this.baseUrl + 'book/', book).toPromise();
    else
      return await this.http.put<any>(this.baseUrl + 'book/' + book.number, book).toPromise();
  }

  public async loanBook(book: Book, loan: LoanRequest): Promise<any> {
    return await this.http.post<any>(this.baseUrl + 'book/' + book.number + '/loan', loan).toPromise();
  }
}
