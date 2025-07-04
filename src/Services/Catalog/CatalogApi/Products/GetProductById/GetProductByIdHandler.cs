
namespace CatalogApi.Products.GetProductById;

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;
public record GetProductByIdResult(Product Product);

internal class GetProductByIQuerydHandler 
    (IDocumentSession session, ILogger<GetProductByIQuerydHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        logger.LogInformation("GetProductByIQuerydHandler.Handle called with {@Query}", query);
        
        var product = await session.LoadAsync<Product>(query.Id, cancellationToken);
        
        if (product == null)
        {
            throw new ProductNotFoundException();
        }

        return new GetProductByIdResult(product);
    }
}