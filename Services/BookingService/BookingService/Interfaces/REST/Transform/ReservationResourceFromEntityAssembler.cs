using BookingService.Domain.Model.Aggregates;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST.Transform;

public static class ReservationResourceFromEntityAssembler
{
    public static ReservationResource ToResourceFromEntity(Reservation entity)
    {
        return new ReservationResource
        (
            entity.Id,
            entity.StartDate,
            entity.EndDate,
            entity.UserId,
            entity.LocalId
        );
    }
}