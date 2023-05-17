import { isRejectedWithValue, Middleware } from '@reduxjs/toolkit';
import { authSlice } from '../../../features/account/auth/slice';

export const unauthorizedMiddleware: Middleware = ({
 dispatch
}) => (next) => (action) => {
 if (isRejectedWithValue(action) && action.payload.status === 401) {
   dispatch(authSlice.actions.logout());
 }

 return next(action);
};