import { BrowserRouter } from "react-router-dom";
import { AuthProvider } from "./context/AuthContext";
import Header from "./components/Header/Header";
import Footer from "./components/Footer/Footer";
import AppRoutes from "./routes/AppRoutes";
import "./styles.css";

export default function App() {
  return <BrowserRouter><AuthProvider><Header /><AppRoutes /><Footer /></AuthProvider></BrowserRouter>;
}
