namespace CatalogApi.Products.DeleteProduct;

public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;
public record DeleteProductResult(bool IsSuccess);

internal class DeleteProductCommandHandler
    (IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);
        
        // Load the product from the database
        var product = await session.LoadAsync<Product>(command.Id, cancellationToken);
        if (product == null)
        {
            logger.LogWarning("Product with Id {Id} not found", command.Id);
            //return new DeleteProductResult(false);

            throw new ProductNotFoundException();
        }
        
        // Delete the product
        session.Delete(product);
        await session.SaveChangesAsync(cancellationToken);
        
        return new DeleteProductResult(true);
    }
}
