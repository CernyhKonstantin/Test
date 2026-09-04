import { useEffect, useState } from "react";
import { getListings } from "../../api/listingsApi";
import type { Listing } from "../../types/Listing";
import SearchBar from "../../components/SearchBar/SearchBar";
import ListingCard from "../../components/ListingCard/ListingCard";

export default function Home() {
  const [listings, setListings] = useState<Listing[]>([]);
  const [loading, setLoading] = useState(true);

  const load = async (city?: string, guests?: number) => {
    setLoading(true);
    try { setListings(await getListings({ city, guests })); }
    finally { setLoading(false); }
  };

  useEffect(() => { load(); }, []);

  return (
    <main>
      <section className="hero">
        <h1>Find a place you'll love.</h1>
        <p>Discover unique homes and unforgettable stays.</p>
        <SearchBar onSearch={load} />
      </section>
      <section className="content">
        <h2>Explore stays</h2>
        {loading ? <p>Loading...</p> : <div className="grid">{listings.map(x => <ListingCard key={x.id} listing={x} />)}</div>}
      </section>
    </main>
  );
}
