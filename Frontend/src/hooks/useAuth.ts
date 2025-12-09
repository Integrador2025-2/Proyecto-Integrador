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

const USE_MOCK = import.meta.env.VITE_USE_MOCK === 'true'
const activeAuthService = USE_MOCK ? authServiceMock : authService
console.log('🔧 Auth Mode:', USE_MOCK ? 'MOCK' : 'REAL BACKEND')
const API_URL = import.meta.env.VITE_API_URL

export const useAuth = () => {
    const navigate = useNavigate()
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const {
        user,
        isAuthenticated,
        setAuth,
        clearAuth,
        updateUser: updateUserStore,
    } = useAuthStore()

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

    const verifyTwoFactor = async (twoFactorToken: string, code: string): Promise<AuthResponse> => {
        setIsLoading(true)
        setError(null)
        try {
            const response = await activeAuthService.verifyTwoFactor(twoFactorToken, code)
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

    const logout = async (): Promise<void> => {
        setIsLoading(true)
        try {
            await activeAuthService.logout()
            clearAuth()
            navigate('/login')
        } catch (err) {
            console.error('Logout error:', err)
        } finally {
            setIsLoading(false)
        }
    }

    const getCurrentUser = async (): Promise<User | null> => {
        try {
            return await activeAuthService.getCurrentUser()
        } catch (err) {
            console.error('Get current user error:', err)
            return null
        }
    }

    const updateUser = async (userId: number, updates: Partial<User>): Promise<User | null> => {
        setIsLoading(true)
        setError(null)
        try {
            const updatedUser = await activeAuthService.updateUser(userId, updates)
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

    // === Google Auth ===

    const loginWithGoogleRedirect = async (): Promise<void> => {
        setIsLoading(true)
        setError(null)
        try {
            const authUrl = await activeAuthService.getGoogleAuthUrl()
            window.location.href = authUrl
        } catch (err) {
            const errorMessage =
                err instanceof Error
                    ? err.message
                    : 'Error al redirigir a Google para autenticación'
            setError(errorMessage)
            setIsLoading(false)
        }
    }

    const handleGoogleCallback = async (code: string) => {
        console.log('API_URL:', API_URL)
        console.log('Google login URL:', `${API_URL}/auth/google-login`)
        console.log('Google code:', code)

        const response = await fetch(`${API_URL}/auth/google-login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ googleToken: code }),
        })

        if (!response.ok) {
            console.error('Google login status:', response.status)
            throw new Error('Error en login con Google')
        }

        const data: AuthResponse = await response.json()
        setAuth(data) // aquí tu función de Zustand para guardar user/token
        navigate('/dashboard')
    }

    return {
        user,
        isAuthenticated,

        initLogin,
        verifyTwoFactor,
        register,
        logout,
        getCurrentUser,
        updateUser,
        changePassword,

        loginWithGoogleRedirect,
        handleGoogleCallback,

        isLoading,
        error,
    }
}
