namespace Application.Books.Dto;

public class BookDto
{
    public string Isbn { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public DateOnly? DateBorrowed { get; set; }
    public DateOnly? DateUntilMustBeReturned { get; set; }
}