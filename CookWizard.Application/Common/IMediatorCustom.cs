namespace CookWizard.Application.Common;

public interface IMediatorCustom
{
    Task<TResponse> SendAsync<TResponse>(IRequestCustom<TResponse> request, CancellationToken cancellationToken = default);
}
