import { api } from "./axios";
import type { Listing } from "../types/Listing";

export const getListings = async (params?: {
  city?: string;
  maxPrice?: number;
  guests?: number;
}) => (await api.get<Listing[]>("/listings", { params })).data;

export const getListing = async (id: number) =>
  (await api.get<Listing>(`/listings/${id}`)).data;

export const createListing = async (data: unknown) =>
  (await api.post<Listing>("/listings", data)).data;

export const deleteListing = async (id: number) =>
  api.delete(`/listings/${id}`);
