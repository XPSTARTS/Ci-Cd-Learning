using LibraryManager.Core.Models;

namespace LibraryManager.Core.Interfaces;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> AddAsync(Category category);
    Task<bool> ExistsAsync(int id);
}