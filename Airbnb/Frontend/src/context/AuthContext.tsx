import { createContext, useContext, useEffect, useState } from "react";
import type { User } from "../types/User";
import { login as loginApi, register as registerApi } from "../api/authApi";

interface AuthContextValue {
  user: User | null;
  token: string | null;
  login: (email: string, password: string) => Promise<void>;
  register: (firstName: string, lastName: string, email: string, password: string) => Promise<void>;
  logout: () => void;
}

const AuthContext = createContext<AuthContextValue | undefined>(undefined);

export function AuthProvider({ children }: { children: React.ReactNode }) {
  const [user, setUser] = useState<User | null>(() => {
    const raw = localStorage.getItem("airbnb_user");
    return raw ? JSON.parse(raw) : null;
  });
  const [token, setToken] = useState<string | null>(() => localStorage.getItem("airbnb_token"));

  useEffect(() => {
    if (user) localStorage.setItem("airbnb_user", JSON.stringify(user));
    else localStorage.removeItem("airbnb_user");
  }, [user]);

  const save = (data: { token: string; user: User }) => {
    localStorage.setItem("airbnb_token", data.token);
    setToken(data.token);
    setUser(data.user);
  };

  const login = async (email: string, password: string) => save(await loginApi(email, password));
  const register = async (firstName: string, lastName: string, email: string, password: string) =>
    save(await registerApi(firstName, lastName, email, password));

  const logout = () => {
    localStorage.removeItem("airbnb_token");
    localStorage.removeItem("airbnb_user");
    setToken(null);
    setUser(null);
  };

  return <AuthContext.Provider value={{ user, token, login, register, logout }}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const value = useContext(AuthContext);
  if (!value) throw new Error("useAuth must be used inside AuthProvider.");
  return value;
}
