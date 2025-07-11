using BookingService.Domain.Services;
using BookingService.Application.External.OutboundServices;
using BookingService.Domain.Messaging;
using BookingService.Domain.Model.Aggregates;
using BookingService.Domain.Model.Commands;
using BookingService.Domain.Repositories;
using Shared.Application.External.OutboundServices;
using Shared.Domain.Model.Events;
using Shared.Domain.Repositories;

namespace BookingService.Application.Internal.CommandServices;

public class ReservationCommandService(
 IUserExternalService userExternalService,
 IReservationLocalExternalService reservationLocalExternalService, 
 IBookingEventPublisher bookingEventPublisher,
 //INotificationReservationExternalService notificationReservationExternalService,
 IReservationRepository reservationRepository,
 IUnitOfWork unitOfWork) : IReservationCommandService
{
 public async Task<Reservation> Handle(CreateReservationCommand reservation)
 {
     var userExists = await userExternalService.UserExists(reservation.UserId);
     if (!userExists)
     {
         throw new KeyNotFoundException("User does not exist");
     }

     var localExists = await reservationLocalExternalService.LocalReservationExists(reservation.LocalId);
     if (!localExists)
     {
         throw new KeyNotFoundException("Local does not exist");
     }
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new BadHttpRequestException("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
         throw new BadHttpRequestException("Start date must be greater than current date");
     }
     if (reservation.EndDate.Year < DateTime.Now.Year || reservation.EndDate.Month < DateTime.Now.Month || reservation.EndDate.Day < DateTime.Now.Day)
     {
         throw new BadHttpRequestException("End date must be greater than current date");
     }

     if (await reservationLocalExternalService.IsLocalOwner(reservation.UserId, reservation.LocalId))
     {
            throw new BadHttpRequestException("User is the owner of the local, he cannot make a reservation");
     }
     



     var reservationCreated = new Reservation(reservation);
     await reservationRepository.AddAsync(reservationCreated);
     var ownerId = await reservationLocalExternalService.GetOwnerIdByLocalId(reservation.LocalId);
     var reservationUser = await userExternalService.FetchUser(reservation.UserId);
     await bookingEventPublisher.PublishAsync(new BookingCreatedEvent
     {
         BookingId = reservationCreated.Id,
         UserEmail = reservationUser.Email,
         CreatedAt = DateTime.UtcNow,
         Content = "El usuario, "+ reservationUser.Username + "ha realizado una reserva en el local: " + reservation.LocalId + " desde " + reservation.StartDate.ToString("dd/MM/yyyy") + " hasta " + reservation.EndDate.ToString("dd/MM/yyyy") + ".",
         OwnerId = ownerId
     });
     
     await unitOfWork.CompleteAsync();
     
     return reservationCreated;
 }

 public async Task<Reservation> Handle(UpdateReservationDateCommand reservation)
 {
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new BadHttpRequestException("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
         throw new BadHttpRequestException("Start date must be greater than current date");
     }
     if (reservation.EndDate < DateTime.Now)
     {
         throw new BadHttpRequestException("End date must be greater than current date");
     }

     var reservationToUpdate = await reservationRepository.FindByIdAsync(reservation.Id);
        if (reservationToUpdate == null)
        {
            throw new KeyNotFoundException("Reservation does not exist");
        }
        reservationToUpdate.UpdateDate(reservation);
        reservationRepository.Update(reservationToUpdate);
        await unitOfWork.CompleteAsync();
        return reservationToUpdate;
 }

 public async Task<Reservation> Handle(DeleteReservationCommand reservation)
 {
     var reservationToDelete = await reservationRepository.FindByIdAsync(reservation.Id);
     if (reservationToDelete == null)
     {
         throw new KeyNotFoundException("Reservation does not exist");
     }

     reservationRepository.Remove(reservationToDelete);
     await unitOfWork.CompleteAsync();
     return reservationToDelete;
 }
}