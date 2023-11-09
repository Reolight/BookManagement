﻿using Application.Books.Dto;
using Application.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers;

[ApiController, Authorize, Route("[controller]")]
public class BookController : ControllerBase
{
    private readonly IBookService _bookService;

    public BookController(IBookService bookService) => _bookService = bookService;

    [HttpGet, ProducesResponseType(StatusCodes.Status200OK)]
    public IActionResult GetBooks()
        => Ok(_bookService.GetAllBooks());

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBookById(int id)
        => _bookService.GetById(id) is not { } book
            ? NotFound()
            : Ok(book);

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK),
     ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult GetBookByIsbn([FromQuery] string isbn)
        => _bookService.GetByIsbn(isbn) is not { } book
            ? NotFound()
            : Ok(book);

    [HttpDelete, ProducesResponseType(StatusCodes.Status204NoContent)]
    public IActionResult DeleteBook(int id)
    {
        _bookService.RemoveBook(id);
        return NoContent();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult AddBook(BookCreationDto newBook)
    {
        if (_bookService.AddBook(newBook) is not { } newBookDto)
            return BadRequest("Book was not created");
        return Created($"/book/{newBookDto.Id}", newBookDto);
    }

    [HttpPatch("{id}")]
    [ProducesResponseType(StatusCodes.Status202Accepted),
     ProducesResponseType(StatusCodes.Status400BadRequest)]
    public IActionResult UpdateBook(int id, [FromBody] BookUpdateDto updateDto)
    {
        _bookService.UpdateBook(id, updateDto);
        return Accepted();
    }
}