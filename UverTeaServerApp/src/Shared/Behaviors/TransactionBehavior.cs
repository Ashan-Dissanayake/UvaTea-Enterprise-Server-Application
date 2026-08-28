using MediatR;
using Microsoft.Extensions.Logging;
using UverTeaServerApp.src.Shared.Persistence;

namespace UverTeaServerApp.Shared.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;

    public TransactionBehavior(
        IUnitOfWork unitOfWork,
        ILogger<TransactionBehavior<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!IsCommand())
        {
            return await next();
        }

        var requestName = typeof(TRequest).Name;

        try
        {
            _logger.LogInformation("[TRANSACTION] Beginning transaction for {RequestName}", requestName);
            await _unitOfWork.BeginTransactionAsync(cancellationToken);

            var response = await next();

            _logger.LogInformation("[TRANSACTION] Committing transaction for {RequestName}", requestName);
            await _unitOfWork.CommitTransactionAsync(cancellationToken);

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "[TRANSACTION] Error handling {RequestName}. Rolling back transaction...", requestName);
            await _unitOfWork.RollbackTransactionAsync(cancellationToken);
            throw;
        }
    }

    private static bool IsCommand()
    {
        var requestType = typeof(TRequest);
        return requestType.Name.EndsWith("Command", StringComparison.OrdinalIgnoreCase) ||
               requestType.Name.Contains("Command", StringComparison.OrdinalIgnoreCase) ||
               typeof(ITransactionalRequest).IsAssignableFrom(requestType);
    }
}
