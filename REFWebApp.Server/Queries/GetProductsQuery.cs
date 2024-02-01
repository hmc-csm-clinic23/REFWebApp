using MediatR;

namespace REFWebApp.Server.Queries
{
    public record GetProductsQuery() : IRequest<IEnumerable<Product>>;
}
