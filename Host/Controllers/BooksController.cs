using System.Text;
using Application.Books.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController, Authorize, Route("[controller]")]
[ProducesResponseType(StatusCodes.Status401Unauthorized)]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService) => _bookService = bookService;

    /// <summary>
    /// Endpoint to get all existing books.
    /// </summary>
    /// <returns>List of all books</returns>
    [HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetBooks()
        => Ok(_bookService.GetAllBooks());

    /// <summary>
    /// Endpoint to get book by id.
    /// </summary>
    /// <param name="id">Id of the book</param>
    /// <returns>Information about the book if found</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBookById(int id)
        => _bookService.GetById(id) is not { } book
            ? NotFound()
            : Ok(book);

    /// <summary>
    /// Endpoint to get book by ISBN.
    /// </summary>
    /// <param name="isbn">The International Standard Book Number (ISBN) is a numeric commercial book identifier that is intended to be unique.
    /// In the current solution contains 13 digits and 4 delimiters (hyphens)</param>
    /// <returns>Books with matched ISBN or Not found</returns>
    /// <response code="400">ISBN is wrong (or this version contains 10 digits which is unsupported)</response>
    /// <response code="404">No books with ISBN provided found</response>
    [HttpGet("isbn/{isbn}")]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status404NotFound),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult GetBookByIsbn(string isbn)
    {
        if (isbn.Sum(c => char.IsDigit(c)? 0 : 1) != 4 && isbn.Length != 17)
            return BadRequest("ISBN provided is not correct");
        
        return _bookService.GetByIsbn(isbn) is not { Count: > 0 } books
            ? NotFound()
            : Ok(books);
    }

    /// <summary>
    /// Endpoint to get list of all borrowed books
    /// </summary>
    /// <returns>List of borrowed books</returns>
    [HttpGet("borrowed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetBorrowedBooks() 
        => Ok(_bookService.GetBorrowedBooks());

    /// <summary>
    /// Endpoint to get list of all owed books
    /// </summary>
    /// <returns>List of owed books</returns>
    [HttpGet("owed")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetOwedBooks()
        => Ok(_bookService.GetOwedBooks());
    
    /// <summary>
    /// Deletes book by id
    /// </summary>
    /// <param name="id">Id of book to delete</param>
    /// <returns>No content only</returns>
    [HttpDelete, ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteBook(int id)
    {
        _bookService.RemoveBook(id);
        return NoContent();
    }

    /// <summary>
    /// Endpoint to add a new book
    /// </summary>
    /// <param name="newBook">Model to specify all required information about the book. All fields are required.
    /// N.B.: if your book contains ISBN with 10 digits, you can convert it to new 13 digit ISBN</param>
    /// <returns>Newly created book in case of success</returns>
    /// <response code="400">Check provided information (maybe ISBN has wrong format)</response> 
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddBook(BookCreationDto newBook)
    {
        if (_bookService.AddBook(newBook) is not { } newBookDto)
            return BadRequest("Book was not created");
        return Created($"/book/{newBookDto.Id}", newBookDto);
    }

    /// <summary>
    /// Partially updates book by id
    /// </summary>
    /// <param name="id">Id of the updated book</param>
    /// <param name="updateDto">Model with optional fields. Not null fields will be updated, null fields will be ignored</param>
    /// <returns>Accepted, if successful</returns>
    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateBook(int id, [FromBody] BookUpdateDto updateDto)
    {
        _bookService.UpdateBook(id, updateDto);
        return Accepted();
    }

    /// <summary>
    /// Marks book as borrowed.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="borrowDto">Contains two strings with dates of borrow and return like 'DD.MM.YYYY'</param>
    /// <returns>Accepted, if successful</returns>
    /// <response code="404">Book is really does not exist, or it was already borrowed, or date of return is earlier than date of borrow</response>
    [HttpPatch("{id}/borrow")]
    [ProducesResponseType(StatusCodes.Status202Accepted),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult BorrowBook(int id, [FromBody] BookBorrowDto borrowDto) =>
        _bookService.BorrowBook(id, borrowDto)
            ? Accepted()
            : NotFound("There is no such book or it was already borrowed");

    /// <summary>
    /// Unmarks borrowed book by id only.
    /// </summary>
    /// <param name="id"></param>
    /// <response code="400">There is no such book or it is already returned</response>
    [HttpPatch("{id}/return")]
    [ProducesResponseType(StatusCodes.Status202Accepted),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult ReturnOwedBook(int id)
        => _bookService.ReturnBook(id)
            ? Accepted()
            : NotFound("There is no such book in the library or it was already returned");
}