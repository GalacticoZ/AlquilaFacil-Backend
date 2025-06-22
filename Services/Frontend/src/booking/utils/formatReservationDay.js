export const formatReservationDay = (date) => (
  `${date.split('T')[0] } ${date.split('T')[1].split(':')[0]}:${date.split('T')[1].split(':')[1]}`
)