import { BookLoan } from "./book-loan";

export class Book {
  public number?: number;
  public title!: string;
  public author!: string;
  public currentLoan?: BookLoan;
  public loansCount?: number;

  public isLent(): boolean {
    return this.currentLoan ? true : false;
  }
}
