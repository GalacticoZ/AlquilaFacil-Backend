using Shared.Infrastructure.Persistence.EFC.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Repositories;
using SubscriptionsService.Infrastructure.Persistence.EFC.Configuration;

namespace SubscriptionsService.Infrastructure.Persistence.EFC.Repositories;

public class InvoiceRepository(AppDbContext context) : BaseRepository<Invoice>(context), IInvoiceRepository
{
    
}