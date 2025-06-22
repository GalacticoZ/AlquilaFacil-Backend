export class ReservationResponse {
  constructor({ id, startDate, endDate, userId, localId }) {
    this.id = id;
    this.startDate = startDate;
    this.endDate = endDate;
    this.userId = userId;
    this.localId = localId;
  }
}


export const userReservationsFakeData = [
  {
    id: 1,
    start: '2025-04-11 08:00',
    end: '2025-04-11 09:00',
    userId: 1,
    localId: 1,
    title: 'Tu reservación',
    calendarId: 'userReservation',
  },
  {
    id: 2,
    start: '2025-04-12 08:00',
    end: '2025-04-12 10:00',
    userId: 2,
    localId: 2,
    title: 'Tu reservación',
    calendarId: 'userReservation',
  }
];

export const localReservationsFakeData = [
  {
    id: 3,
    start: '2025-04-11 11:00',
    end: '2025-04-11 12:00',
    userId: 1,
    localId: 1,
    title: 'Reserva de tu espacio',
    calendarId: 'localReservation',
  },
  {
    id: 4,
    start: '2025-04-12 11:00',
    end: '2025-04-12 12:00',
    userId: 2,
    localId: 2,
    title: 'Reserva de tu espacio',
    calendarId: 'localReservation',
  }
]

export const premiumUserLocalReservationsFakeData = [
  {
    id: 5,
    start: '2025-04-11 13:00',
    end: '2025-04-11 14:00',
    userId: 1,
    localId: 1,
    title: 'Reserva premium de tu espacio',
    calendarId: 'premiumUserLocalReservation',
  },
  {
    id: 6,
    start: '2025-04-12 13:00',
    end: '2025-04-12 15:00',
    userId: 2,
    localId: 2,
    title: 'Reserva premium de tu espacio',
    calendarId: 'premiumUserLocalReservation',
  }
];
