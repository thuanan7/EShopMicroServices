using Marten.Internal;

namespace Catalog.API.Products.GetProductsByCatagory
{
    public record GetProductsByCatagoryResponse(IEnumerable<Product> Products);
    public class GetProductsByCatagoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/catagory/{catagory}", async (ISender sender, string catagory) =>
            {
                var result = await sender.Send(new GetProductsByCatagoryQuery(catagory));

                var response = result.Adapt<GetProductsByCatagoryResponse>();

                return Results.Ok(response);
            })
               .WithName("GetProductsByCatagory")
               .Produces<GetProductsByCatagoryResponse>(StatusCodes.Status200OK)
               .ProducesProblem(StatusCodes.Status400BadRequest)
               .WithSummary("Get Products By Catagory")
               .WithDescription("Get Products By Catagory");
        }
    }
}
