import React, { ChangeEvent, FormEvent, MouseEvent, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { IAuth } from '../models/models'
import { useAppDispatch } from '../hooks/redux'
import { login, register } from '../store/ActionCreators'

export default function RegisterPage() {
  const dispatch = useAppDispatch()
  const navigate = useNavigate()
  const [form, setForm] = useState<IAuth>({
    password: '',
    login: ''
  })

  const isFormValid = () => {
    return form.password.trim().length && form.login.trim().length
  }

  const changeHandler = (event: ChangeEvent<HTMLInputElement>) => {
    setForm(prev => ({ ...prev, [event.target.name]: event.target.value }))
  }

  const registerHandler = async (event: MouseEvent<HTMLButtonElement>) => {
    event.preventDefault()
    if (isFormValid()) {
      await dispatch(register(form))
      navigate('/')
    } else {
      alert('Form is invalid!')
    }
  }

  return (
    <div className="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
      <div className="container mx-auto mt-8 mb-auto">
        <h1 className="text-2xl font-bold mb-6 text-center">Create new account</h1>
        <form className="w-full max-w-sm mx-auto bg-white p-8 rounded-md shadow-md">
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2">Login</label>
            <input className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
              id="login" name="login" placeholder="Login" onChange={changeHandler} />
          </div>
          <div className="mb-4">
            <label className="block text-gray-700 text-sm font-bold mb-2">Password</label>
            <input className="w-full px-3 py-2 border border-gray-300 rounded-md focus:outline-none focus:border-indigo-500"
              type="password" id="password" name="password" placeholder="********" onChange={changeHandler} />
          </div>
          <button className="w-full bg-indigo-500 text-white text-sm font-bold py-2 px-4 rounded-md hover:bg-indigo-600 transition duration-300" onClick={registerHandler}>
            Sign up
          </button>
        </form>
      </div>
    </div>
  )
}
