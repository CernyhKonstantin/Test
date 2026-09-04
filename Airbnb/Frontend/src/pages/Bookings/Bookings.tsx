import { useEffect, useState } from "react";
import { cancelBooking, getMyBookings } from "../../api/bookingsApi";
import type { Booking } from "../../types/Booking";

export default function Bookings() {
  const [bookings, setBookings] = useState<Booking[]>([]);
  useEffect(() => { getMyBookings().then(setBookings); }, []);
  const cancel = async (id: number) => { await cancelBooking(id); setBookings(await getMyBookings()); };

  return <main className="content"><h1>My trips</h1>
    <div className="booking-list">{bookings.map(b => <article className="booking-row" key={b.id}>
      <img src={b.mainImageUrl} alt={b.listingTitle} />
      <div><h3>{b.listingTitle}</h3><p>{b.checkIn.slice(0,10)} → {b.checkOut.slice(0,10)}</p><p>{b.guests} guests · €{b.totalPrice}</p><strong>{b.status}</strong></div>
      {b.status !== "Cancelled" && <button className="secondary" onClick={() => cancel(b.id)}>Cancel</button>}
    </article>)}</div>
  </main>;
}
