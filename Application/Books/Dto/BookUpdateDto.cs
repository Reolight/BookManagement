using System.ComponentModel.DataAnnotations;

namespace Application.Books.Dto;

public class BookUpdateDto
{
    [MaxLength(128)]
    public string? Name { get; set; }
    [MaxLength(64)]
    public string? Genre { get; set; }
    public string? Description { get; set; }
    [MaxLength(64)]
    public string? Author { get; set; }
}