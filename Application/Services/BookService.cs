using Application.Books.Dto;
using Application.Services.Abstractions;
using Core.Entities;
using Infrastructure.Repositories;
using Mapster;

namespace Application.Services;

public class BookService : IBookService
{
    private readonly IRepository<Book> _repository;

    public BookService(IRepository<Book> repository) => 
        _repository = repository;

    public List<BookDto> GetAllBooks() 
        => _repository.FindAll()
            .ProjectToType<BookDto>()
            .ToList();

    public List<BookDto> GetBorrowedBooks()
        => _repository
            .FindByCondition(book => book.DateBorrowed != null)
            .ProjectToType<BookDto>()
            .ToList();

    public List<BookDto> GetOwedBooks()
        => _repository
            .FindByCondition(book => DateOnly.FromDateTime(DateTime.Now) >= book.ReturningDate)
            .ProjectToType<BookDto>()
            .ToList();

    public BookDto? GetById(int id)
        => _repository
            .FindByCondition(book => book.Id == id)
            .ProjectToType<BookDto>()
            .FirstOrDefault();

    public BookDto? GetByIsbn(string isbn) 
        => _repository
            .FindByCondition(book => book.Isbn == isbn)
            .ProjectToType<BookDto>()
            .FirstOrDefault();

    public BookDto? AddBook(BookCreationDto bookDto)
        => _repository
            .Create(bookDto.Adapt<Book>())
            .Adapt<BookDto>();

    public bool BorrowBook(int id, BookBorrowDto borrowDto)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not { } borrowedBook || borrowDto.BorrowedDate <= borrowDto.ReturnDate)
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

    public void UpdateBook(int id, BookUpdateDto bookDto)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not {} bookToUpdate)
            return;
        _repository.Update(bookDto.Adapt(bookToUpdate));
    }

    public void RemoveBook(int id)
    {
        if (_repository.FindByCondition(book => book.Id == id)
                .FirstOrDefault() is not {} bookToRemove) 
            return;
        _repository.Delete(bookToRemove);
    }
}