import { useState } from "react";

export default function SearchBar({ onSearch }: { onSearch: (city: string, guests?: number) => void }) {
  const [city, setCity] = useState("");
  const [guests, setGuests] = useState("");

  return (
    <form className="search-bar" onSubmit={(e) => {
      e.preventDefault();
      onSearch(city, guests ? Number(guests) : undefined);
    }}>
      <div><label>Where</label><input value={city} onChange={e => setCity(e.target.value)} placeholder="Search destinations" /></div>
      <div><label>Guests</label><input type="number" min="1" value={guests} onChange={e => setGuests(e.target.value)} placeholder="Add guests" /></div>
      <button type="submit">Search</button>
    </form>
  );
}
