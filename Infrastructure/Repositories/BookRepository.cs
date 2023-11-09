using System.Linq.Expressions;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public abstract class BookRepository : IRepository<Book>
{
    private readonly DbContext _context;

    protected BookRepository(DbContext context) => _context = context;

    public Book Create(Book item) 
        => _context.Set<Book>().Add(item).Entity;

    public void Update(Book item)
    {
        _context.Set<Book>().Update(item);
    }

    public void Delete(Book item)
    {
        _context.Set<Book>().Remove(item);
    }

    public IQueryable<Book> FindAll()
        => _context.Set<Book>().AsNoTracking();

    public IQueryable<Book> FindByCondition(Expression<Func<Book, bool>> expression)
        => _context.Set<Book>().Where(expression).AsNoTracking();
}