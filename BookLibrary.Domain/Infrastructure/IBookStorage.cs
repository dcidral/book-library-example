using System.Collections.Generic;

namespace BookLibrary.Domain.Infrastructure
{
    public interface IBookStorage
    {
        void Save(Book book);
        Book Get(int id);
        IEnumerable<Book> Search(Filters filters);
        IEnumerable<Book> GetAll();

        public class Filters
        {
            public int number;
            public string title;
            public string author;
        }
    }
}