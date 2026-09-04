import { api } from "./axios";
import type { Review } from "../types/Review";

export const getReviews = async (listingId: number) =>
  (await api.get<Review[]>(`/listings/${listingId}/reviews`)).data;

export const createReview = async (listingId: number, rating: number, comment: string) =>
  (await api.post<Review>(`/listings/${listingId}/reviews`, { rating, comment })).data;
