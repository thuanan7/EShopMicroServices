﻿namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductRequest(string Name, List<string> Catagory, string Description, string ImageFile, decimal Price);
    public record CreateProductResponse(Guid ProductId);
    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender) =>
            {
                var command = request.Adapt<CreateProductCommand>();

                var result = await sender.Send(command);

                var response = result.Adapt<CreateProductResponse>();

                return Results.Created($"/products/{response.ProductId}", response);
            })
               .WithName("CreateProducts")
               .Produces<CreateProductResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Create Product")
               .WithDescription("Create Product");
        }
    }
}
