using LibraryManager.Core.Models;

namespace LibraryManager.Core.Interfaces;

public interface IBookRepository
{
    Task<IEnumerable<Book>> GetAllAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId);
    Task<Book> AddAsync(Book book);
    Task<Book?> UpdateAsync(Book book);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> IsISBNUniqueAsync(string isbn, int? excludeId = null);
}