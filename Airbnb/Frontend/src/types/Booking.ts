export interface Booking {
  id: number;
  listingId: number;
  listingTitle: string;
  mainImageUrl: string;
  checkIn: string;
  checkOut: string;
  guests: number;
  totalPrice: number;
  status: string;
}
