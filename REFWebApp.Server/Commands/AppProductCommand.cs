using MediatR;

namespace REFWebApp.Server.Commands
{
    public record AddProductCommand(Product Product) : IRequest;
}
