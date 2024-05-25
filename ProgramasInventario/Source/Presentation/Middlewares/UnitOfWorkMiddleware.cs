using Infrastructure.Postgres.Common;
using Microsoft.EntityFrameworkCore;

namespace Presentation.Middlewares;

public class UnitOfWorkMiddleware
{
    private readonly RequestDelegate _next;

    public UnitOfWorkMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context, IUnitOfWork unitOfWork)
    {
        try
        {
            await _next(context);
            await unitOfWork.Commit();
        }
        catch (DbUpdateException)
        {
            await unitOfWork.Rollback();
            throw;
        }
        catch (InvalidOperationException)
        {
            await unitOfWork.Rollback();
            throw;
        }
    }
}