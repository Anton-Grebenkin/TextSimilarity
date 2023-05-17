export interface IAuthRequest {
    password: string
    login: string
}

export interface IAuthResponse {
    authToken: string
    login: string
}

export interface IGetAPIKeyResponse {
    apiKey: string
}