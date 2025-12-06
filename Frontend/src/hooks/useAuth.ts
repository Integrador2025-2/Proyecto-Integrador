import { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { authServiceMock } from '../mocks/auth.service.mock'
import { authService } from '../services/auth.service'
import { useAuthStore } from '../store/authStore'
import type { User, TwoFactorInitResponse, AuthResponse } from '../types'

interface LoginCredentials {
    email: string
    password: string
}

interface RegisterData {
    firstName: string
    lastName: string
    email: string
    password: string
    roleId?: number
}

// Seleccionar servicio según variable de entorno
const USE_MOCK = import.meta.env.VITE_USE_MOCK === 'true'
const activeAuthService = USE_MOCK ? authServiceMock : authService

console.log('🔧 Auth Mode:', USE_MOCK ? 'MOCK' : 'REAL BACKEND')

export const useAuth = () => {
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    // Zustand store
    const {
        user,
        isAuthenticated,
        setAuth,
        clearAuth,
        updateUser: updateUserStore,
    } = useAuthStore()

    /**
     * Inicia el proceso de login 2FA (Paso 1)
     */
    const initLogin = async (credentials: LoginCredentials): Promise<TwoFactorInitResponse> => {
        setIsLoading(true)
        setError(null)
        try {
            const response = await activeAuthService.initLogin(
                credentials.email,
                credentials.password,
            )
            return response
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al iniciar sesión'
            setError(errorMessage)
            throw err
        } finally {
            setIsLoading(false)
        }
    }

    /**
     * Verifica el código 2FA (Paso 2)
     */
    const verifyTwoFactor = async (twoFactorToken: string, code: string): Promise<AuthResponse> => {
        setIsLoading(true)
        setError(null)
        try {
            const response = await activeAuthService.verifyTwoFactor(twoFactorToken, code)

            // Actualizar el store con los datos de autenticación
            setAuth(response)

            return response
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Código incorrecto'
            setError(errorMessage)
            throw err
        } finally {
            setIsLoading(false)
        }
    }

    /**
     * Registra un nuevo usuario
     */
    const register = async (userData: RegisterData): Promise<AuthResponse> => {
        setIsLoading(true)
        setError(null)
        try {
            const response = await activeAuthService.register(
                userData.firstName,
                userData.lastName,
                userData.email,
                userData.password,
                userData.roleId,
            )

            // Actualizar el store con los datos de autenticación
            setAuth(response)

            return response
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al registrarse'
            setError(errorMessage)
            throw err
        } finally {
            setIsLoading(false)
        }
    }

    /**
     * Cierra sesión
     */
    const logout = async (): Promise<void> => {
        setIsLoading(true)
        try {
            await activeAuthService.logout()

            // Limpiar el store
            clearAuth()

            navigate('/login')
        } catch (err) {
            console.error('Logout error:', err)
        } finally {
            setIsLoading(false)
        }
    }

    /**
     * Obtiene el usuario actual
     */
    const getCurrentUser = async (): Promise<User | null> => {
        try {
            return await activeAuthService.getCurrentUser()
        } catch (err) {
            console.error('Get current user error:', err)
            return null
        }
    }

    /**
     * Actualiza el usuario
     */
    const updateUser = async (userId: number, updates: Partial<User>): Promise<User | null> => {
        setIsLoading(true)
        setError(null)
        try {
            const updatedUser = await activeAuthService.updateUser(userId, updates)

            // Actualizar el store
            updateUserStore(updatedUser)

            return updatedUser
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al actualizar usuario'
            setError(errorMessage)
            throw err
        } finally {
            setIsLoading(false)
        }
    }

    /**
     * Cambia la contraseña
     */
    const changePassword = async (currentPassword: string, newPassword: string): Promise<void> => {
        setIsLoading(true)
        setError(null)
        try {
            await activeAuthService.changePassword(currentPassword, newPassword)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al cambiar contraseña'
            setError(errorMessage)
            throw err
        } finally {
            setIsLoading(false)
        }
    }

    return {
        // Estado del store
        user,
        isAuthenticated,

        // Métodos de autenticación
        initLogin,
        verifyTwoFactor,
        register,
        logout,
        getCurrentUser,
        updateUser,
        changePassword,

        // Estado de carga y errores
        isLoading,
        error,
    }
}
