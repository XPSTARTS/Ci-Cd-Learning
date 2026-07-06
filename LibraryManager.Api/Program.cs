using LibraryManager.Core.Data;
using LibraryManager.Core.Interfaces;
using LibraryManager.Core.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register repositories (singleton for in-memory data)
builder.Services.AddSingleton<IBookRepository, InMemoryBookRepository>();
builder.Services.AddSingleton<ICategoryRepository, InMemoryCategoryRepository>();
builder.Services.AddSingleton<IBookService, BookService>();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Seed the database
using (var scope = app.Services.CreateScope())
{
    var bookRepo = scope.ServiceProvider.GetRequiredService<IBookRepository>();
    var categoryRepo = scope.ServiceProvider.GetRequiredService<ICategoryRepository>();
    await DataSeeder.SeedAsync(bookRepo, categoryRepo);
}

app.Run();

// Make Program accessible to integration tests
public partial class Program { }