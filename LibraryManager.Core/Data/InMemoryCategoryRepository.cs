using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Models;

namespace LibraryManager.Core.Data;

public class InMemoryCategoryRepository : ICategoryRepository
{
    private readonly List<Category> _categories = new();
    private int _nextId = 1;

    public Task<IEnumerable<Category>> GetAllAsync()
    {
        return Task.FromResult(_categories.AsEnumerable());
    }

    public Task<Category?> GetByIdAsync(int id)
    {
        var category = _categories.FirstOrDefault(c => c.Id == id);
        return Task.FromResult(category);
    }

    public Task<Category> AddAsync(Category category)
    {
        category.Id = _nextId++;
        _categories.Add(category);
        return Task.FromResult(category);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_categories.Any(c => c.Id == id));
    }
}