using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using MongoChangeStream.DDD.Application.Common.Interfaces;
using MongoChangeStream.DDD.Application.Handlers;
using MongoChangeStream.DDD.Domain.Entities;
using MongoChangeStream.DDD.Infrastructure.DependencyInjection;
using MongoChangeStream.DDD.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.RegisterChangeStreamHandler<Product, ProductChangeStreamHandler>("Products");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/api/products", async ([FromServices] MongoDbContext context, [FromBody] Product product) =>
{
    product.CreatedAt = DateTime.UtcNow;
    var collection = context.GetCollection<Product>("Products");
    await collection.InsertOneAsync(product);
    return Results.Ok(product);
})
.WithName("CreateProduct");

app.MapPut("/api/products/{id}", async ([FromServices] MongoDbContext context, Guid id, [FromBody] Product product) =>
{
    var collection = context.GetCollection<Product>("Products");
    product.UpdatedAt = DateTime.UtcNow;
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
    var result = await collection.ReplaceOneAsync(filter, product);
    return result.ModifiedCount > 0 ? Results.Ok(product) : Results.NotFound();
})
.WithName("UpdateProduct")
;

app.MapDelete("/api/products/{id}", async ([FromServices] MongoDbContext context, Guid id) =>
{
    var collection = context.GetCollection<Product>("Products");
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
    var result = await collection.DeleteOneAsync(filter);
    return result.DeletedCount > 0 ? Results.Ok() : Results.NotFound();
})
.WithName("DeleteProduct")
;

app.MapGet("/api/products", async ([FromServices] MongoDbContext context) =>
{
    var collection = context.GetCollection<Product>("Products");
    var products = await collection.Find(_ => true).ToListAsync();
    return Results.Ok(products);
})
.WithName("GetAllProducts")
;

app.MapGet("/api/products/{id}", async ([FromServices] MongoDbContext context, Guid id) =>
{
    var collection = context.GetCollection<Product>("Products");
    var filter = Builders<Product>.Filter.Eq(p => p.Id, id);
    var product = await collection.Find(filter).FirstOrDefaultAsync();
    return product is not null ? Results.Ok(product) : Results.NotFound();
})
.WithName("GetProductById")
;

app.Run();
