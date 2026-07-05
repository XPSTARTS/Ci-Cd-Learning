using LibraryManager.Core.Models;

namespace LibraryManager.Core.Services;

public interface IBookService
{
    Task<IEnumerable<Book>> GetAllBooksAsync();
    Task<Book?> GetBookByIdAsync(int id);
    Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId);
    Task<Book> AddBookAsync(Book book);
    Task<Book?> UpdateBookAsync(int id, Book book);
    Task<bool> DeleteBookAsync(int id);
}