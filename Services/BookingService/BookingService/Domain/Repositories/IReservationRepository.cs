using System.Collections;
using BookingService.Domain.Model.Aggregates;
using Shared.Domain.Repositories;

namespace BookingService.Domain.Repositories;

public interface IReservationRepository : IBaseRepository<Reservation>
{
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(int userId);
    Task<IEnumerable<Reservation>>GetReservationByStartDateAsync(DateTime startDate);
    Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(DateTime endDate);
    Task<IEnumerable<Reservation>> GetReservationsByLocalIdsListAsync(List<int> localIdsList);
}