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
        string? operatorName = _operatorCodes.Value.GetOperatorNameByCode(payment.PhoneNumber.OperatorCode);
        if (operatorName is null)
        {
            _logger.LogWarning($"Payment {JsonSerializer.Serialize(payment, new JsonSerializerOptions() { WriteIndented = true })}" +
                               $" addressed to unknown  operator.\n" +
                               $"Transaction is not saved in Db.");
            return;
        }
        Operator? operatorEntity = await _context.Operators.FirstOrDefaultAsync(entity => entity.Name == operatorName, cancellationToken: cancellationToken);
        if (operatorEntity is null)
        {
            operatorEntity = new Operator() { Name = operatorName };
            await _context.Operators.AddAsync(operatorEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"Operator with name {operatorName} is not found in Db. \nCreated new Operator");
        }
        User? userEntity = await _context.Users.FirstOrDefaultAsync(entity => entity.PhoneNumber == payment.PhoneNumber.ToString(), cancellationToken: cancellationToken);
        if (userEntity is null)
        {
            userEntity = new User() { PhoneNumber = payment.PhoneNumber.ToString() };
            await _context.Users.AddAsync(userEntity, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User with number {payment.PhoneNumber} is not found in Db. \nCreated new User");
        }
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
        _logger.LogInformation($"New transaction added in Db.\nTransaction: {JsonSerializer.Serialize(transaction, new JsonSerializerOptions() { WriteIndented = true })}");
    }
}
