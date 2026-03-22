namespace CookWizard.Application.Common;

public class MediatorCustom : IMediatorCustom
{
    private readonly IServiceProvider _serviceProvider;
    public MediatorCustom(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    public async Task<TResponse> SendAsync<TResponse>(IRequestCustom<TResponse> request, CancellationToken cancellationToken = default)
    {
        var handlerType = typeof(IRequestHandlerCustom<,>).MakeGenericType(request.GetType(), typeof(TResponse));
        var handler = _serviceProvider.GetService(handlerType);
        if (handler == null)
            throw new InvalidOperationException($"No se encontro un handler para {request.GetType().Name}");

        var method = handlerType.GetMethod("HandleAsync");

        return await (Task<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken });
    }
}
