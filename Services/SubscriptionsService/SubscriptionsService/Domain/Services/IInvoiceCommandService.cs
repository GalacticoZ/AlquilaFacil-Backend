using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Commands;

namespace SubscriptionsService.Domain.Services;

public interface IInvoiceCommandService
{
    public Task<Invoice?> Handle(CreateInvoiceCommand command);
}