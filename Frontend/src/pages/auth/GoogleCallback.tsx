import { useEffect, useState } from 'react'
import { useNavigate, useSearchParams } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '../../components/ui/card'
import { Alert, AlertDescription } from '../../components/ui/alert'

export const GoogleCallback = () => {
    const navigate = useNavigate()
    const [searchParams] = useSearchParams()
    const { loginWithGoogle } = useAuth()
    const [error, setError] = useState<string>('')

    useEffect(() => {
        const handleGoogleCallback = async () => {
            const code = searchParams.get('code')
            const errorParam = searchParams.get('error')

            if (errorParam) {
                setError('Error en la autenticación con Google')
                setTimeout(() => navigate('/login'), 3000)
                return
            }

            if (!code) {
                setError('No se recibió código de autorización')
                setTimeout(() => navigate('/login'), 3000)
                return
            }

            try {
                // El código de Google debe ser intercambiado por un token en el backend
                // Por ahora usamos el código directamente (el backend debe manejarlo)
                await loginWithGoogle(code)
                navigate('/')
            } catch (err: any) {
                console.error('Google login error:', err)
                setError(err.response?.data?.message || 'Error al iniciar sesión con Google')
                setTimeout(() => navigate('/login'), 3000)
            }
        }

        handleGoogleCallback()
    }, [searchParams, loginWithGoogle, navigate])

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-50">
            <Card className="w-full max-w-md">
                <CardHeader className="space-y-1">
                    <CardTitle className="text-2xl font-bold text-center">
                        Autenticando con Google
                    </CardTitle>
                    <CardDescription className="text-center">Por favor espere...</CardDescription>
                </CardHeader>

                <CardContent className="flex flex-col items-center space-y-4">
                    {error ? (
                        <Alert variant="destructive">
                            <AlertDescription>{error}</AlertDescription>
                        </Alert>
                    ) : (
                        <div className="flex flex-col items-center space-y-4">
                            <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600"></div>
                            <p className="text-sm text-gray-600">Procesando autenticación...</p>
                        </div>
                    )}
                </CardContent>
            </Card>
        </div>
    )
}
