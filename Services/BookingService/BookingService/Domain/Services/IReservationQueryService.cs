using BookingService.Domain.Model.Aggregates;
using BookingService.Domain.Model.Queries;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Domain.Services;

public interface IReservationQueryService
{
   
    Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(GetReservationsByUserId query);
    Task<IEnumerable<Reservation>>GetReservationByStartDateAsync(GetReservationByStartDate query);
    Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(GetReservationByEndDate query);
    Task<IEnumerable<LocalReservationResource>> GetReservationsByOwnerIdAsync(GetReservationsByOwnerIdQuery query);
    
}