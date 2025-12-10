// src/globalStates/useUserStore.ts

import { create } from 'zustand'
import type { User } from '@/models/business/user.model'
import { AuthService } from '@/services/auth.service'

interface UserState {
    // Estado
    user: User | null
    isAuthenticated: boolean
    isLoading: boolean

    // Acciones
    setUser: (user: User | null) => void
    loadUser: () => void
    logout: () => void
    clearUser: () => void
}

/**
 * Store global de usuario usando Zustand
 * Maneja el estado de autenticación del usuario en toda la aplicación
 */
export const useUserStore = create<UserState>((set) => ({
    // Estado inicial
    user: null,
    isAuthenticated: false,
    isLoading: true,

    /**
     * Establece el usuario actual
     */
    setUser: (user) =>
        set({
            user,
            isAuthenticated: user !== null,
            isLoading: false,
        }),

    /**
     * Carga el usuario desde localStorage al iniciar la aplicación
     */
    loadUser: () => {
        const user = AuthService.getCurrentUser()
        const isAuthenticated = AuthService.isAuthenticated()

        set({
            user,
            isAuthenticated,
            isLoading: false,
        })
    },

    /**
     * Cierra sesión y limpia el estado
     */
    logout: () => {
        AuthService.logout()
        set({
            user: null,
            isAuthenticated: false,
            isLoading: false,
        })
    },

    /**
     * Limpia el usuario sin hacer logout en el servicio
     * (útil para resetear el estado sin tocar localStorage)
     */
    clearUser: () =>
        set({
            user: null,
            isAuthenticated: false,
            isLoading: false,
        }),
}))
