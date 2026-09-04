import { FormEvent, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useAuth } from "../../context/AuthContext";

export default function Register() {
  const { register } = useAuth();
  const navigate = useNavigate();
  const [form, setForm] = useState({ firstName: "", lastName: "", email: "", password: "" });
  const [error, setError] = useState("");

  const submit = async (e: FormEvent) => {
    e.preventDefault();
    setError("");
    try { await register(form.firstName, form.lastName, form.email, form.password); navigate("/"); }
    catch (err: any) { setError(err.response?.data?.message ?? "Registration failed."); }
  };

  return <main className="auth-page"><form className="form-card" onSubmit={submit}>
    <h1>Create account</h1>
    {error && <div className="error">{error}</div>}
    {(["firstName","lastName","email","password"] as const).map(key =>
      <input key={key} type={key === "password" ? "password" : key === "email" ? "email" : "text"}
        value={form[key]} onChange={e => setForm({...form, [key]: e.target.value})}
        placeholder={key.replace(/([A-Z])/g, " $1")} required />)}
    <button>Sign up</button>
    <p>Already registered? <Link to="/login">Log in</Link></p>
  </form></main>;
}
