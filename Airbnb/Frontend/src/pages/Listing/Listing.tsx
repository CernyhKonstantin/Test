import { FormEvent, useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { getListing } from "../../api/listingsApi";
import { createBooking } from "../../api/bookingsApi";
import { createReview, getReviews } from "../../api/reviewsApi";
import { toggleFavorite } from "../../api/favoritesApi";
import { useAuth } from "../../context/AuthContext";
import type { Listing as ListingType } from "../../types/Listing";
import type { Review } from "../../types/Review";
import Rating from "../../components/Rating/Rating";

export default function Listing() {
  const { id } = useParams();
  const { user } = useAuth();
  const [listing, setListing] = useState<ListingType | null>(null);
  const [reviews, setReviews] = useState<Review[]>([]);
  const [dates, setDates] = useState({ checkIn: "", checkOut: "", guests: "1" });
  const [review, setReview] = useState({ rating: 5, comment: "" });
  const [message, setMessage] = useState("");

  useEffect(() => {
    if (!id) return;
    getListing(Number(id)).then(setListing);
    getReviews(Number(id)).then(setReviews);
  }, [id]);

  if (!listing) return <main className="content"><p>Loading...</p></main>;

  const book = async (e: FormEvent) => {
    e.preventDefault();
    if (!user) { setMessage("Please log in before booking."); return; }
    try {
      const result = await createBooking({ listingId: listing.id, checkIn: dates.checkIn, checkOut: dates.checkOut, guests: Number(dates.guests) });
      setMessage(`Booking confirmed. Total: €${result.totalPrice}`);
    } catch (err: any) { setMessage(err.response?.data?.message ?? "Booking failed."); }
  };

  const favorite = async () => {
    if (!user) { setMessage("Please log in to save favorites."); return; }
    const result = await toggleFavorite(listing.id);
    setMessage(result.isFavorite ? "Saved to favorites." : "Removed from favorites.");
  };

  const submitReview = async (e: FormEvent) => {
    e.preventDefault();
    if (!user) { setMessage("Please log in to review."); return; }
    try {
      const created = await createReview(listing.id, review.rating, review.comment);
      setReviews([created, ...reviews]);
      setReview({ rating: 5, comment: "" });
      setMessage("Review added.");
    } catch (err: any) { setMessage(err.response?.data?.message ?? "Could not add review."); }
  };

  return <main className="content detail">
    <Link to="/">← Back to explore</Link>
    <div className="gallery">{listing.imageUrls.map((url, i) => <img key={i} src={url} alt={listing.title} />)}</div>
    <div className="detail-layout">
      <section>
        <h1>{listing.title}</h1>
        <p className="muted">{listing.city}, {listing.country} · {listing.propertyType}</p>
        <Rating value={listing.averageRating} count={listing.reviewCount} />
        <p>{listing.description}</p>
        <p>{listing.maxGuests} guests · {listing.bedrooms} bedrooms · {listing.beds} beds · {listing.bathrooms} bathrooms</p>
        <h2>Reviews</h2>
        {reviews.map(r => <article className="review" key={r.id}><strong>{r.userName}</strong><div>★ {r.rating}</div><p>{r.comment}</p></article>)}
        {user && <form className="form-card" onSubmit={submitReview}>
          <h3>Leave a review</h3>
          <select value={review.rating} onChange={e => setReview({...review, rating: Number(e.target.value)})}>
            {[1,2,3,4,5].map(x => <option key={x} value={x}>{x} stars</option>)}
          </select>
          <textarea value={review.comment} onChange={e => setReview({...review, comment: e.target.value})} placeholder="Your review" required />
          <button>Submit review</button>
        </form>}
      </section>
      <aside className="booking-card">
        <h2>€{listing.pricePerNight} <small>night</small></h2>
        <form onSubmit={book}>
          <label>Check in<input type="date" required value={dates.checkIn} onChange={e => setDates({...dates, checkIn: e.target.value})}/></label>
          <label>Check out<input type="date" required value={dates.checkOut} onChange={e => setDates({...dates, checkOut: e.target.value})}/></label>
          <label>Guests<input type="number" min="1" max={listing.maxGuests} value={dates.guests} onChange={e => setDates({...dates, guests: e.target.value})}/></label>
          <button>Reserve</button>
        </form>
        <button className="secondary" onClick={favorite}>♡ Save to favorites</button>
        {message && <p className="notice">{message}</p>}
      </aside>
    </div>
  </main>;
}
