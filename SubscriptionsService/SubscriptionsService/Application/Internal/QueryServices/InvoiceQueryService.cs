using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Model.Queries;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Domain.Services;

namespace SubscriptionsService.Application.Internal.QueryServices;

public class InvoiceQueryService(IInvoiceRepository invoiceRepository) : IInvoiceQueryService
{
    public async Task<Invoice?> Handle(GetInvoiceByIdQuery query)
    {
        return await invoiceRepository.FindByIdAsync(query.Id);
    }
    
    public async Task<IEnumerable<Invoice>> Handle(GetAllInvoicesQuery query)
    {
        return await invoiceRepository.ListAsync();
    }
}