import { Link } from "react-router-dom";
import type { Listing } from "../../types/Listing";
import Rating from "../Rating/Rating";

export default function ListingCard({ listing }: { listing: Listing }) {
  return (
    <Link to={`/listing/${listing.id}`} className="listing-card">
      <img src={listing.imageUrls[0] ?? "https://images.unsplash.com/photo-1564013799919-ab600027ffc6?auto=format&fit=crop&w=1200&q=80"} alt={listing.title} />
      <div className="listing-info">
        <div className="listing-top"><strong>{listing.city}, {listing.country}</strong><Rating value={listing.averageRating} count={listing.reviewCount} /></div>
        <span>{listing.propertyType} · {listing.bedrooms} bedrooms</span>
        <strong>€{listing.pricePerNight} night</strong>
      </div>
    </Link>
  );
}
