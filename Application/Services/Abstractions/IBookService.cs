using Application.Books.Dto;
using Core.Entities;

namespace Application.Services.Abstractions;

public interface IBookService
{
    public List<BookDto> GetAllBooks();
    public List<BookDto> GetBorrowedBooks();
    public List<BookDto> GetOwedBooks();
    public BookDto? GetById(int id);
    public BookDto? GetByIsbn(string isbn);
    public BookDto? AddBook(BookCreationDto bookDto);
    public void UpdateBook(int id, BookUpdateDto bookUpdateDto);
    public bool BorrowBook(int id, BookBorrowDto borrowDto);
    public bool ReturnBook(int id);
    public void RemoveBook(int id);
}