export interface Review {
  id: number;
  listingId: number;
  userId: number;
  userName: string;
  rating: number;
  comment: string;
  createdAt: string;
}
