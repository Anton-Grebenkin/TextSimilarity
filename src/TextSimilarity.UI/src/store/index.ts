import { combineReducers, configureStore } from "@reduxjs/toolkit"
import authReducer from './slices/AuthSlice'


const rootReducer = combineReducers({
    authReducer
})

export function setupStore() {
    return configureStore({
        reducer: rootReducer
    })
}

export type RootState = ReturnType<typeof rootReducer>
export type AppStore = ReturnType<typeof setupStore>
export type AppDispatch = AppStore['dispatch']


