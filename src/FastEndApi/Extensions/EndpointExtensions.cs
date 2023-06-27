
using FluentValidation.Results;

namespace FastEndApi.Extensions;

public static class EndpointExtensions
{
    public static void AddGeneralError<TRequest, TResponse>(this Endpoint<TRequest, TResponse> endpoint, string message)
        where TRequest : notnull
    {
        endpoint.AddError(new ValidationFailure("", message));
    }
}
