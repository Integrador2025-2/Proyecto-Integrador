import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'

type LoginState = 'credentials' | 'verification'

export default function LoginPage() {
    const navigate = useNavigate()
    const { initLogin, verifyTwoFactor, loginWithGoogleRedirect, isLoading, error } = useAuth()

    const [state, setState] = useState<LoginState>('credentials')
    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [code, setCode] = useState('')
    const [twoFactorToken, setTwoFactorToken] = useState('')
    const [maskedDestination, setMaskedDestination] = useState('')

    const handleCredentialsSubmit = async (e: React.FormEvent) => {
        e.preventDefault()

        try {
            const response = await initLogin({ email, password })
            setTwoFactorToken(response.twoFactorToken)
            setMaskedDestination(response.maskedDestination)
            setState('verification')
        } catch (err) {
            console.error('Login init failed:', err)
        }
    }

    const handleVerificationSubmit = async (e: React.FormEvent) => {
        e.preventDefault()

        try {
            await verifyTwoFactor(twoFactorToken, code)
            navigate('/dashboard')
        } catch (err) {
            console.error('Verification failed:', err)
        }
    }

    const handleBackToCredentials = () => {
        setState('credentials')
        setCode('')
        setTwoFactorToken('')
        setMaskedDestination('')
    }

    const handleCodeChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        const value = e.target.value.replace(/\D/g, '').slice(0, 6)
        setCode(value)
    }

    const handleGoogleLogin = async () => {
        try {
            const response = await fetch(`${import.meta.env.VITE_API_URL}/auth/google-auth-url`)
            const data = await response.json()

            // DEBUG: ver qué URL está devolviendo el backend
            console.log('authUrl desde backend:', data.authUrl)

            window.location.href = data.authUrl
        } catch (error) {
            console.error('Error obteniendo Google auth URL', error)
        }
    }


    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-indigo-100 px-4">
            <div className="max-w-md w-full bg-white rounded-2xl shadow-xl p-8">
                <div className="text-center mb-8">
                    <h1 className="text-3xl font-bold text-gray-900">
                        {state === 'credentials' ? 'Iniciar Sesión' : 'Verificación 2FA'}
                    </h1>
                    <p className="text-gray-600 mt-2">
                        {state === 'credentials'
                            ? 'Ingrese sus credenciales para acceder al sistema'
                            : `Ingrese el código enviado a ${maskedDestination}`}
                    </p>
                </div>

                {error && (
                    <div className="mb-4 p-3 bg-red-50 border border-red-200 rounded-lg text-red-700 text-sm">
                        {error}
                    </div>
                )}

                {state === 'credentials' ? (
                    <form onSubmit={handleCredentialsSubmit} className="space-y-6">
                        <div>
                            <label
                                htmlFor="email"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Correo Electrónico
                            </label>
                            <input
                                id="email"
                                type="email"
                                value={email}
                                onChange={(e) => setEmail(e.target.value)}
                                required
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                                placeholder="usuario@email.com"
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
                                type="password"
                                value={password}
                                onChange={(e) => setPassword(e.target.value)}
                                required
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition"
                                placeholder="••••••••"
                            />
                        </div>

                        <button
                            type="submit"
                            disabled={isLoading}
                            className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isLoading ? 'Procesando...' : 'Continuar'}
                        </button>

                        <button
                            type="button"
                            onClick={handleGoogleLogin}
                            disabled={isLoading}
                            className="mt-4 w-full flex items-center justify-center gap-2 border border-gray-300 py-3 rounded-lg font-semibold text-gray-700 hover:bg-gray-50 transition disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            <span className="text-lg">G</span>
                            <span>Continuar con Google</span>
                        </button>
                    </form>
                ) : (
                    <form onSubmit={handleVerificationSubmit} className="space-y-6">
                        <div>
                            <label
                                htmlFor="code"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Código de Verificación
                            </label>
                            <input
                                id="code"
                                type="text"
                                value={code}
                                onChange={handleCodeChange}
                                required
                                maxLength={6}
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent transition text-center text-2xl font-mono tracking-widest"
                                placeholder="000000"
                                autoComplete="off"
                            />
                            <p className="mt-2 text-xs text-gray-500 text-center">
                                Ingrese el código de 6 dígitos
                            </p>
                        </div>

                        <button
                            type="submit"
                            disabled={isLoading || code.length !== 6}
                            className="w-full bg-blue-600 text-white py-3 rounded-lg font-semibold hover:bg-blue-700 transition disabled:opacity-50 disabled:cursor-not-allowed"
                        >
                            {isLoading ? 'Verificando...' : 'Verificar Código'}
                        </button>

                        <button
                            type="button"
                            onClick={handleBackToCredentials}
                            className="w-full text-gray-600 py-2 text-sm hover:text-gray-800 transition"
                        >
                            ← Volver al inicio de sesión
                        </button>
                    </form>
                )}

                {state === 'credentials' && (
                    <>
                        <div className="mt-6 text-center">
                            <p className="text-sm text-gray-600">
                                ¿No tiene una cuenta?{' '}
                                <a
                                    href="/register"
                                    className="text-blue-600 hover:text-blue-700 font-semibold"
                                >
                                    Regístrese aquí
                                </a>
                            </p>
                        </div>

                        <div className="mt-4 p-3 bg-blue-50 rounded-lg">
                            <p className="text-xs text-blue-800 font-semibold mb-1">
                                💡 Usuarios de prueba:
                            </p>
                            <p className="text-xs text-blue-700">
                                • juan.perez@email.com (Administrador)
                            </p>
                            <p className="text-xs text-blue-700">
                                • maria.gonzalez@email.com (Investigador)
                            </p>
                            <p className="text-xs text-blue-700 mt-2">
                                <span className="font-semibold">Nota:</span> Revisa la consola del
                                navegador para ver el código 2FA
                            </p>
                        </div>
                    </>
                )}
            </div>
        </div>
    )
}
