import ErrorPage from "../../../common/components/ErrorPage";
import FullScreenLoader from "../../../common/components/FullScreenLoader";
import { useAppDispatch, useAppSelector } from "../../../common/store";
import { isApiError } from "../../../common/utils/apiErrorHelper";
import { useGenerateAPIKeyMutation, useGetAPIKeyQuery, useRevokeAPIKeyMutation } from "../accountApi";

export default function DashboarPage() {
  const { username } = useAppSelector(state => state.authReducer);
  const { data: apiKeyResponse, isLoading: getAPIKeyIsLoading, isSuccess: getAPIKeyIsSuccess, error: getAPIKeyError, isError: getAPIKeyIsError } = useGetAPIKeyQuery();
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

  if (getAPIKeyIsLoading || generateAPIKeyIsLoading || revokeAPIKeyIsLoading) {
    return <FullScreenLoader />
  }

  if ((getAPIKeyIsError && !isApiError(getAPIKeyError)) ||
    (generateAPIKeyIsError && !isApiError(generateAPIKeyError)) ||
    (revokeAPIKeyIsError && !isApiError(revokeAPIKeyError))) {
    return <ErrorPage />
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
      </div>
    </div>
  )
}
