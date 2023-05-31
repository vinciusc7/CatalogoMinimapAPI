using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinimalAPI.ApiEndPoints
{
    public static class CategoriasEndPoints
    {
        public static void MapCategoriasEndPoints(this WebApplication app)
        {
            app.MapPost("/categorias", async (Categoria categoria, AppDbContext db) =>
            {
                db.Categorias.Add(categoria);
                await db.SaveChangesAsync();
                return Results.Created($"/categorias/{categoria.CategoriaId}", categoria);
            });

            app.MapGet("/categorias", async (AppDbContext db) =>
            {
                IEnumerable<Categoria> categorias = await db.Categorias.ToListAsync();
                return Results.Ok(categorias);
            });

            app.MapGet("/categorias/{id:int}", async (AppDbContext db, int id) =>
            {
                return await db.Categorias.FindAsync(id)
                                            is Categoria categoria
                                            ? Results.Ok(categoria)
                                            : Results.NotFound("Categoria não encontrada!");
            });

            app.MapPut("/categrias/{id:int}", async (int id, AppDbContext db, Categoria categoria) =>
            {
                if (categoria.CategoriaId != id)
                {
                    return Results.BadRequest();
                }

                var categoriaDB = await db.Categorias.FindAsync(id);
                if (categoriaDB == null)
                {
                    return Results.NotFound("Categoria não encontrada!");
                }
                categoriaDB.Nome = categoria.Nome;
                categoriaDB.Descricao = categoria.Descricao;
                await db.SaveChangesAsync();
                return Results.Ok(categoriaDB);
            });

            app.MapDelete("/categorias/{id:int}", async (int id, AppDbContext db) =>
            {
                var categoria = await db.Categorias.FindAsync(id);
                if (categoria == null)
                {
                    return Results.NotFound("Categoria não encontrada");
                }

                db.Categorias.Remove(categoria);
                db.SaveChangesAsync();
                return Results.Ok("Categoria deleteda com sucesso!");
            });

        }
    }
}
