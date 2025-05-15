using BookingService.Domain.Model.Commands;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST.Transform;

public static class UpdateReservationDateCommandFromResourceAssembler
{
    public static UpdateReservationDateCommand ToCommandFromResource(int id,UpdateReservationResource resource)
    {
        return new UpdateReservationDateCommand(
            id,
            resource.StartDate,
            resource.EndDate
        );
    }
}

