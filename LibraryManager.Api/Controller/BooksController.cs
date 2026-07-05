using LibraryManager.Core.Models;
using LibraryManager.Core.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private readonly IBookService _bookService;

    public BooksController(IBookService bookService)
    {
        _bookService = bookService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetAll()
    {
        var books = await _bookService.GetAllBooksAsync();
        return Ok(books);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetById(int id)
    {
        var book = await _bookService.GetBookByIdAsync(id);
        if (book == null)
            return NotFound();
        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> Create(Book book)
    {
        try
        {
            var createdBook = await _bookService.AddBookAsync(book);
            return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Book>> Update(int id, Book book)
    {
        var updatedBook = await _bookService.UpdateBookAsync(id, book);
        if (updatedBook == null)
            return NotFound();
        return Ok(updatedBook);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _bookService.DeleteBookAsync(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }

    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetByCategory(int categoryId)
    {
        try
        {
            var books = await _bookService.GetBooksByCategoryAsync(categoryId);
            return Ok(books);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}