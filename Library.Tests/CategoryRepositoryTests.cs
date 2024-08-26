using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using LibraryDataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Library.Tests
{
    public class CategoryRepositoryTests
    {
        private List<Category> _categories = new List<Category>
        {
            new Category {CategoryId = 1, Name = "FirstCategory"},
            new Category {CategoryId = 2, Name = "SecondCategory"},
            new Category {CategoryId = 3, Name = "ThirdCategory"},
            new Category {CategoryId = 4, Name = "ForthCategory"},
        };


        [Fact]
        public async Task CategoryRepositoryGetAllCategories_ShouldReturnZeroCategories_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.True(!context.Categories.Any());
            var categories = await repo.GetCategoriesAsync();
            Assert.True(categories.Count == 0);
        }

        [Fact]
        public async Task CategoryRepositoryGetAllCategories_ShouldReturnCategories_IfAny()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Categories.AddRangeAsync(_categories);
            await context.SaveChangesAsync();
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.True(context.Categories.Any());
            var categories = await repo.GetCategoriesAsync();
            Assert.True(categories.Any());
            Assert.True(categories.Count == _categories.Count);
            Assert.Equal(_categories, categories);
        }

        [Fact]
        public async Task CategoryRepositoryGetCategoryById_ShouldReturnNull_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.True(!context.Categories.Any());
            var category = await repo.GetCategoryByIdAsync(1);
            Assert.Null(category);
        }

        [Fact]
        public async Task CategoryRepositoryGetCategoryById_ShouldReturnCategory_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Categories.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.True(context.Categories.Any());
            var category = await repo.GetCategoryByIdAsync(_categories.ElementAt(0).CategoryId);
            Assert.NotNull(category);
            Assert.Equal(_categories.ElementAt(0), category);
        }

        [Fact]
        public async Task CategoryRepositorCreateCategory_ShouldCreateCategory_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);


            Assert.Null(await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categories.ElementAt(0).CategoryId));
            var category = await repo.CreateCategoryAsync(_categories.ElementAt(0));
            Assert.NotNull(category);
            Assert.Equal(_categories.ElementAt(0), category);
        }

        [Fact]
        public async Task CategoryRepositoryDeleteCategory_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);


            Assert.Null(await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categories.ElementAt(0).CategoryId));
           
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteCategoryAsync(_categories.ElementAt(0).CategoryId));
        }

        [Fact]
        public async Task CategoryRepositoryDeleteCategory_ShouldRemoveCategory_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Categories.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.NotNull(await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categories.ElementAt(0).CategoryId));
            await repo.DeleteCategoryAsync(_categories.ElementAt(0).CategoryId);
            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.DeleteCategoryAsync(_categories.ElementAt(0).CategoryId));
        }


        [Fact]
        public async Task CategoryRepositoryUpdateCategory_ShouldThrowException_IfNone()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            ICategoryRepository repo = new CategoryRepository(context);


            Assert.Null(await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categories.ElementAt(0).CategoryId));

            await Assert.ThrowsAsync<KeyNotFoundException>(() => repo.UpdateCategoryAsync(_categories.ElementAt(0)));
        }

        [Fact]
        public async Task CategoryRepositoryUpdateCategory_ShouldUpdateCategory_IfFound()
        {
            var options = new DbContextOptionsBuilder<LibraryContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new LibraryContext(options);
            await context.Categories.AddAsync(_categories.ElementAt(0));
            await context.SaveChangesAsync();

            var categoryToUpdate = new Category
            {
                Name = "UpdatedName",
                CategoryId = _categories.ElementAt(0).CategoryId
            };
            ICategoryRepository repo = new CategoryRepository(context);

            Assert.NotNull(await context.Categories.FirstOrDefaultAsync(c => c.CategoryId == _categories.ElementAt(0).CategoryId));

            var categoryUpdated = await repo.UpdateCategoryAsync(categoryToUpdate);
            Assert.NotSame(categoryToUpdate, categoryUpdated);
            Assert.Equal(categoryToUpdate.Name, categoryUpdated.Name);
        }
    }
}