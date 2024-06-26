import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import { IAuthRequest, IAuthResponse, IGetAPIHistoryResponse, IGetAPIKeyResponse } from './types';
import { baseQueryWithAuth } from '../../common/utils/baseQuery';
import { ColumnSort } from '@tanstack/table-core';
import { Filter } from '../../common/types';


export const ACCOUNT_API_REDUCER_KEY = 'accountApi';
export const accountApi = createApi({
  reducerPath: ACCOUNT_API_REDUCER_KEY,
  baseQuery: baseQueryWithAuth,
  tagTypes: ['APIKey'],
  endpoints: (builder) => ({
    register: builder.mutation<IAuthResponse, IAuthRequest>({
      query: (request) => {
        return ({
          url: '/Register',
          method: 'POST',
          body: request
        })
      }
    }),
    login: builder.mutation<IAuthResponse, IAuthRequest>({
      query: (request) => {
        return ({
          url: '/Login',
          method: 'POST',
          body: request
        })
      }
    }),
    getAPIKey: builder.query<IGetAPIKeyResponse, void>({
      query: () => {
        return ({
          url: '/GetAPIKey',
          method: 'GET',
        })
      },
      providesTags: ['APIKey']
    }),
    generateAPIKey: builder.mutation<IGetAPIKeyResponse, void>({
      query: () => {
        return ({
          url: '/GenerateAPIKey',
          method: 'POST',
        })
      },
      invalidatesTags: ['APIKey']
    }),
    revokeAPIKey: builder.mutation<void, void>({
      query: () => {
        return ({
          url: '/RevokeAPIKey',
          method: 'POST',
        })
      },
      invalidatesTags: ['APIKey']
    }),
    getAPIHistory: builder.query<IGetAPIHistoryResponse, Filter>({
      query: (filter) => {
        console.log(filter)
        return ({
          url: '/GetAPIHistory',
          method: 'POST',
          body: filter
        })
      },
    }),
  }),
});


export const {
  useRegisterMutation,
  useLoginMutation,
  useGetAPIKeyQuery,
  useGenerateAPIKeyMutation,
  useRevokeAPIKeyMutation,
  useGetAPIHistoryQuery
} = accountApi;


