namespace CCAS.Application.Common.Exceptions;

public class DeleteForbiddenException : Exception
{
    public DeleteForbiddenException()
    : base()
    {
    }

    public DeleteForbiddenException(string message)
        : base(message)
    {
    }

    public DeleteForbiddenException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
