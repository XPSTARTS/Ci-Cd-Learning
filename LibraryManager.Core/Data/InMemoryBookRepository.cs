using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Models;

namespace LibraryManager.Core.Data;

public class InMemoryBookRepository : IBookRepository
{
    private readonly List<Book> _books = new();
    private int _nextId = 1;

    public Task<IEnumerable<Book>> GetAllAsync()
    {
        return Task.FromResult(_books.AsEnumerable());
    }

    public Task<Book?> GetByIdAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        return Task.FromResult(book);
    }

    public Task<IEnumerable<Book>> GetByCategoryAsync(int categoryId)
    {
        var books = _books.Where(b => b.CategoryId == categoryId);
        return Task.FromResult(books);
    }

    public Task<Book> AddAsync(Book book)
    {
        book.Id = _nextId++;
        _books.Add(book);
        return Task.FromResult(book);
    }

    public Task<Book?> UpdateAsync(Book book)
    {
        var existing = _books.FirstOrDefault(b => b.Id == book.Id);
        if (existing == null) return Task.FromResult<Book?>(null);

        existing.Title = book.Title;
        existing.Author = book.Author;
        existing.ISBN = book.ISBN;
        existing.PublicationYear = book.PublicationYear;
        existing.IsAvailable = book.IsAvailable;
        existing.CategoryId = book.CategoryId;

        return Task.FromResult<Book?>(existing);
    }

    public Task<bool> DeleteAsync(int id)
    {
        var book = _books.FirstOrDefault(b => b.Id == id);
        if (book == null) return Task.FromResult(false);
        _books.Remove(book);
        return Task.FromResult(true);
    }

    public Task<bool> ExistsAsync(int id)
    {
        return Task.FromResult(_books.Any(b => b.Id == id));
    }

    public Task<bool> IsISBNUniqueAsync(string isbn, int? excludeId = null)
    {
        var exists = _books.Any(b =>
            b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase) &&
            (!excludeId.HasValue || b.Id != excludeId.Value));
        return Task.FromResult(!exists);
    }
}