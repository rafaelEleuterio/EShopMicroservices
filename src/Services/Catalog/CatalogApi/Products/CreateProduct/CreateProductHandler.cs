namespace CatalogApi.Products.CreateProduct;

public record CreateProductCommand(string Name,
    List<string> Category,
    string Description,
    string ImageFile,
    decimal Price)
        : ICommand<CreateProductResult>;
public record CreateProductResult(Guid Id);


internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
    {
        // Create Product Entity from command object
        // Save the product to the database
        // Return CreateProductResult

        var produc = new Product
        {
            Name = command.Name,
            Category = command.Category,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Price = command.Price
        };

        // To do
        // Save to DB

        session.Store(produc);
        await session.SaveChangesAsync(cancellationToken);

        // Return result
        return new CreateProductResult(Guid.NewGuid());
    }
}
