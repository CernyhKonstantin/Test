import { FormEvent, useEffect, useState } from "react";
import { getMe, updateMe } from "../../api/usersApi";
import { useAuth } from "../../context/AuthContext";

export default function Profile() {
  const { user } = useAuth();
  const [form, setForm] = useState({ firstName: "", lastName: "", phoneNumber: "", profileImageUrl: "" });
  const [message, setMessage] = useState("");
  useEffect(() => { getMe().then(u => setForm({ firstName: u.firstName, lastName: u.lastName, phoneNumber: u.phoneNumber ?? "", profileImageUrl: u.profileImageUrl ?? "" })); }, []);
  if (!user) return null;
  const submit = async (e: FormEvent) => { e.preventDefault(); await updateMe(form); setMessage("Profile updated."); };
  return <main className="auth-page"><form className="form-card" onSubmit={submit}><h1>Profile</h1><p>{user.email}</p>
    <input value={form.firstName} onChange={e => setForm({...form, firstName:e.target.value})} placeholder="First name"/>
    <input value={form.lastName} onChange={e => setForm({...form, lastName:e.target.value})} placeholder="Last name"/>
    <input value={form.phoneNumber} onChange={e => setForm({...form, phoneNumber:e.target.value})} placeholder="Phone"/>
    <input value={form.profileImageUrl} onChange={e => setForm({...form, profileImageUrl:e.target.value})} placeholder="Profile image URL"/>
    <button>Save changes</button>{message && <p className="notice">{message}</p>}</form></main>;
}
