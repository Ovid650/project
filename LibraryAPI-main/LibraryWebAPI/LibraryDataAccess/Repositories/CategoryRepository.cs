using LibraryDataAccess.Data;
using LibraryDataAccess.Models;
using Microsoft.EntityFrameworkCore;
using SQLitePCL;
using System.Runtime.CompilerServices;

namespace LibraryDataAccess.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LibraryContext _context;

        public CategoryRepository(LibraryContext context)
        {
            _context = context;
        }
        public async Task<Category> CreateCategoryAsync(Category category)
        {
            var categoryAdded = await _context.Categories.AddAsync(category);
            await _context.SaveChangesAsync();
            return categoryAdded.Entity;
        }

        public async Task DeleteCategoryAsync(int id)
        {
            var categoryToDelete = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
            if(categoryToDelete == null)
            {
                throw new KeyNotFoundException();
            }

            _context.Remove(categoryToDelete);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _context.Categories.ToListAsync();
            return categories;
        }

        public async Task<Category?> GetCategoryByIdAsync(int id)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == id);
        }

        public async Task<Category> UpdateCategoryAsync(Category category)
        {
            var categoryToUpdate = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == category.CategoryId);
            if (categoryToUpdate == null)
            {
                throw new KeyNotFoundException();
            }

            categoryToUpdate.Name = category.Name;
            var categoryUpdated = _context.Update(categoryToUpdate);
            await _context.SaveChangesAsync();
            return categoryUpdated.Entity;
        }
    }
}
