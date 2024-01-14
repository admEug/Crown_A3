﻿using CQRSTest.Models;
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
            private ProductContext context;
            public UpdateProductCommandHandler(ProductContext context)
            {
                this.context = context;
            }
            public async Task<int> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
            {
                var product = context.Product.Where(a => a.Id == command.Id).FirstOrDefault();

                if (product == null)
                {
                    return default;
                }
                else
                {
                    product.Name = command.Name;
                    product.Price = command.Price;
                    await context.SaveChangesAsync();
                    return product.Id;
                }
            }
        }
    }
}
