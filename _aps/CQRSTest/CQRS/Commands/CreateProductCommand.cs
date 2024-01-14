using CQRSTest.Models;
using CQRSTest.Repository;
using MediatR;

namespace CQRSTest.CQRS.Commands
{
    public record CreateProductCommand : IRequest<int>
    {
        public string Name { get; set; }
        public decimal Price { get; set; }       
    }

    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, int>
    {    
        private IRepository<Product> _repo;

        public CreateProductCommandHandler(IRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<int> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            var product = new Product();
            product.Name = command.Name;
            product.Price = command.Price;

            await _repo.CreateAsync(product);
       
            return product.Id;
        }
    }
}
