export default function Rating({ value, count }: { value: number; count: number }) {
  return <span className="rating">★ {value ? value.toFixed(1) : "New"} {count ? `(${count})` : ""}</span>;
}
