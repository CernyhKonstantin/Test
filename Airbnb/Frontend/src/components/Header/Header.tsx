import { Link } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function Header() {
  const { user, logout } = useAuth();

  return (
    <header className="header">
      <Link to="/" className="logo">airbnb</Link>
      <nav>
        <Link to="/">Explore</Link>
        {user && <Link to="/bookings">Trips</Link>}
        {user && <Link to="/favorites">Favorites</Link>}
        {user ? (
          <>
            <Link to="/profile">{user.firstName}</Link>
            <button className="link-button" onClick={logout}>Log out</button>
          </>
        ) : (
          <>
            <Link to="/login">Log in</Link>
            <Link className="signup" to="/register">Sign up</Link>
          </>
        )}
      </nav>
    </header>
  );
}
