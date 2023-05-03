import { Route, Routes } from "react-router-dom";
import HomePage  from "./pages/HomePage";
import DashboardPage from "./pages/DashboardPage";
import LoginPage from "./pages/LoginPage";
import RegisterPage from "./pages/RegisterPage";
import Navigation from "./components/Navigation";
import ProtectedRoute from "./components/ProtectedRoute";


function App() {
  return(
    <div className="bg-gray-100 min-h-screen flex flex-col">
      <Navigation/>
      <Routes>
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/home" element={<HomePage />}></Route>
        <Route path="/sign-in" element={<LoginPage />}></Route>
        <Route path="/sign-up" element={<RegisterPage />}></Route>
        <Route path="/dashboard" element={<ProtectedRoute><DashboardPage/></ProtectedRoute>}></Route>
      </Routes>
    </div>
  );
}


export default App;
