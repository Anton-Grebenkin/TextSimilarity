import { AnyAction, combineReducers, configureStore,  getDefaultMiddleware,  Reducer } from "@reduxjs/toolkit"
import { unauthorizedMiddleware } from "./middleware/unauthorizedMiddleware";
import authReducer from "../../features/account/auth/slice"
import { ACCOUNT_API_REDUCER_KEY, accountApi } from "../../features/account/accountApi";
import { TypedUseSelectorHook, useDispatch, useSelector } from "react-redux";

const combinedReducer = combineReducers({
    authReducer,
    [ACCOUNT_API_REDUCER_KEY]: accountApi.reducer
  });

const rootReducer: Reducer = (state: RootState, action: AnyAction) => {
    if (action.type === "auth/logout") {
      state = {} as RootState;
    }
    return combinedReducer(state, action);
  };


export function setupStore() {
    return configureStore({
        reducer: rootReducer,
        middleware: (getDefaultMiddleware) => getDefaultMiddleware().concat([
          accountApi.middleware,
          //unauthorizedMiddleware    
        ]),
    })
}

export type AppStore = ReturnType<typeof setupStore>
export type RootState = ReturnType<typeof combinedReducer>;
export type AppDispatch = AppStore['dispatch']
export const useAppDispatch = () => useDispatch<AppDispatch>()
export const useAppSelector: TypedUseSelectorHook<RootState> = useSelector


