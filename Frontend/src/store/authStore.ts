import { create } from 'zustand'
import type { User, AuthResponse } from '../types'

interface AuthState {
    user: User | null
    token: string | null
    refreshToken: string | null
    isAuthenticated: boolean
    isInitialized: boolean // ✅ NUEVO

    // Actions
    setAuth: (authData: AuthResponse) => void
    clearAuth: () => void
    updateUser: (user: User) => void
    initializeAuth: () => void
}

const AUTH_TOKEN_KEY = 'auth_token'
const REFRESH_TOKEN_KEY = 'refresh_token'
const USER_KEY = 'user'

export const useAuthStore = create<AuthState>((set) => ({
    user: null,
    token: null,
    refreshToken: null,
    isAuthenticated: false,
    isInitialized: false, // ✅ NUEVO

    // Guardar datos de autenticación después del login
    setAuth: (authData: AuthResponse) => {
        localStorage.setItem(AUTH_TOKEN_KEY, authData.token)
        localStorage.setItem(REFRESH_TOKEN_KEY, authData.refreshToken)
        localStorage.setItem(USER_KEY, JSON.stringify(authData.user))

        set({
            user: authData.user,
            token: authData.token,
            refreshToken: authData.refreshToken,
            isAuthenticated: true,
            isInitialized: true, // ✅ NUEVO
        })
    },

    // Limpiar datos de autenticación al logout
    clearAuth: () => {
        localStorage.removeItem(AUTH_TOKEN_KEY)
        localStorage.removeItem(REFRESH_TOKEN_KEY)
        localStorage.removeItem(USER_KEY)

        set({
            user: null,
            token: null,
            refreshToken: null,
            isAuthenticated: false,
            isInitialized: true, // ✅ Mantener true para no volver a inicializar
        })
    },

    // Actualizar información del usuario
    updateUser: (user: User) => {
        localStorage.setItem(USER_KEY, JSON.stringify(user))
        set({ user })
    },

    // Inicializar autenticación desde localStorage al cargar la app
    initializeAuth: () => {
        const token = localStorage.getItem(AUTH_TOKEN_KEY)
        const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)
        const userStr = localStorage.getItem(USER_KEY)

        if (token && userStr) {
            try {
                const user = JSON.parse(userStr) as User
                set({
                    user,
                    token,
                    refreshToken,
                    isAuthenticated: true,
                    isInitialized: true, // ✅ NUEVO
                })
            } catch (error) {
                console.error('Error parsing user from localStorage:', error)
                localStorage.removeItem(AUTH_TOKEN_KEY)
                localStorage.removeItem(REFRESH_TOKEN_KEY)
                localStorage.removeItem(USER_KEY)
                set({
                    user: null,
                    token: null,
                    refreshToken: null,
                    isAuthenticated: false,
                    isInitialized: true, // ✅ NUEVO
                })
            }
        } else {
            set({
                isInitialized: true,
            })
        }
    },
}))
