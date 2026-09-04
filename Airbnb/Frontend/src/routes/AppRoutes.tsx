import { Navigate, Route, Routes } from "react-router-dom";
import Home from "../pages/Home/Home";
import Login from "../pages/Login/Login";
import Register from "../pages/Register/Register";
import Listing from "../pages/Listing/Listing";
import Bookings from "../pages/Bookings/Bookings";
import Favorites from "../pages/Favorites/Favorites";
import Profile from "../pages/Profile/Profile";
import { useAuth } from "../context/AuthContext";

function Private({ children }: { children: React.ReactNode }) {
  const { user } = useAuth();
  return user ? <>{children}</> : <Navigate to="/login" replace />;
}

export default function AppRoutes() {
  return <Routes>
    <Route path="/" element={<Home />} />
    <Route path="/listing/:id" element={<Listing />} />
    <Route path="/login" element={<Login />} />
    <Route path="/register" element={<Register />} />
    <Route path="/bookings" element={<Private><Bookings /></Private>} />
    <Route path="/favorites" element={<Private><Favorites /></Private>} />
    <Route path="/profile" element={<Private><Profile /></Private>} />
    <Route path="*" element={<Navigate to="/" replace />} />
  </Routes>;
}
