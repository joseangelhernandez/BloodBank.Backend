using BloodBank.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace BloodBank.Web;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken ct)
    {
        int status;
        if (exception is NotFoundException) status = StatusCodes.Status404NotFound;
        else if (exception is ForbiddenException) status = StatusCodes.Status403Forbidden;
        else if (exception is BusinessException business) status = business.StatusCode;
        else return false;

        await Results.Problem(detail: exception.Message, statusCode: status).ExecuteAsync(context);
        return true;
    }
}
