
import { useMemo, useState } from "react";
import ErrorPage from "../../../common/components/ErrorPage";
import FullScreenLoader from "../../../common/components/FullScreenLoader";
import { useAppSelector } from "../../../common/store";
import { isApiError } from "../../../common/utils/apiErrorHelper";
import MaterialReactTable, { MRT_PaginationState, type MRT_ColumnDef, MRT_SortingState } from 'material-react-table';
import { useGenerateAPIKeyMutation, useGetAPIHistoryQuery, useGetAPIKeyQuery, useRevokeAPIKeyMutation } from "../accountApi";
import { IHistotyItem } from "../types";
import { format } from 'date-fns'
import { Navigate } from "react-router";


export default function DashboarPage() {
  const { username } = useAppSelector(state => state.authReducer);
  const [pagination, setPagination] = useState<MRT_PaginationState>({
    pageIndex: 0,
    pageSize: 10,
  });
  const [sorting, setSorting] = useState<MRT_SortingState>([]);
  const { data: apiKeyResponse, isLoading: getAPIKeyIsLoading, isSuccess: getAPIKeyIsSuccess, error: getAPIKeyError, isError: getAPIKeyIsError } = useGetAPIKeyQuery();
  const { data: apiHistoryResponse, isLoading: getAPIHistoryIsLoading, isFetching: getAPIHistoryIsFetching, isSuccess: getAPIHistoryIsSuccess, error: getAPIHistoryError, isError: getAPIHistoryIsError } = useGetAPIHistoryQuery({ start: pagination.pageIndex * pagination.pageSize, size: pagination.pageSize, sort: JSON.stringify(sorting ?? []) });
  const [generateAPIKey, { isLoading: generateAPIKeyIsLoading, isSuccess: generateAPIKeyIsSuccess, error: generateAPIKeyError, isError: generateAPIKeyIsError }] = useGenerateAPIKeyMutation();
  const [revokeAPIKey, { isLoading: revokeAPIKeyIsLoading, isSuccess: revokeAPIKeyIsSuccess, error: revokeAPIKeyError, isError: revokeAPIKeyIsError }] = useRevokeAPIKeyMutation();
  const showGenerateAPIKeyButton = () => {
    return getAPIKeyIsError && isApiError(getAPIKeyError) && getAPIKeyError.status == 404
  }
  const handleGenerate = async (event: React.MouseEvent) => {
    event.preventDefault()
    await generateAPIKey();
  }

  const handleRevoke = async (event: React.MouseEvent) => {
    event.preventDefault()
    await revokeAPIKey();
  }

  const columns = useMemo<MRT_ColumnDef<IHistotyItem>[]>(
    () => [
      {
        accessorKey: 'requestDate', //access nested data with dot notation
        header: 'requestDate',
        Cell: ({ cell }) => format(new Date(cell.getValue<Date>()), 'dd.MM.yyyy HH:mm:ss')
      },
      {
        accessorKey: 'duration',
        header: 'duration',
      },
      {
        accessorKey: 'request', //normal accessorKey
        header: 'request',
      },
      {
        accessorKey: 'response',
        header: 'response',
      },
      {
        accessorKey: 'responseCode',
        header: 'responseCode',
      },
    ],
    [],
  );

  if (getAPIKeyIsLoading || generateAPIKeyIsLoading || revokeAPIKeyIsLoading) {
    return <FullScreenLoader />
  }

  if ((getAPIKeyIsError && !isApiError(getAPIKeyError)) ||
    (generateAPIKeyIsError && !isApiError(generateAPIKeyError)) ||
    (revokeAPIKeyIsError && !isApiError(revokeAPIKeyError))) {
    return <Navigate to='/error' />
  }

  return (
    <div className="mx-auto max-w-7xl px-4 py-6 sm:px-6 lg:px-8">
      <div className="container flex flex-col gap-6">
        <h1 className="text-4xl md:text-5xl lg:text-6xl text-center md:text-left font-bold tracking-tight text-gray-900">Hello, {username}</h1>
        <div className='flex flex-col md:flex-row gap-4 justify-center md:justify-start items-center'>
          <p className="text-lg ">Your API key:</p>
          <input className='flex w-2/3 md:w-1/3 h-10 rounded-md border border-slate-300 bg-transparent py-2 px-3 text-sm placeholder:text-slate-400 focus:outline-none focus:ring-2 focus:ring-slate-400 focus:ring-offset-2 disabled:cursor-not-allowed disabled:opacity-50'
            readOnly placeholder="Generate an API key to display it here" value={`${apiKeyResponse?.apiKey && getAPIKeyIsSuccess ? apiKeyResponse?.apiKey : ''}`}
          />
          {showGenerateAPIKeyButton() &&
            <button onClick={handleGenerate} className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-md text-sm py-2 px-3 w-2/3 md:w-1/3 lg:w-1/6 h-10 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
              Generate API key
            </button>
          }
          {!showGenerateAPIKeyButton() &&
            <button onClick={handleRevoke} className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-md text-sm py-2 px-3 w-2/3 md:w-1/3 lg:w-1/6 h-10 text-center dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
              Revoke API key
            </button>
          }
        </div>
        <div>
          <p className="text-lg mt-4 mb-4 md:text-left text-center">Your API history:</p>
          <MaterialReactTable
            columns={columns}
            data={apiHistoryResponse?.items && getAPIHistoryIsSuccess ? apiHistoryResponse?.items : []}
            manualPagination
            onPaginationChange={setPagination}
            onSortingChange={setSorting}
            rowCount={apiHistoryResponse?.rowCount}
            state={{
              pagination,
              sorting,
              isLoading: getAPIHistoryIsLoading,
              showProgressBars: getAPIHistoryIsFetching
            }}
          />
        </div>
      </div>
    </div>
  )
}
