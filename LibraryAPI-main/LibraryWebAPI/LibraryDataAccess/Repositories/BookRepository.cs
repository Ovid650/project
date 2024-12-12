using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryDataAccess.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly LibraryContext _context;

        public BookRepository(LibraryContext context)
        {
            _context = context;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            return await _context.Books
                                 .Include(b => b.Category)
                                 .Include(b => b.Author)
                                 .ToListAsync();
        }

        public async Task<Book?> GetBookByIdAsync(int id)
        {
            return await _context.Books
                                 .Include(b => b.Category)
                                 .Include(b => b.Author)
                                 .FirstOrDefaultAsync(b => b.BookId == id);
        }

        public async Task<Book> CreateBookAsync(Book book)
        {
            var bookAdded = await _context.Books.AddAsync(book);
            await _context.SaveChangesAsync();
            return bookAdded.Entity;
        }

        public async Task<Book> UpdateBookAsync(Book book)
        {
            var bookToUpdate = await _context.Books.FirstOrDefaultAsync(b => b.BookId == book.BookId);
            if (bookToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            bookToUpdate.Title = book.Title;
            bookToUpdate.Price = book.Price;
            bookToUpdate.CategoryId = book.CategoryId;
            bookToUpdate.AuthorId = book.AuthorId;

            var bookUpdated = _context.Update(bookToUpdate);
            await _context.SaveChangesAsync();
            return bookUpdated.Entity;
        }

        public async Task DeleteBookAsync(int id)
        {
            var bookToDelete = await _context.Books.FirstOrDefaultAsync(b => b.BookId == id);
            if (bookToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Books.Remove(bookToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
