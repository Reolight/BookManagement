namespace Application.Books.Dto;


// TODO: ISBN parser. There are hyphens for best reading. Should be deleted before requesting smth from db...
public class BookBorrowDto
{
    public string BorrowedDate { get; set; }
    public string ReturnDate { get; set; }
}