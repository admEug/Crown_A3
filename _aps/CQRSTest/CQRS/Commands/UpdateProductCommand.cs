using CQRSTest.Models;
using CQRSTest.Repository;
using MediatR;

namespace CQRSTest.CQRS.Commands
{
    public class UpdateProductCommand : IRequest<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, int>
        {      
            private IRepository<Product> _repo;

            public UpdateProductCommandHandler(IRepository<Product> repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                Product product = _repo.ReadAsync(command.Id).Result;                   

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.Name = command.Name;
                    product.Price = command.Price;

                    await _repo.UpdateAsync(product);

                    return product.Id;
                }
            }
        }
    }
}
