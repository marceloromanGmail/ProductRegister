using Application._Common.Exceptions;
using AutoMapper;
using Domain.Main.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Main.Products.Commands.CreateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IMainContext _context;
        private readonly IMapper _mapper;
        public UpdateProductCommandHandler(IMainContext mainContext, IMapper mapper)
        {
            _context = mainContext;
            _mapper = mapper;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Products.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (entity == null)
            {
                throw new NotFoundException(nameof(Product), request.Id);
            }

            entity.Name = request.Name;
            entity.Description = request.Description;
            entity.Price = request.Price;
            entity.Stock = request.Stock;
            entity.Status = request.Status;

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
