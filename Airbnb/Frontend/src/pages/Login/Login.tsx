import { FormEvent, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function Login() {
  const { login } = useAuth();
  const navigate = useNavigate();
  const [email, setEmail] = useState("guest@airbnb.local");
  const [password, setPassword] = useState("Password123!");
  const [error, setError] = useState("");

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError("");
    try { await login(email, password); navigate("/"); }
    catch (err: any) { setError(err.response?.data?.message ?? "Login failed."); }
  };

  return <main className="auth-page"><form className="form-card" onSubmit={submit}>
    <h1>Log in</h1>
    {error && <div className="error">{error}</div>}
    <input type="email" value={email} onChange={e => setEmail(e.target.value)} placeholder="Email" required />
    <input type="password" value={password} onChange={e => setPassword(e.target.value)} placeholder="Password" required />
    <button>Log in</button>
    <p>Don't have an account? <Link to="/register">Sign up</Link></p>
  </form></main>;
}
