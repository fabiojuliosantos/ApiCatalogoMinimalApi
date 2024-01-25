using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString)
);

var app = builder.Build();

#region Endpoints Categorias

#region Get

app.MapGet("/", () => "Catálogo de produtos - 2024");

app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync());

app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrada!");
});

#endregion Get

#region Post

app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
{
    db.Categorias.Add(categoria);
    await db.SaveChangesAsync();

    return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
});

#endregion Post

#region Update

app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
{
    if(categoria.CategoriaId != id) return Results.BadRequest();

    var categoriaDB = await db.Categorias.FindAsync(id);

    if(categoriaDB is null) return Results.NotFound();

    categoriaDB.Nome = categoria.Nome;
    categoriaDB.Descricao = categoria.Descricao;

    await db.SaveChangesAsync();
    return Results.Ok(categoriaDB);
});

#endregion Update

#region Delete

app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
{
    var categoriaDB = await db.Categorias.FindAsync(id);

    if (categoriaDB is null) return Results.NotFound();

    db.Categorias.Remove(categoriaDB);
    await db.SaveChangesAsync();

    return Results.NoContent();
});

#endregion Delete

#endregion Endpoints Categorias

#region Endpoints Produtos

#region Get

app.MapGet("/produtos", async (AppDbContext db) => await db.Produtos.ToListAsync());

app.MapGet("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    return await db.Produtos.FindAsync(id) is Produto produto ? Results.Ok(produto) : Results.NotFound("Produto não encontrado!"); 
});

#endregion Get

#region Post

app.MapPost("/produtos", async (Produto produto, AppDbContext db) => 
{
    db.Produtos.Add(produto);
    await db.SaveChangesAsync();

    return Results.Created($"/produtos/{produto.ProdutoId}", produto);
});

#endregion Post

#region Update

app.MapPut("/produtos/{id:int}", async (int id, Produto produto, AppDbContext db) =>
{
    if (produto.ProdutoId != id) return Results.BadRequest();

    var produtoDB = await db.Produtos.FindAsync(id);

    if (produtoDB is null) return Results.NotFound();

    produtoDB.Nome = produto.Nome;
    produtoDB.Descricao = produto.Descricao;
    produtoDB.Preco = produto.Preco;
    produtoDB.Imagem = produto.Imagem;
    produtoDB.DataCompra = produto.DataCompra;
    produtoDB.Estoque = produto.Estoque;
    produtoDB.CategoriaId = produto.CategoriaId;

    await db.SaveChangesAsync();

    return Results.Ok(produtoDB);

});

#endregion Update

#region Delete

app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
{
    var ProdutoDB = await db.Produtos.FindAsync(id);

    if (ProdutoDB is null) return Results.NotFound();

    db.Produtos.Remove(ProdutoDB);
    await db.SaveChangesAsync();

    return Results.NoContent();
});


#endregion Delete

#endregion Endpoints Produtos

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();