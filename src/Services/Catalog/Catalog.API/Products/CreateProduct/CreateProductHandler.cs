namespace Catalog.API.Products.CreateProduct
{
    public record CreateProductCommand(string Name, List<string> Catagory, string Description, string ImageFile, decimal Price)
        : ICommand<CreateProductResult>;

    public record CreateProductResult(Guid ProductId);

    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(x => x.Catagory).NotEmpty().WithMessage("Category is required");
            RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    internal class CreateProductCommandHandler
        (IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            // create product entity from command object
            var product = new Product
            {
                Name = command.Name,
                Catagory = command.Catagory,
                Description = command.Description,
                ImageFile = command.ImageFile,
                Price = command.Price
            };

            // TODO: save product entity to database
            session.Store(product);
            await session.SaveChangesAsync(cancellationToken);

            // return CreateProductResult with product id
            return new CreateProductResult(product.Id);
        }
    }
}
