namespace CatalogApi.Products.UpdateProduct;

public record UpdateProductCommand(
    Guid Id,
    string Name,
    string Description,
    string ImageFile,
    decimal Price,
    List<string> Category
) : ICommand<UpdateProductResult>;
public record UpdateProductResult(bool IsSuccess);

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required!");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required!").Length(2, 150).WithMessage("Name must be between 2 and 150 characters!");
        RuleFor(x => x.Category).NotEmpty().WithMessage("Category is required!");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required!");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0!");
    }
}

internal class UpdateProductCommandHandler
    (IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        // Load the product from the database
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        
        if (product == null)
        {
            //return new UpdateProductResult(false);
            throw new ProductNotFoundException(command.Id);
        }
        
        // Update product properties
        product.Name = command.Name;
        product.Description = command.Description;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;
        product.Category = command.Category;
        
        // Save changes to the database
        session.Update(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new UpdateProductResult(true);
    }
}
