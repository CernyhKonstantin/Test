import { api } from "./axios";
import type { AuthResponse } from "../types/User";

export const login = async (email: string, password: string) =>
  (await api.post<AuthResponse>("/auth/login", { email, password })).data;

export const register = async (
  firstName: string,
  lastName: string,
  email: string,
  password: string
) => (await api.post<AuthResponse>("/auth/register", { firstName, lastName, email, password })).data;
