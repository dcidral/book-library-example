using BookLibrary.Domain;
using LiteDB;

namespace BookLibrary.Infrastructure.LiteDB
{
    public class DataBaseMapper
    {
        public static void ConfigureDB()
        {
            var mapper = BsonMapper.Global;

            mapper.Entity<Book>()
                .Id(x => x.Number)
                .Ignore(x => x.CurrentLoan)
                .Ignore(x => x.LoansCount);

            mapper.Entity<BookLoan>()
                .Id(x => x.Id);
        }
    }
}
