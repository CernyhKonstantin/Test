import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { getFavorites, toggleFavorite, type Favorite } from "../../api/favoritesApi";

export default function Favorites() {
  const [favorites, setFavorites] = useState<Favorite[]>([]);
  const load = () => getFavorites().then(setFavorites);
  useEffect(load, []);
  return <main className="content"><h1>Favorites</h1>
    <div className="grid">{favorites.map(f => <Link className="listing-card" key={f.id} to={`/listing/${f.listingId}`}>
      <img src={f.imageUrl} alt={f.title}/><div className="listing-info"><strong>{f.title}</strong><span>{f.city}, {f.country}</span><strong>€{f.pricePerNight} night</strong>
      <button className="secondary" onClick={async e => { e.preventDefault(); await toggleFavorite(f.listingId); load(); }}>Remove</button></div>
    </Link>)}</div>
  </main>;
}
