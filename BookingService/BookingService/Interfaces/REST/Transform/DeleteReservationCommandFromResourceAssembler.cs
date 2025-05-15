using BookingService.Domain.Model.Commands;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST.Transform;

public static class DeleteReservationCommandFromResourceAssembler
{
    public static DeleteReservationCommand ToCommandFromResource(DeleteReservationResource resource)
    {
        return new DeleteReservationCommand(resource.Id);
    }
}