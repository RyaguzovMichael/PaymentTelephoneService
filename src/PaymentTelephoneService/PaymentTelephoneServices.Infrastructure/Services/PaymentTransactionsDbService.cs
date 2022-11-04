using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PaymentTelephoneServices.Application.Contracts;
using PaymentTelephoneServices.Domain.DbModels;
using PaymentTelephoneServices.Domain.Models;
using PaymentTelephoneServices.Domain.OptionModels;
using System.Text.Json;
using PaymentTelephoneServices.Infrastructure.Persistence;

namespace PaymentTelephoneServices.Infrastructure.Services;

internal class PaymentTransactionsDbService : IPaymentTransactionsDbService
{
    private readonly IOptions<OperatorCodesOptions> _operatorCodes;
    private readonly TransactionsContext _context;
    private readonly ILogger<PaymentTransactionsDbService> _logger;

    public PaymentTransactionsDbService(IOptions<OperatorCodesOptions> operatorCodes,
                                        TransactionsContext context,
                                        ILogger<PaymentTransactionsDbService> logger)
    {
        _operatorCodes = operatorCodes;
        _context = context;
        _logger = logger;
    }

    public async Task SavePaymentTransactionAsync(Payment payment, CancellationToken cancellationToken)
    {
        var operatorName = _operatorCodes.Value.GetOperatorNameByCode(payment.PhoneNumber.OperatorCode);
        if (operatorName is null)
        {
            _logger.LogWarning("Payment {} addressed to unknown operator.\nTransaction is not saved in Db.", 
                               JsonSerializer.Serialize(payment, new JsonSerializerOptions() { WriteIndented = true }));
            return;
        }
        
        var operatorEntity = await GetOperatorEntity(operatorName, cancellationToken);
        var userEntity = await GetUserEntity(payment, cancellationToken);

        Transaction transaction = new()
        {
            User = userEntity,
            UserId = userEntity.Id,
            Operator = operatorEntity,
            OperatorId = operatorEntity.Id,
            PaymentAmount = payment.PaymentAmount,
            TransactionTime = DateTime.Now
        };
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("New transaction added in Db.\nTransaction: {}",
                               JsonSerializer.Serialize(transaction, new JsonSerializerOptions() { WriteIndented = true }));
    }

    private async Task<User> GetUserEntity(Payment payment, CancellationToken cancellationToken)
    {
        var userEntity = await _context.Users.FirstOrDefaultAsync(entity => entity.PhoneNumber == payment.PhoneNumber.ToString(), cancellationToken);
        if (userEntity is not null) return userEntity;
        
        userEntity = new User() { PhoneNumber = payment.PhoneNumber.ToString() };
        await _context.Users.AddAsync(userEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"User with number {payment.PhoneNumber} is not found in Db. \nCreated new User");
        return userEntity;
    }

    private async Task<Operator> GetOperatorEntity(string operatorName, CancellationToken cancellationToken)
    {
        var operatorEntity = await _context.Operators.FirstOrDefaultAsync(entity => entity.Name == operatorName, cancellationToken);
        if (operatorEntity is not null) return operatorEntity;
        
        operatorEntity = new Operator() { Name = operatorName };
        await _context.Operators.AddAsync(operatorEntity, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"Operator with name {operatorName} is not found in Db. \nCreated new Operator");
        return operatorEntity;
    }
}
