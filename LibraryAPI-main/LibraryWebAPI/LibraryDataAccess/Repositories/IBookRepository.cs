using LibraryDataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public interface IBookRepository
    {
        Task<List<Book>> GetBooksAsync();

        Task<Book?> GetBookByIdAsync(int id);

        Task<Book> CreateBookAsync(Book book);

        Task<Book> UpdateBookAsync(Book book);

        Task DeleteBookAsync(int id);
    }
}
