namespace LibraryManager.Core.Models;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty;
    public int PublicationYear { get; set; }
    public bool IsAvailable { get; set; } = true;
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
}