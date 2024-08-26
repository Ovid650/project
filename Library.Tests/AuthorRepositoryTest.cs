using LibraryDataAccess;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace LibraryDataAccess.Tests
{
    public class AuthorRepositoryTests
    {
        private readonly DbContextOptions<LibraryContext> _options;
        private readonly LibraryContext _context;
        private readonly AuthorRepository _repository;

        public AuthorRepositoryTests()
        {
            // Setup in-memory database options
            _options = new DbContextOptionsBuilder<LibraryContext>()
                            .UseInMemoryDatabase(databaseName: "LibraryTestDb_Author")
                            .Options;

            // Initialize the in-memory context and repository
            _context = new LibraryContext(_options);
            _repository = new AuthorRepository(_context);

            // Seed the database with initial data
            _context.Authors.AddRange(
                new Author { AuthorId = 1, AuthorName = "Author 1" },
                new Author { AuthorId = 2, AuthorName = "Author 2" }
            );
            _context.SaveChanges();
        }

        [Fact]
        public async Task GetAuthorsAsync_ReturnsAllAuthors()
        {
            // Act
            var authors = await _repository.GetAuthorsAsync();

            // Assert
            Assert.Equal(2, authors.Count);
            Assert.Contains(authors, a => a.AuthorName == "Author 1");
            Assert.Contains(authors, a => a.AuthorName == "Author 2");
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsAuthor_WhenAuthorExists()
        {
            // Act
            var author = await _repository.GetAuthorByIdAsync(1);

            // Assert
            Assert.NotNull(author);
            Assert.Equal("Author 1", author.AuthorName);
        }

        [Fact]
        public async Task GetAuthorByIdAsync_ReturnsNull_WhenAuthorDoesNotExist()
        {
            // Act
            var author = await _repository.GetAuthorByIdAsync(99);

            // Assert
            Assert.Null(author);
        }

        [Fact]
        public async Task CreateAuthorAsync_AddsNewAuthor()
        {
            // Arrange
            var newAuthor = new Author { AuthorName = "New Author" };

            // Act
            var createdAuthor = await _repository.CreateAuthorAsync(newAuthor);
            var authorFromDb = await _repository.GetAuthorByIdAsync(createdAuthor.AuthorId);

            // Assert
            Assert.NotNull(authorFromDb);
            Assert.Equal("New Author", authorFromDb.AuthorName);
        }

        [Fact]
        public async Task UpdateAuthorAsync_UpdatesExistingAuthor()
        {
            // Arrange
            var authorToUpdate = await _repository.GetAuthorByIdAsync(1);
            authorToUpdate.AuthorName = "Updated Author";

            // Act
            var updatedAuthor = await _repository.UpdateAuthorAsync(authorToUpdate);
            var authorFromDb = await _repository.GetAuthorByIdAsync(updatedAuthor.AuthorId);

            // Assert
            Assert.NotNull(authorFromDb);
            Assert.Equal("Updated Author", authorFromDb.AuthorName);
        }

        [Fact]
        public async Task DeleteAuthorAsync_RemovesAuthor_WhenAuthorExists()
        {
            // Act
            await _repository.DeleteAuthorAsync(1);
            var authorFromDb = await _repository.GetAuthorByIdAsync(1);

            // Assert
            Assert.Null(authorFromDb);
        }

        [Fact]
        public async Task DeleteAuthorAsync_ThrowsKeyNotFoundException_WhenAuthorDoesNotExist()
        {
            // Act & Assert
            await Assert.ThrowsAsync<KeyNotFoundException>(() => _repository.DeleteAuthorAsync(99));
        }
    }
}
