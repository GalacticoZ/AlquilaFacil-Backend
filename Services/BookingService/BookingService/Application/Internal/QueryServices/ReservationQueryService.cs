using BookingService.Domain.Services;
using BookingService.Application.External.OutboundServices;
using BookingService.Domain.Model.Aggregates;
using BookingService.Domain.Model.Queries;
using BookingService.Domain.Repositories;
using BookingService.Interfaces.REST.Resources;

namespace BookingService.Application.Internal.QueryServices;

public class ReservationQueryService(IReservationRepository reservationRepository, IReservationLocalExternalService reservationLocalExternalService, ISubscriptionInfoExternalService subscriptionInfoExternalService) : IReservationQueryService
{
    public async Task<IEnumerable<Reservation>> GetReservationsByUserIdAsync(GetReservationsByUserId query)
    {
        return await reservationRepository.GetReservationsByUserIdAsync(query.UserId);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByStartDateAsync(GetReservationByStartDate query)
    {
        return await reservationRepository.GetReservationByStartDateAsync(query.StartDate);
    }

    public async Task<IEnumerable<Reservation>> GetReservationByEndDateAsync(GetReservationByEndDate query)
    {
        return await reservationRepository.GetReservationByEndDateAsync(query.EndDate);
    }

    public async Task<IEnumerable<LocalReservationResource>> GetReservationsByOwnerIdAsync(GetReservationsByOwnerIdQuery query)
    {
        var localReservations = new List<LocalReservationResource>();
        var ownerLocals = await reservationLocalExternalService.GetLocalsByUserId(query.OwnerId);
        if (ownerLocals == null)
        {
            return localReservations;
        }
        // Extracting local IDs from the locals
        var localIdsList = ownerLocals.Select(local => local.Id).ToList();
        var reservations = await reservationRepository.GetReservationsByLocalIdsListAsync(localIdsList);
        if (reservations == null)
        {
            return localReservations;
        }
        // Extracting user IDs from the reservations
        var userIdsList = reservations.Select(reservation => reservation.UserId).ToList();
        // Getting subscriptions by user IDs
        var subscriptions = await subscriptionInfoExternalService.GetSubscriptionByUserIdsList(userIdsList);
        // Creating a dictionary of subscriptions by user ID
        var subscriptionDict = subscriptions
            .GroupBy(s => s.UserId)
            .ToDictionary(g => g.Key, g => g.First());
        
        foreach (var reservation in reservations)
        {
            int subscriptionActiveStatus = 2;
            subscriptionDict.TryGetValue(reservation.UserId, out var subscription);
            var localReservationResource = new LocalReservationResource(
                reservation.Id,
                reservation.StartDate,
                reservation.EndDate,
                reservation.UserId,
                reservation.LocalId,
                subscription?.PlanId == subscriptionActiveStatus,
                reservation.VoucherImageUrl
            );
            localReservations.Add(localReservationResource);
        }

        return localReservations;
    }
}