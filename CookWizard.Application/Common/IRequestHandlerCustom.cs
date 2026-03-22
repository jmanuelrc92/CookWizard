namespace CookWizard.Application.Common;

public interface IRequestHandlerCustom<in TRequest, TResponse>where TRequest : IRequestCustom<TResponse>
{
    Task<TResponse> HandleAsync(TRequest request, CancellationToken cancellationToken = default);
}