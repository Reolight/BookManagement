using Application.Books.Dto;
using Core.Entities;

namespace Application.Services.Abstractions;

public interface IBookService
{
    public List<BookDto> GetAllBooks();
    public BookDto? FindById(int id);
    public BookDto? FindByIsbn(string isbn);
    public BookDto? AddBook(BookDto bookDto);
    public void UpdateBook(BookDto bookDto);
    public void RemoveBook(BookDto bookDto);
}