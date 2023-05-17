export interface ApiError{
  message: string
}

export interface ApiErrorResponse{
    status: number;
    data: ApiError
}

export function isApiError(error: unknown): error is ApiErrorResponse {
    return (
      typeof error === "object" &&
      error != null &&
      "status" in error &&
      typeof (error as any).status === "number" &&
      typeof (error as any).data.message === "string"
    );
  }