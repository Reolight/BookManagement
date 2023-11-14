using Application.Books.Dto;
using Application.Services.Abstractions;
using Core.Entities;
using Infrastructure.Repositories;
using Mapster;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _repository;

    public BookService(IRepository<Book> repository) => _repository = repository;

    public List<BookDto> GetAllBooks()
    {
        var all = _repository.FindAll();
        var converted = all.ProjectToType<BookDto>();
        return converted.ToList();
    }

    public List<BookDto> GetBorrowedBooks()
        => _repository
            .FindByCondition(book => book.DateBorrowed != null)
            .ProjectToType<BookDto>()
            .ToList();

    public List<BookDto> GetOwedBooks()
    {
        var currentDate = DateOnly.FromDateTime(DateTime.Now);
        return _repository
            .FindByCondition(book => currentDate >= book.ReturningDate)
            .ProjectToType<BookDto>()
            .ToList();
    }

    public BookDto? GetById(int id)
        => _repository
            .FindByCondition(book => book.Id == id)
            .ProjectToType<BookDto>()
            .FirstOrDefault();

    public List<BookDto> GetByIsbn(string isbn)
        => _repository
            .FindByCondition(book => book.Isbn == isbn)
            .ProjectToType<BookDto>()
            .ToList();

    public BookDto AddBook(BookCreationDto bookDto)
        => _repository
            .Create(bookDto.Adapt<Book>())
            .Adapt<BookDto>();

    public bool BorrowBook(int id, BookBorrowDto borrowDto)
    {
        if (!DateOnly.TryParse(borrowDto.BorrowedDate, out DateOnly borrowDate) ||
                !DateOnly.TryParse(borrowDto.ReturnDate, out DateOnly returnDate) ||
                borrowDate > returnDate ||
                _repository.FindByCondition(book => book.Id == id && book.DateBorrowed == null)
                .FirstOrDefault() is not { } borrowedBook)
            return false;
        
        _repository.Update(borrowDto.Adapt(borrowedBook));
        return true;
    }

    public bool ReturnBook(int id)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not { DateBorrowed: not null } returnedBook)
            return false;
        (returnedBook.DateBorrowed, returnedBook.ReturningDate) = (null, null);
        _repository.Update(returnedBook);
        return true;
    }

    public void UpdateBook(int id, BookUpdateDto bookUpdateDto)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not {} bookUpdated)
            return;
        var updatedBook = bookUpdateDto.Adapt(bookUpdated);
        _repository.Update(updatedBook);
    }

    public void RemoveBook(int id)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not {} bookToRemove) 
            return;
        _repository.Delete(bookToRemove);
    }
}