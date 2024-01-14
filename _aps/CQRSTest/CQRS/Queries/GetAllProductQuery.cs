using CQRSTest.Models;
using CQRSTest.Repository;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CQRSTest.CQRS.Queries
{
    public record GetAllProductQuery : IRequest<IEnumerable<Product>>
    {       
    }

    public class GetAllProductQueryHandler : IRequestHandler<GetAllProductQuery, IEnumerable<Product>>
    {
        private IRepository<Product> _repo;
        public GetAllProductQueryHandler(IRepository<Product> repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<Product>> Handle(GetAllProductQuery query, CancellationToken cancellationToken)
        {
            var productList = await _repo.ReadAllAsync();
            return productList;
        }
    }
}
