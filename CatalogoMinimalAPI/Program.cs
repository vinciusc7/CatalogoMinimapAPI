using CatalogoMinimalAPI.Context;
using CatalogoMinimalAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();
//
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
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.Run();

