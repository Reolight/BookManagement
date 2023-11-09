using Application.Books.Dto;
using Application.Services.Abstractions;

namespace Application.Services;

public class BookService : IBookService
{
    public IEnumerable<BookDto> GetAllBooks()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<BookDto> GetAllBooksPaginated(int page, int countOnPage)
    {
        throw new NotImplementedException();
    }

    public BookDto FindById(int id)
    {
        throw new NotImplementedException();
    }

    public BookDto FindByIsbn(string isbn)
    {
        throw new NotImplementedException();
    }

    public BookDto? AddBook(BookDto bookDto)
    {
        throw new NotImplementedException();
    }

    public void UpdateBook(BookDto bookDto)
    {
        throw new NotImplementedException();
    }

    public void RemoveBook(BookDto bookDto)
    {
        throw new NotImplementedException();
    }
}