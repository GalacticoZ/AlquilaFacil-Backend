using BookingService.Domain.Services;
using BookingService.Application.External.OutboundServices;
using BookingService.Application.External.OutboundServices;
using BookingService.Domain.Model.Aggregates;
using BookingService.Domain.Model.Commands;
using BookingService.Domain.Repositories;
using BookingService.Shared.Domain.Repositories;

namespace BookingService.Application.Internal.CommandServices;

public class ReservationCommandService(
 IUserReservationExternalService userReservationExternalService,
 IReservationLocalExternalService reservationLocalExternalService, 
 INotificationReservationExternalService notificationReservationExternalService,
 IReservationRepository reservationRepository,
 IUnitOfWork unitOfWork) : IReservationCommandService
{
 public async Task<Reservation> Handle(CreateReservationCommand reservation)
 {
     var userExists = await userReservationExternalService.UserExists(reservation.UserId);
     if (!userExists)
     {
         throw new Exception("User does not exist");
     }

     var localExists = await reservationLocalExternalService.LocalReservationExists(reservation.LocalId);
     if (!localExists)
     {
         throw new Exception("Local does not exist");
     }
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new Exception("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
         throw new Exception("Start date must be greater than current date");
     }
     if (reservation.EndDate.Year < DateTime.Now.Year || reservation.EndDate.Month < DateTime.Now.Month || reservation.EndDate.Day < DateTime.Now.Day)
     {
         throw new Exception("End date must be greater than current date");
     }

     if (await reservationLocalExternalService.IsLocalOwner(reservation.UserId, reservation.LocalId))
     {
            throw new BadHttpRequestException("User is the owner of the local, he cannot make a reservation");
     }
     
     

     var reservationCreated = new Reservation(reservation);
     await reservationRepository.AddAsync(reservationCreated);
     await unitOfWork.CompleteAsync();
     
     //var ownerId = await reservationLocalExternalService.GetOwnerIdByLocalId(reservation.LocalId);
     /*
     await notificationReservationExternalService.CreateNotification(
         "Nueva reservación",
         $"Tienes una nueva reservación desde el {reservation.StartDate:dd/MM/yyyy} a las {reservation.StartDate:HH:mm} hasta el {reservation.EndDate:dd/MM/yyyy} a las {reservation.EndDate:HH:mm}. Verifica los datos del voucher de pago para corroborar que el depósito se realizó correctamente",
         ownerId
     );
     */
     return reservationCreated;
 }

 public async Task<Reservation> Handle(UpdateReservationDateCommand reservation)
 {
     if(reservation.StartDate > reservation.EndDate)
     {
         throw new Exception("Start date must be less than end date");
     }
     if (reservation.StartDate.Year < DateTime.Now.Year || reservation.StartDate.Month < DateTime.Now.Month || reservation.StartDate.Day < DateTime.Now.Day)
     {
            throw new Exception("Start date must be greater than current date");
     }
     if (reservation.EndDate < DateTime.Now)
     {
         throw new Exception("End date must be greater than current date");
     }

     var reservationToUpdate = await reservationRepository.FindByIdAsync(reservation.Id);
        if (reservationToUpdate == null)
        {
            throw new Exception("Reservation does not exist");
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
         throw new Exception("Reservation does not exist");
     }

     reservationRepository.Remove(reservationToDelete);
     await unitOfWork.CompleteAsync();
     return reservationToDelete;
 }
}