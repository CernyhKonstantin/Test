import { api } from "./axios";
import type { Booking } from "../types/Booking";

export const getMyBookings = async () =>
  (await api.get<Booking[]>("/bookings/mine")).data;

export const createBooking = async (data: {
  listingId: number;
  checkIn: string;
  checkOut: string;
  guests: number;
}) => (await api.post<Booking>("/bookings", data)).data;

export const cancelBooking = async (id: number) =>
  api.post(`/bookings/${id}/cancel`);
