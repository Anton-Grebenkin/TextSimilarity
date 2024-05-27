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

export interface IHistoryItem{
    requestDate: Date
    duration: number
    request: string
    response: string
    responseCode: number
}
export interface IGetAPIHistoryResponse {
    items: IHistoryItem[]
    rowCount: number
}