using CQRSTest.Models;
using CQRSTest.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSTest.CQRS.Queries
{
    public record GetProductByIdQuery : IRequest<Product>
    {
        public int Id { get; set; }       
    }

    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, Product>
    {
        private IRepository<Product> _repo;

        public GetProductByIdQueryHandler(IRepository<Product> repo)
        {
            _repo = repo;
        }

        public async Task<Product> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
        {
            var product = await _repo.ReadAsync(query.Id);
            return product;
        }
    }
}
