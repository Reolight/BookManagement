using System.ComponentModel.DataAnnotations;

namespace Application.Books.Dto;

public class BookCreationDto
{
    [MinLength(17), MaxLength(17)]
    public string Isbn { get; set; } = string.Empty;
    [MaxLength(128)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(64)]
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    [MaxLength(64)]
    public string Author { get; set; } = string.Empty;
}