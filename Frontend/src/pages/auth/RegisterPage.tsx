import { useState, FormEvent } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'

export default function RegisterPage() {
    const navigate = useNavigate()
    const { register, isLoading } = useAuth()

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password: '',
        confirmPassword: '',
    })

    const [error, setError] = useState<string>('')

    const validatePassword = (password: string): string | null => {
        if (password.length < 8) {
            return 'La contraseña debe tener al menos 8 caracteres'
        }
        if (!/[A-Z]/.test(password)) {
            return 'La contraseña debe contener al menos una mayúscula'
        }
        if (!/[a-z]/.test(password)) {
            return 'La contraseña debe contener al menos una minúscula'
        }
        if (!/[0-9]/.test(password)) {
            return 'La contraseña debe contener al menos un número'
        }
        return null
    }

    const handleSubmit = async (e: FormEvent) => {
        e.preventDefault()
        setError('')

        if (
            !formData.firstName ||
            !formData.lastName ||
            !formData.email ||
            !formData.password ||
            !formData.confirmPassword
        ) {
            setError('Por favor complete todos los campos')
            return
        }

        if (formData.password !== formData.confirmPassword) {
            setError('Las contraseñas no coinciden')
            return
        }

        const passwordError = validatePassword(formData.password)
        if (passwordError) {
            setError(passwordError)
            return
        }

        try {
            await register({
                firstName: formData.firstName,
                lastName: formData.lastName,
                email: formData.email,
                password: formData.password,
                roleId: 2, // Usuario por defecto
            })
            navigate('/dashboard')
        } catch (err: any) {
            console.error('Register error:', err)
            setError(
                err.response?.data?.message ||
                    err.response?.data ||
                    'Error al crear la cuenta. Por favor intente nuevamente.',
            )
        }
    }

    const handleInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const { name, value } = e.target
        setFormData((prev) => ({ ...prev, [name]: value }))
        if (error) setError('')
    }

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8">
                <div className="text-center mb-8">
                    <h1 className="text-3xl font-bold text-gray-900">Crear Cuenta</h1>
                    <p className="text-gray-600 mt-2">Complete el formulario para registrarse</p>
                </div>

                {error && (
                    <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-lg text-red-700 text-sm">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="grid grid-cols-2 gap-4">
                        <div>
                            <label
                                htmlFor="firstName"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Nombre
                            </label>
                            <input
                                id="firstName"
                                name="firstName"
                                type="text"
                                placeholder="Juan"
                                value={formData.firstName}
                                onChange={handleInputChange}
                                disabled={isLoading}
                                required
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                            />
                        </div>

                        <div>
                            <label
                                htmlFor="lastName"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Apellido
                            </label>
                            <input
                                id="lastName"
                                name="lastName"
                                type="text"
                                placeholder="Pérez"
                                value={formData.lastName}
                                onChange={handleInputChange}
                                disabled={isLoading}
                                required
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                            />
                        </div>
                    </div>

                    <div>
                        <label
                            htmlFor="email"
                            className="block text-sm font-medium text-gray-700 mb-2"
                        >
                            Correo Electrónico
                        </label>
                        <input
                            id="email"
                            name="email"
                            type="email"
                            placeholder="correo@ejemplo.com"
                            value={formData.email}
                            onChange={handleInputChange}
                            disabled={isLoading}
                            required
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                        />
                    </div>

                    <div>
                        <label
                            htmlFor="password"
                            className="block text-sm font-medium text-gray-700 mb-2"
                        >
                            Contraseña
                        </label>
                        <input
                            id="password"
                            name="password"
                            type="password"
                            placeholder="••••••••"
                            value={formData.password}
                            onChange={handleInputChange}
                            disabled={isLoading}
                            required
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                        />
                        <p className="text-xs text-gray-500 mt-1">
                            Mínimo 8 caracteres, incluir mayúsculas, minúsculas y números
                        </p>
                    </div>

                    <div>
                        <label
                            htmlFor="confirmPassword"
                            className="block text-sm font-medium text-gray-700 mb-2"
                        >
                            Confirmar Contraseña
                        </label>
                        <input
                            id="confirmPassword"
                            name="confirmPassword"
                            type="password"
                            placeholder="••••••••"
                            value={formData.confirmPassword}
                            onChange={handleInputChange}
                            disabled={isLoading}
                            required
                            className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition disabled:opacity-50 disabled:cursor-not-allowed mt-6"
                    >
                        {isLoading ? 'Creando cuenta...' : 'Crear Cuenta'}
                    </button>
                </form>

                <div className="mt-6 text-center">
                    <p className="text-sm text-gray-600">
                        ¿Ya tiene una cuenta?{' '}
                        <Link to="/login" className="text-blue-600 hover:text-blue-700 font-semibold">
                            Inicie sesión aquí
                        </Link>
                    </p>
                </div>
            </div>
        </div>
    )
}
