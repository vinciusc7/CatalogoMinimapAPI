using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogoMinimalAPI.ApiEndPoints;

public static class ProdutosEndPoints
{
    public static void MapProdutosEndPoints(this WebApplication app)
    {

        app.MapPost("/produtos", async (Produto produto, AppDbContext db) =>
        {
            db.Produtos.Add(produto);
            db.SaveChangesAsync();
            return Results.Created($"/produtos/{produto.ProdutoId}", produto);
        });

        app.MapGet("/produtos", async (AppDbContext db) =>
        {
            IEnumerable<Produto> produtos = await db.Produtos.ToListAsync();
            return Results.Ok(produtos);
        });
        app.MapGet("/produtos/{id:int}", async (AppDbContext db, int id) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            if (produto is null)
            {
                return Results.NotFound("Nenhum produto encontrado");
            }
            return Results.Ok(produto); ;
        });
        app.MapPut("/produtos/{id:int}", async (AppDbContext db, Produto produto, int id) =>
        {
            if (produto.ProdutoId != id)
            {
                return Results.BadRequest();
            }
            var produtoDB = await db.Produtos.FindAsync(id);
            if (produtoDB is null)
            {
                return Results.NotFound("Produto não encontrado!");
            }

            produtoDB.Nome = produto.Nome;
            produtoDB.Descricao = produto.Descricao;
            produtoDB.Preco = produto.Preco;
            produtoDB.ImagemUrl = produto.ImagemUrl;
            produtoDB.DataCompra = produto.DataCompra;
            produtoDB.Estoque = produto.Estoque;
            produtoDB.CategoriaId = produto.CategoriaId;
            db.SaveChangesAsync();

            return Results.Ok(produtoDB);

        });

        app.MapDelete("/produtos/{id:int}", async (int id, AppDbContext db) =>
        {
            var produto = await db.Produtos.FindAsync(id);
            if (produto is null)
            {
                return Results.NotFound("Nenhum produto encontrado!");
            }
            db.Produtos.Remove(produto);
            db.SaveChangesAsync();
            return Results.Ok();
        });
    }
}
