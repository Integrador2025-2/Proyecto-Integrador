import { Navigate, Outlet } from 'react-router-dom'
import { useAuthStore } from '../../store/authStore'

interface PrivateRouteProps {
    requiredRoles?: string[]
}

export const PrivateRoute = ({ requiredRoles }: PrivateRouteProps) => {
    const { isAuthenticated, user, isLoading } = useAuthStore()

    if (isLoading) {
        return (
            <div className="flex items-center justify-center min-h-screen">
                <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-gray-900"></div>
            </div>
        )
    }

    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }

    if (requiredRoles && requiredRoles.length > 0) {
        const hasRequiredRole = user && requiredRoles.includes(user.roleName)

        if (!hasRequiredRole) {
            return <Navigate to="/unauthorized" replace />
        }
    }

    return <Outlet />
}
