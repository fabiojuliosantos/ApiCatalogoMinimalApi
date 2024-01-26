using ApiCatalogo.Context;
using ApiCatalogo.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiCatalogo.ApiEndpoints;

public static class CategoriasEndpoints
{
    public static void MapCategoriasEndpoints(this WebApplication app)
    {

        #region Get

        app.MapGet("/", () => "Catálogo de produtos - 2024").WithTags("Categorias");

        app.MapGet("/categorias", async (AppDbContext db) => await db.Categorias.ToListAsync()).WithTags("Categorias");

        app.MapGet("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            return await db.Categorias.FindAsync(id) is Categoria categoria ? Results.Ok(categoria) : Results.NotFound("Categoria não encontrada!");
        }).WithTags("Categorias");

        #endregion Get

        #region Post

        app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
        {
            db.Categorias.Add(categoria);
            await db.SaveChangesAsync();

            return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
        }).WithTags("Categorias").RequireAuthorization();

        #endregion Post

        #region Update

        app.MapPut("/categorias/{id:int}", async (int id, Categoria categoria, AppDbContext db) =>
        {
            if (categoria.CategoriaId != id) return Results.BadRequest();

            var categoriaDB = await db.Categorias.FindAsync(id);

            if (categoriaDB is null) return Results.NotFound();

            categoriaDB.Nome = categoria.Nome;
            categoriaDB.Descricao = categoria.Descricao;

            await db.SaveChangesAsync();
            return Results.Ok(categoriaDB);
        }).WithTags("Categorias").RequireAuthorization();

        #endregion Update

        #region Delete

        app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
        {
            var categoriaDB = await db.Categorias.FindAsync(id);

            if (categoriaDB is null) return Results.NotFound();

            db.Categorias.Remove(categoriaDB);
            await db.SaveChangesAsync();

            return Results.NoContent();
        }).WithTags("Categorias").RequireAuthorization();

        #endregion Delete

    }
}
