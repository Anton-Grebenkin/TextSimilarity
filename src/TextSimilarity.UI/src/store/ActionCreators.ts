import { isAxiosError } from "axios"
import { AppDispatch } from "."
import axios from "../axios"
import { IAuth, IAuthResponse } from "../models/models"
import { authSlice } from "./slices/AuthSlice"

export const register = (data: IAuth) => {
    return async (dispatch: AppDispatch) => {
        dispatch(authSlice.actions.login())
        try {
            const response = await axios.post<IAuthResponse>(`Account/Register`, data)
            dispatch(authSlice.actions.loginSuccess({
                token: response.data.authToken,
                username: data.login
            }))
        } catch (e) {
            let error = "Something went wrong"
            if (isAxiosError(e) && e.response?.data) {
                error = e.response?.data
            } 
            dispatch(authSlice.actions.loginError({ message: error }))
        }
    }
}

export const login = (data: IAuth) => {
    return async (dispatch: AppDispatch) => {
        dispatch(authSlice.actions.login())
        try {
            const response = await axios.post<IAuthResponse>(`Account/Login`, data)
            dispatch(authSlice.actions.loginSuccess({
                token: response.data.authToken,
                username: data.login
            }))
        } catch (e) {
            let error = "Something went wrong"
            if (isAxiosError(e) && e.response?.data) {
                error = e.response?.data
                console.log(error)
            } 
            dispatch(authSlice.actions.loginError({ message: error }))
        }
    }
}