using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Models;

namespace LibraryManager.Core.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(IBookRepository bookRepo, ICategoryRepository categoryRepo)
    {
        // Check if data already exists
        var categories = await categoryRepo.GetAllAsync();
        if (categories.Any()) return;

        // Add categories
        var fiction = await categoryRepo.AddAsync(new Category { Name = "Fiction", Description = "Fictional books" });
        var nonFiction = await categoryRepo.AddAsync(new Category { Name = "Non-Fiction", Description = "Non-fiction books" });
        var sciFi = await categoryRepo.AddAsync(new Category { Name = "Science Fiction", Description = "Sci-fi books" });

        // Add books
        await bookRepo.AddAsync(new Book
        {
            Title = "The Great Gatsby",
            Author = "F. Scott Fitzgerald",
            ISBN = "978-0743273565",
            PublicationYear = 1925,
            CategoryId = fiction.Id,
            IsAvailable = true
        });

        await bookRepo.AddAsync(new Book
        {
            Title = "Clean Code",
            Author = "Robert C. Martin",
            ISBN = "978-0132350884",
            PublicationYear = 2008,
            CategoryId = nonFiction.Id,
            IsAvailable = true
        });

        await bookRepo.AddAsync(new Book
        {
            Title = "Dune",
            Author = "Frank Herbert",
            ISBN = "978-0441172719",
            PublicationYear = 1965,
            CategoryId = sciFi.Id,
            IsAvailable = false
        });
    }
}