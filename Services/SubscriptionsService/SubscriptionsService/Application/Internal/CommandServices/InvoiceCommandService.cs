using Shared.Domain.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.CommandServices;

public class InvoiceCommandService(
    IInvoiceRepository invoiceRepository,
    IUnitOfWork unitOfWork) : IInvoiceCommandService
{
    public async Task<Invoice?> Handle(CreateInvoiceCommand command)
    {
        if (command.Amount <= 0)
        {
            throw new Exception("Invoice amount cannot be negative or less than 0");
        }
        var invoice = new Invoice(command.SubscriptionId, command.Amount, command.Date);
        await invoiceRepository.AddAsync(invoice);
        await unitOfWork.CompleteAsync();
        return invoice;
    }
}