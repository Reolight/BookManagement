using Application.Books.Dto;
using Core.Entities;
using Mapster;

namespace Application.Books.MappingProfiles;

public class BookUpdateMapping : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<BookUpdateDto, Book>()
            .IgnoreNullValues(true);
    }
}