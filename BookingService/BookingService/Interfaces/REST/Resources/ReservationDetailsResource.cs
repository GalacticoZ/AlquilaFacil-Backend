using BookingService.Interfaces.REST.Resources;

namespace BookingService.Interfaces.REST.Resources;

public record ReservationDetailsResource(IEnumerable<LocalReservationResource> Reservations);