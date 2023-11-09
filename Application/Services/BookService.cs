using Application.Books.Dto;
using Application.Services.Abstractions;
using AutoMapper;
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

    public BookDto? FindById(int id)
        => _repository
            .FindByCondition(book => book.Id == id)
            .ProjectToType<BookDto>()
            .FirstOrDefault();

    public BookDto? FindByIsbn(string isbn) 
        => _repository
            .FindByCondition(book => book.Isbn == isbn)
            .ProjectToType<BookDto>()
            .FirstOrDefault();

    public BookDto? AddBook(BookDto bookDto)
        => _repository
            .Create(bookDto.Adapt<Book>())
            .Adapt<BookDto>();

    public void UpdateBook(BookDto bookDto)
    {
        _repository.Update(bookDto.Adapt<Book>());
    }

    public void RemoveBook(BookDto bookDto)
    {
        _repository.Delete(bookDto.Adapt<Book>());
    }
}