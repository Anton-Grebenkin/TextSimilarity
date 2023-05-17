import { Navigate, useLocation, useNavigate } from 'react-router-dom';
import { useAppSelector } from '../store';

interface IProps {
  onlyPublic?: boolean;
  children: JSX.Element;
}

const ProtectedRoute = ({children, onlyPublic = false}: IProps) => {

  const { isAuthenticated } = useAppSelector(state => state.authReducer);
  const location = useLocation();

  if (onlyPublic && isAuthenticated) {
    return <Navigate to='/dashboard' state={{ from: location }}/>;
  }

  if (!onlyPublic && !isAuthenticated) {
    return <Navigate to="/sign-in" state={{ from: location }}/>;
  }

  return children;
};

export default ProtectedRoute;
