using Application.Books.Dto;

namespace Application.Services.Abstractions;

public interface IBookService
{
    public IEnumerable<BookDto> GetAllBooks();
    public IEnumerable<BookDto> GetAllBooksPaginated(int page, int countOnPage);
    public BookDto FindById(int id);
    public BookDto FindByIsbn(string isbn);
    public BookDto? AddBook(BookDto bookDto);
    public void UpdateBook(BookDto bookDto);
    public void RemoveBook(BookDto bookDto);
}