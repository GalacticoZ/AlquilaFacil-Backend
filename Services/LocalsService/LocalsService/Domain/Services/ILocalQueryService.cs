using LocalsService.Domain.Model.Aggregates;
using LocalsService.Domain.Model.Queries;

namespace LocalsService.Domain.Services;

public interface ILocalQueryService
{
    Task<IEnumerable<Local>> Handle(GetAllLocalsQuery query);
    Task<Local?> Handle(GetLocalByIdQuery query);
    HashSet<string> Handle(GetAllLocalDistrictsQuery query);
    
    Task<IEnumerable<Local>> Handle(GetLocalsByUserIdQuery query);
    Task<IEnumerable<Local>> Handle(GetLocalsByCategoryIdAndCapacityRangeQuery query);
    Task<bool> Handle(IsLocalOwnerQuery query);
    Task<int> Handle(GetLocalOwnerIdByLocalId query);


}