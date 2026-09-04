export interface Listing {
  id: number;
  hostId: number;
  hostName: string;
  title: string;
  description: string;
  propertyType: string;
  city: string;
  country: string;
  address: string;
  pricePerNight: number;
  maxGuests: number;
  bedrooms: number;
  beds: number;
  bathrooms: number;
  averageRating: number;
  reviewCount: number;
  imageUrls: string[];
}
