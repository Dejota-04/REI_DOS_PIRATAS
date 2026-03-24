using Microsoft.EntityFrameworkCore;
using Sprint1.Models;

namespace Sprint1.Data
{
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext() { }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Produto> Produtos { get; set; }
    }
}