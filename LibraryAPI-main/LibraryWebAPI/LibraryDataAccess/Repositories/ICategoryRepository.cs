using LibraryDataAccess.Models;

namespace LibraryDataAccess.Repositories
{
    public interface ICategoryRepository
    {
        Task<List<Category>> GetCategoriesAsync();

        Task<Category?> GetCategoryByIdAsync(int id);

        Task<Category> CreateCategoryAsync(Category category);

        Task<Category> UpdateCategoryAsync(Category category);

        Task DeleteCategoryAsync(int id);
    }
}
