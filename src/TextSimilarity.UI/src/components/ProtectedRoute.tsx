import { Navigate, Route, useLocation } from 'react-router-dom';
import { useAppSelector } from '../hooks/redux';

const ProtectedRoute = ({ children }: { children: JSX.Element }) => {

  const { isAuthenticated } = useAppSelector(state => state.authReducer);

  if (!isAuthenticated) {
    return <Navigate to="/sign-in" />;
  }

  return children;
};

export default ProtectedRoute;
