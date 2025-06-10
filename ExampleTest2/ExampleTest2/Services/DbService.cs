using ExampleTest2.Data;
using ExampleTest2.Models;
using Microsoft.EntityFrameworkCore;

namespace ExampleTest2.Services;

public class DbService : IDbService
{
    private readonly DatabaseContext _context;
    public DbService(DatabaseContext context)
    {
        _context = context;
    }
    
}