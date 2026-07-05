using System.Net;
using System.Net.Http.Json;
using LibraryManager.Core.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace LibraryManager.IntegrationTests.Controllers;

public class BooksControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public BooksControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetAllBooks_ReturnsSuccessAndBooks()
    {
        // Act
        var response = await _client.GetAsync("/api/books");

        // Assert
        response.EnsureSuccessStatusCode();
        var books = await response.Content.ReadFromJsonAsync<List<Book>>();
        Assert.NotNull(books);
        Assert.NotEmpty(books); // Because we seeded data
    }

    [Fact]
    public async Task GetBookById_ExistingId_ReturnsBook()
    {
        // Act
        var response = await _client.GetAsync("/api/books/1");

        // Assert
        response.EnsureSuccessStatusCode();
        var book = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(book);
        Assert.Equal(1, book.Id);
    }

    [Fact]
    public async Task GetBookById_NonExistingId_ReturnsNotFound()
    {
        // Act
        var response = await _client.GetAsync("/api/books/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateBook_ValidBook_ReturnsCreated()
    {
        // Arrange
        var newBook = new Book
        {
            Title = "Integration Test Book",
            Author = "Test Author",
            ISBN = "INT-TEST-001",
            PublicationYear = 2024,
            CategoryId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", newBook);

        // Assert
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var createdBook = await response.Content.ReadFromJsonAsync<Book>();
        Assert.NotNull(createdBook);
        Assert.NotEqual(0, createdBook.Id);
    }

    [Fact]
    public async Task CreateBook_InvalidBook_ReturnsBadRequest()
    {
        // Arrange
        var invalidBook = new Book
        {
            Title = "", // Invalid - empty title
            ISBN = "", // Invalid - empty ISBN
            PublicationYear = 2024,
            CategoryId = 1
        };

        // Act
        var response = await _client.PostAsJsonAsync("/api/books", invalidBook);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_ExistingBook_ReturnsNoContent()
    {
        // Act
        var response = await _client.DeleteAsync("/api/books/2");

        // Assert
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
    }

    [Fact]
    public async Task DeleteBook_NonExistingBook_ReturnsNotFound()
    {
        // Act
        var response = await _client.DeleteAsync("/api/books/9999");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}