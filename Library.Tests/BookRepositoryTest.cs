using LibraryDataAccess;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryDataAccess.Tests
{
    public class BookRepositoryTests
    {
        private readonly DbContextOptions<LibraryContext> _options;
        private readonly LibraryContext _context;
        private readonly BookRepository _repository;

        public BookRepositoryTests()
        {
            _options = new DbContextOptionsBuilder<LibraryContext>()
                            .UseInMemoryDatabase(databaseName: "LibraryTestDb")
                            .Options;

            _context = new LibraryContext(_options);
            _repository = new BookRepository(_context);

            
            _context.Books.AddRange(
                new Book { BookId = 1, Title = "Test Book 1", Price = 10 },
                new Book { BookId = 2, Title = "Test Book 2", Price = 15 }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetBooksAsync_ReturnsAllBooks()
        {
            // Act
            var books = await _repository.GetBooksAsync();

            // Assert
            Assert.Equal(2, books.Count);
            Assert.Contains(books, b => b.Title == "Test Book 1");
            Assert.Contains(books, b => b.Title == "Test Book 2");
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsBook_WhenBookExists()
        {
            // Act
            var book = await _repository.GetBookByIdAsync(1);

            // Assert
            Assert.NotNull(book);
            Assert.Equal("Test Book 1", book.Title);
        }

        [Fact]
        public async Task GetBookByIdAsync_ReturnsNull_WhenBookDoesNotExist()
        {
            // Act
            var book = await _repository.GetBookByIdAsync(99);

            // Assert
            Assert.Null(book);
        }

        [Fact]
        public async Task CreateBookAsync_AddsNewBook()
        {
            // Arrange
            var newBook = new Book { Title = "New Test Book", Price = 20 };

            // Act
            var createdBook = await _repository.CreateBookAsync(newBook);
            var bookFromDb = await _repository.GetBookByIdAsync(createdBook.BookId);

            // Assert
            Assert.NotNull(bookFromDb);
            Assert.Equal("New Test Book", bookFromDb.Title);
            Assert.Equal(20, bookFromDb.Price);
        }

        [Fact]
        public async Task UpdateBookAsync_UpdatesExistingBook()
        {
            // Arrange
            var bookToUpdate = await _repository.GetBookByIdAsync(1);
            bookToUpdate.Title = "Updated Test Book";

            // Act
            var updatedBook = await _repository.UpdateBookAsync(bookToUpdate);
            var bookFromDb = await _repository.GetBookByIdAsync(updatedBook.BookId);

            // Assert
            Assert.NotNull(bookFromDb);
            Assert.Equal("Updated Test Book", bookFromDb.Title);
        }

        [Fact]
        public async Task DeleteBookAsync_RemovesBook_WhenBookExists()
        {
            // Act
            await _repository.DeleteBookAsync(1);
            var bookFromDb = await _repository.GetBookByIdAsync(1);

            // Assert
            Assert.Null(bookFromDb);
        }

        [Fact]
        public async Task DeleteBookAsync_ThrowsKeyNotFoundException_WhenBookDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteBookAsync(99));
        }
    }
}
