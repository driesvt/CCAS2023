using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Commands;

public class DeleteLecturerCommand : IRequest
{
    public int Id { get; set; }
}

public class DeleteLecturerCommandHandler : IRequestHandler<DeleteLecturerCommand>
{
    private readonly IApplicationDbContext context;

    public DeleteLecturerCommandHandler(IApplicationDbContext context)
    {
        this.context = context;
    }

    public async Task<Unit> Handle(DeleteLecturerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Lecturers
            .SingleOrDefaultAsync(l => l.Id == request.Id, cancellationToken);

        if (entity == null)
        {
            throw new NotFoundException(nameof(Lecturer), request.Id);
        }

        context.Lecturers.Remove(entity);

        await context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
