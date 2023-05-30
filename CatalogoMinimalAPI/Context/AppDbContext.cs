using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinimalAPI.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Produto>? Produtos { get; set; }
    public DbSet<Categoria>? Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        //categoria
        mb.Entity<Categoria>().HasKey(c => c.CategoriaId);
        mb.Entity<Categoria>().Property(c => c.Nome)
                                            .HasMaxLength(100)
                                            .IsRequired();
        mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(255).IsRequired();

        //Produto
        mb.Entity<Produto>().HasKey(c => c.ProdutoId);

    }
}
