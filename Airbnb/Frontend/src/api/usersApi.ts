import { api } from "./axios";
import type { User } from "../types/User";

export const getMe = async () => (await api.get<User>("/users/me")).data;

export const updateMe = async (data: {
  firstName: string;
  lastName: string;
  phoneNumber?: string;
  profileImageUrl?: string;
}) => (await api.put<User>("/users/me", data)).data;
