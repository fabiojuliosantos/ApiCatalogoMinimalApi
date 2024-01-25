using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.Context;

public class AppDbContext : DbContext 
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    public DbSet<Produto> ? Produtos {  get; set; }
    public DbSet<Categoria> ? Categorias { get; set; }

    protected override void OnModelCreating(ModelBuilder mb) 
    {
        #region Categoria
        
        mb.Entity<Categoria>().HasKey(c => c.CategoriaId); //=> CategoriaId é a chave primária

        mb.Entity<Categoria>().Property(c => c.Nome).HasMaxLength(100).IsRequired(); //=> Propriedade nome tem tamanho de 100 e não pode ser nula.

        mb.Entity<Categoria>().Property(c => c.Descricao).HasMaxLength(150).IsRequired();

        #endregion Categoria

        #region Produto

        mb.Entity<Produto>().HasKey(c => c.ProdutoId);

        mb.Entity<Produto>().Property(c=> c.Nome).HasMaxLength(100).IsRequired(true);

        mb.Entity<Produto>().Property(c => c.Descricao).HasMaxLength(150);

        mb.Entity<Produto>().Property(c => c.Imagem).HasMaxLength(100);

        mb.Entity<Produto>().Property(c => c.Preco).HasPrecision(14, 2);

        #endregion Produto

        #region Relacionamento entre tabelas

        mb.Entity<Produto>().HasOne<Categoria>(c => c.Categoria)
                            .WithMany(p => p.Produtos)
                            .HasForeignKey(c => c.CategoriaId);

        #endregion Relacionamento entre tabelas
    }
}
