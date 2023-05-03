import { createSlice, PayloadAction } from "@reduxjs/toolkit";

interface AuthState {
    isAuthenticated: boolean
    token: string
    isLoading: boolean
    error: string
}

const ACCESS_KEY = 'dc-access'

function getInitialState(): AuthState {
    return {
        isAuthenticated: Boolean(localStorage.getItem(ACCESS_KEY) ?? ''),
        token: localStorage.getItem(ACCESS_KEY) ?? '',
        error: "",
        isLoading: false
    }
}

const initialState: AuthState = getInitialState()

interface AuthPayload {
    token: string
    username: string
}

interface ErrorPayload {
    message: string
}

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout(state) {
            state.isAuthenticated = false
            state.token = ''
            localStorage.removeItem(ACCESS_KEY)
        },
        login(state){
            state.isLoading = true
            state.error = ""
        },
        loginSuccess(state, action: PayloadAction<AuthPayload>) {
            state.token = action.payload.token
            state.isAuthenticated = Boolean(action.payload.token)
            state.error = ""
            localStorage.setItem(ACCESS_KEY, action.payload.token)
            state.isLoading = false
        },
        loginError(state, action: PayloadAction<ErrorPayload>){
            state.error = action.payload.message
            state.isLoading = false
        }
    }
})

export default authSlice.reducer