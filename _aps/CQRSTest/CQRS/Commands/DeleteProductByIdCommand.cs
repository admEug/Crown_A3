using CQRSTest.Models;
using CQRSTest.Repository;
using MediatR;

namespace CQRSTest.CQRS.Commands
{
    public record DeleteProductByIdCommand : IRequest<int>
    {
        public int Id { get; set; }
        public class DeleteProductByIdCommandHandler : IRequestHandler<DeleteProductByIdCommand, int>
        {     
            private IRepository<Product> _repo;

            public DeleteProductByIdCommandHandler(IRepository<Product> repo)
            {
                _repo = repo;
            }

            public async Task<int> Handle(DeleteProductByIdCommand command, CancellationToken cancellationToken)
            {
                await _repo.DeleteAsync(command.Id);

                return command.Id;          
            }
        }
    }
}
