using BookingService.Domain.Model.Commands;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST.Transform;

public static class CreateReservationCommandFromResourceAssembler
{
    public static CreateReservationCommand ToCommandFromResource( CreateReservationResource resource)
    {
        return new CreateReservationCommand(
            resource.StartDate,
            resource.EndDate,
            resource.UserId,
            resource.LocalId,
            resource.price,
            resource.voucherImageUrl
        );
    }
}