using LibraryDataAccess.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public interface IAuthorRepository
    {
        Task<List<Author>> GetAuthorsAsync();

        Task<Author?> GetAuthorByIdAsync(int id);

        Task<Author> CreateAuthorAsync(Author author);

        Task<Author> UpdateAuthorAsync(Author author);

        Task DeleteAuthorAsync(int id);
    }
}
