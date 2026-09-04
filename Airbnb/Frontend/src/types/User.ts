export interface User {
  id: number;
  firstName: string;
  lastName: string;
  email: string;
  phoneNumber?: string;
  profileImageUrl?: string;
  role: string;
}

export interface AuthResponse {
  token: string;
  user: User;
}
