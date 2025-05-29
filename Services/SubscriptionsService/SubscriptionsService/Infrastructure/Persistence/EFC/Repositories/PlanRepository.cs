using SubscriptionsService.Domain.Model.Entities;
using SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Configuration;
using SubscriptionsService.Shared.Infrastructure.Persistence.EFC.Repositories;
using SubscriptionsService.Domain.Model.Aggregates;
using SubscriptionsService.Domain.Repositories;

namespace SubscriptionsService.Infrastructure.Persistence.EFC.Repositories;

public class PlanRepository(AppDbContext context) : BaseRepository<Plan>(context), IPlanRepository;