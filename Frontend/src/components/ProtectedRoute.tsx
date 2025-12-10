import { Navigate } from 'react-router-dom'
import { useAuthStore } from '../store/authStore'

interface ProtectedRouteProps {
    children: React.ReactNode
}

export default function ProtectedRoute({ children }: ProtectedRouteProps) {
    const isAuthenticated = useAuthStore((state) => state.isAuthenticated)
    const isInitialized = useAuthStore((state) => state.isInitialized)

    // Mostrar loading mientras se inicializa el store
    if (!isInitialized) {
        return (
            <div className="min-h-screen flex items-center justify-center bg-gray-50">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Cargando...</p>
                </div>
            </div>
        )
    }

    // Si ya inicializó y no está autenticado, redirigir a login
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }

    // Usuario autenticado, mostrar contenido protegido
    return <>{children}</>
}
