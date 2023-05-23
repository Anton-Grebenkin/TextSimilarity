import { Route, Routes } from "react-router-dom";
import HomePage  from "./common/components/HomePage";
import Navigation from "./common/components/Navigation";
import ProtectedRoute from "./common/components/ProtectedRoute";
import RegisterPage from "./features/account/auth/RegisterPage";
import LoginPage from "./features/account/auth/LoginPage";
import DashboardPage from "./features/account/dashboard/DashboardPage";
import ErrorPage from "./common/components/ErrorPage";


function App() {
  return(
    <div className="bg-gray-100 min-h-screen">
      <Navigation/>
      <Routes>
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/sign-in" element={<ProtectedRoute onlyPublic><LoginPage /></ProtectedRoute>}></Route>
        <Route path="/sign-up" element={<ProtectedRoute onlyPublic><RegisterPage /></ProtectedRoute>}></Route>
        <Route path="/dashboard" element={<ProtectedRoute><DashboardPage/></ProtectedRoute>}></Route>
        <Route path="/error" element={<ErrorPage/>}></Route>
      </Routes>
    </div>
  );
}


export default App;
