using LibraryManager.Core.Data;
using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Models;
using LibraryManager.Core.Services;
using Moq;

namespace LibraryManager.UnitTests.Services;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _mockBookRepo;
    private readonly Mock<ICategoryRepository> _mockCategoryRepo;
    private readonly BookService _bookService;

    public BookServiceTests()
    {
        _mockBookRepo = new Mock<IBookRepository>();
        _mockCategoryRepo = new Mock<ICategoryRepository>();
        _bookService = new BookService(_mockBookRepo.Object, _mockCategoryRepo.Object);
    }

    [Fact]
    public async Task AddBook_ValidBook_ReturnsBookWithId()
    {
        // Arrange
        var book = new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            CategoryId = 1
        };

        _mockBookRepo.Setup(r => r.IsISBNUniqueAsync(book.ISBN, null)).ReturnsAsync(true);
        _mockCategoryRepo.Setup(r => r.ExistsAsync(book.CategoryId)).ReturnsAsync(true);
        _mockBookRepo.Setup(r => r.AddAsync(It.IsAny<Book>())).ReturnsAsync((Book b) => { b.Id = 1; return b; });

        // Act
        var result = await _bookService.AddBookAsync(book);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal("Clean Code", result.Title);
    }

    [Fact]
    public async Task AddBook_DuplicateISBN_ThrowsArgumentException()
    {
        // Arrange
        var book = new Book
        {
            Title = "Test",
            ISBN = "123",
            PublicationYear = 2020,  // ADD THIS - a valid year
            CategoryId = 1
        };
        _mockBookRepo.Setup(r => r.IsISBNUniqueAsync("123", null)).ReturnsAsync(false);

        // This also needs to pass the category check
        _mockCategoryRepo.Setup(r => r.ExistsAsync(1)).ReturnsAsync(true);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ArgumentException>(() => _bookService.AddBookAsync(book));
        Assert.Contains("ISBN", exception.Message);
    }

    [Fact]
    public async Task AddBook_InvalidYear_ThrowsArgumentException()
    {
        // Arrange
        var book = new Book { Title = "Test", ISBN = "123", PublicationYear = 1000, CategoryId = 1 };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _bookService.AddBookAsync(book));
    }

    [Fact]
    public async Task AddBook_NonExistentCategory_ThrowsArgumentException()
    {
        // Arrange
        var book = new Book { Title = "Test", ISBN = "123", PublicationYear = 2020, CategoryId = 99 };
        _mockBookRepo.Setup(r => r.IsISBNUniqueAsync(book.ISBN, null)).ReturnsAsync(true);
        _mockCategoryRepo.Setup(r => r.ExistsAsync(99)).ReturnsAsync(false);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _bookService.AddBookAsync(book));
    }

    [Fact]
    public async Task GetBookById_ExistingBook_ReturnsBook()
    {
        // Arrange
        var book = new Book { Id = 1, Title = "Test Book" };
        _mockBookRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(book);

        // Act
        var result = await _bookService.GetBookByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Book", result.Title);
    }

    [Fact]
    public async Task DeleteBook_ExistingBook_ReturnsTrue()
    {
        // Arrange
        _mockBookRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

        // Act
        var result = await _bookService.DeleteBookAsync(1);

        // Assert
        Assert.True(result);
    }
}