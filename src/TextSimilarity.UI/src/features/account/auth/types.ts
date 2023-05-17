export interface AuthState {
    isAuthenticated: boolean
    token: string
    username: string
}

export interface AuthPayload {
    token: string
    username: string
}


export interface IRegisterForm {
    password: string
    confirmPassword: string
    login: string
}
