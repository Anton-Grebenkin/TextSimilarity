import { BaseQueryFn, FetchArgs, FetchBaseQueryError, fetchBaseQuery } from "@reduxjs/toolkit/dist/query"
import { authSlice } from "../../features/account/auth/slice"
import { RootState } from "../store"

export const baseQuery = fetchBaseQuery({
     baseUrl: `${process.env.REACT_APP_BASE_URL}Account/`,
     prepareHeaders: (headers, { getState }) => {
        const token = (getState() as RootState).authReducer.token
        if (token) {
            headers.set('Authorization', `Bearer ${token}`)
        }
        headers.set('Content-Type', 'application/json')
        return headers
    }, 
    })
export const baseQueryWithAuth: BaseQueryFn<
    string | FetchArgs,
    unknown,
    FetchBaseQueryError
> = async (args, api, extraOptions) => {
    let result = await baseQuery(args, api, extraOptions)
    if (result.error && (result.error.status === 401)) {
        await api.dispatch(authSlice.actions.logout())
    }
    return result
}
