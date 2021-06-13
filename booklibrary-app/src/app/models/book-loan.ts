export class BookLoan {
  public id?: number;
  public bookNumber!: number;
  public borrowed!: Date;
  public returned?: Date;
  public user!: string;
}
