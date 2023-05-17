import { Link, useNavigate } from 'react-router-dom'
import { useFormik } from 'formik'
import * as Yup from 'yup';
import { useLoginMutation } from '../accountApi';
import { IAuthRequest } from '../types';
import { useEffect } from 'react';
import { authSlice } from './slice';
import { isApiError } from '../../../common/utils/apiErrorHelper';
import { useAppDispatch } from '../../../common/store';

export default function LoginPage() {
  const [login, { data, isLoading, isSuccess, error, isError }] = useLoginMutation();
  const dispatch = useAppDispatch()
  const navigate = useNavigate()

  const to = '/dashboard';

  const formik = useFormik<IAuthRequest>({
    initialValues: {
      login: '',
      password: ''
    },
    onSubmit: async (values) => {
      await login(values)
    },

    validationSchema: Yup.object({
      login: Yup.string()
        .required(),
      password: Yup.string()
        .required()
    })
  })

  useEffect(() => {
    if (isSuccess && data) {
      dispatch(authSlice.actions.loginSuccess({
        token: data.authToken,
        username: data.login
      }))
      navigate(to)
    }

  }, [isLoading]);

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
      <div className="container mx-auto mt-8 mb-auto">
        <h1 className="text-2xl font-bold mb-6 text-center">Sign in to your account</h1>
        <form onSubmit={formik.handleSubmit} className="w-full max-w-sm mx-auto bg-white p-8 rounded-md shadow-md">
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2">Login</label>
            <input className={`w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none ${formik.touched.login && formik.errors.login ? 'border-red-400' : 'border-gray-300 focus:border-indigo-500'}`}
              id="login" name="login" placeholder="Login" onChange={formik.handleChange} onBlur={formik.handleBlur} value={formik.values.login} />
            {(formik.touched.login && formik.errors.login) && (
              <span className='text-red-400'>{formik.errors.login}</span>
            )}
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2">Password</label>
            <input className={`w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none ${formik.touched.password && formik.errors.password ? 'border-red-400' : 'border-gray-300 focus:border-indigo-500'}`}
              type="password" id="password" name="password" placeholder="********" onChange={formik.handleChange} onBlur={formik.handleBlur} value={formik.values.password} />
            {(formik.touched.password && formik.errors.password) && (
              <span className='text-red-400'>{formik.errors.password}</span>
            )}
          </div>
          <div className='mb-4'>
            <button type='submit' className="text-white w-full bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-md text-sm px-5 py-2.5 text-center mr-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800">
              Sign in
              {isLoading && (<svg aria-hidden="true" role="status" className="inline w-4 ml-3 text-white animate-spin" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
                <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="#E5E7EB" />
                <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentColor" />
              </svg>)}
            </button>
          </div>

          {isError && <span className='text-red-400'>{(isApiError(error) && error.data.message) || 'Network error'}</span>}
        </form>
        <p className="mt-10 text-center text-sm text-gray-500">
          Not a member?{' '}
          <Link to="/sign-up" className="font-semibold leading-6 text-indigo-600 hover:text-indigo-500">
            Create new account
          </Link>
        </p>
      </div>
    </div>
  )
}