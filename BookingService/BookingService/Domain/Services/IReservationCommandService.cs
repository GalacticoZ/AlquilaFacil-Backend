using BookingService.Domain.Model.Aggregates;
using BookingService.Domain.Model.Commands;

namespace BookingService.Domain.Services;

public interface IReservationCommandService
{
    Task<Reservation> Handle(CreateReservationCommand reservation);
    Task<Reservation> Handle(UpdateReservationDateCommand reservation);
    Task<Reservation> Handle(DeleteReservationCommand reservation);
}