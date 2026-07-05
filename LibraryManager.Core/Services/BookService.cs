using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Models;

namespace LibraryManager.Core.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly ICategoryRepository _categoryRepository;

    public BookService(IBookRepository bookRepository, ICategoryRepository categoryRepository)
    {
        _bookRepository = bookRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<IEnumerable<Book>> GetAllBooksAsync()
    {
        return await _bookRepository.GetAllAsync();
    }

    public async Task<Book?> GetBookByIdAsync(int id)
    {
        return await _bookRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Book>> GetBooksByCategoryAsync(int categoryId)
    {
        if (!await _categoryRepository.ExistsAsync(categoryId))
            throw new ArgumentException($"Category with ID {categoryId} not found");

        return await _bookRepository.GetByCategoryAsync(categoryId);
    }

    public async Task<Book> AddBookAsync(Book book)
    {
        // Business rules
        if (string.IsNullOrWhiteSpace(book.Title))
            throw new ArgumentException("Book title is required");

        if (string.IsNullOrWhiteSpace(book.ISBN))
            throw new ArgumentException("ISBN is required");

        if (book.PublicationYear < 2000 || book.PublicationYear > DateTime.Now.Year)
            throw new ArgumentException($"Publication year must be between 2000 and {DateTime.Now.Year}");

        if (!await _bookRepository.IsISBNUniqueAsync(book.ISBN))
            throw new ArgumentException($"A book with ISBN {book.ISBN} already exists");

        if (!await _categoryRepository.ExistsAsync(book.CategoryId))
            throw new ArgumentException($"Category with ID {book.CategoryId} not found");

        return await _bookRepository.AddAsync(book);
    }

    public async Task<Book?> UpdateBookAsync(int id, Book book)
    {
        if (!await _bookRepository.ExistsAsync(id))
            return null;

        if (!await _bookRepository.IsISBNUniqueAsync(book.ISBN, id))
            throw new ArgumentException($"A book with ISBN {book.ISBN} already exists");

        if (!await _categoryRepository.ExistsAsync(book.CategoryId))
            throw new ArgumentException($"Category with ID {book.CategoryId} not found");

        book.Id = id;
        return await _bookRepository.UpdateAsync(book);
    }

    public async Task<bool> DeleteBookAsync(int id)
    {
        return await _bookRepository.DeleteAsync(id);
    }
}