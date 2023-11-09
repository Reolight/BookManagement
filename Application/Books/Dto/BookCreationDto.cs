﻿namespace Application.Books.Dto;

public class BookCreationDto
{
    public string Isbn { get; set; } = string.Empty; 
    public string Name { get; set; } = string.Empty;
    public string Genre { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
}