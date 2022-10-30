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
        if (operatorName == null)
        {
            _logger.LogWarning($"Payment {JsonSerializer.Serialize(payment, new JsonSerializerOptions() { WriteIndented = true })}" +
                               $" addressed to unknown  operator.\n" +
                               $"Transaction is not saved in Db.");
            return;
        }
        Operator operatorEntity = await _context.Operators.FirstAsync(entity => entity.Name == operatorName, cancellationToken: cancellationToken);
        User? user = await _context.Users.FirstOrDefaultAsync(entity => entity.PhoneNumber == payment.PhoneNumber.ToString(), cancellationToken: cancellationToken);
        if (user is null)
        {
            user = new User() { PhoneNumber = payment.PhoneNumber.ToString() };
            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            _logger.LogInformation($"User with number {payment.PhoneNumber} is not found in Db. \nCreated new User");
        }
        Transaction transaction = new()
        {
            User = user,
            UserId = user.Id,
            Operator = operatorEntity,
            OperatorId = operatorEntity.Id,
            PaymentAmount = payment.PaymentAmount,
            TransactionTime = DateTime.Now
        };
        await _context.Transactions.AddAsync(transaction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation($"New transaction added in Db.\nTransaction: {JsonSerializer.Serialize(transaction, new JsonSerializerOptions() { WriteIndented = true })}");
    }

    public async Task SetMobileOperatorsData(CancellationToken cancellationToken)
    {
        bool hasChanges = false;
        foreach (var item in _operatorCodes.Value.OperatorCodes)
        {
            if (await _context.Operators.AnyAsync(e => e.Name == item.OperatorName, cancellationToken: cancellationToken)) continue;
            hasChanges = true;
            await _context.AddAsync(new Operator() { Name = item.OperatorName }, cancellationToken);
        }
        if (hasChanges) await _context.SaveChangesAsync(cancellationToken);        
    }
}
