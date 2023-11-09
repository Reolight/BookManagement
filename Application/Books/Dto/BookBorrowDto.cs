namespace Application.Books.Dto;

public class BookBorrowDto
{
    public DateOnly BorrowedDate { get; set; }
    public DateOnly ReturnDate { get; set; }
}