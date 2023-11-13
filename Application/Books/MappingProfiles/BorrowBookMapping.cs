using Application.Books.Dto;
using Core.Entities;
using Mapster;

namespace Application.Books.MappingProfiles;

public class BorrowBookMapping  : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookBorrowDto, Book>()
            .Map(book => book.DateBorrowed, dto => DateOnly.Parse(dto.BorrowedDate))
            .Map(book => book.ReturningDate, dto => DateOnly.Parse(dto.ReturnDate));
    }
}