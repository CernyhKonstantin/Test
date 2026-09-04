import { api } from "./axios";

export interface Favorite {
  id: number;
  listingId: number;
  title: string;
  pricePerNight: number;
  city: string;
  country: string;
  imageUrl: string;
}

export const getFavorites = async () =>
  (await api.get<Favorite[]>("/favorites")).data;

export const toggleFavorite = async (listingId: number) =>
  (await api.post<{ isFavorite: boolean }>(`/favorites/${listingId}/toggle`)).data;
