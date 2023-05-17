import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { AuthPayload, AuthState } from "./types";
import { USER } from "../../../common/constants";


function getInitialState(): AuthState {
    const userStr = localStorage.getItem(USER)
    let user: AuthPayload = {
        token: '',
        username: ''
    }
    if (userStr){
        user = JSON.parse(userStr)
    }
    return {
        isAuthenticated: Boolean(user.token),
        token: user.token,
        username: user.username
    }
}

const initialState: AuthState = getInitialState()

export const authSlice = createSlice({
    name: 'auth',
    initialState,
    reducers: {
        logout(state) {
            localStorage.removeItem(USER)
            state.isAuthenticated = false
            state.token = ''
        },
        loginSuccess(state, action: PayloadAction<AuthPayload>) {
            localStorage.setItem(USER, JSON.stringify(action.payload))
            state.isAuthenticated = Boolean(action.payload.token)
            state.username = action.payload.username
            state.token = action.payload.token
        }
    }
    
})

export default authSlice.reducer