// src/services/AuthService.ts

import api from './api'
import StorageService from './StorageService'
import type {
    LoginCredentials,
    RegisterData,
    AuthResponse,
    User,
} from '@/models/business/user.model'
import axios from 'axios'

const GOOGLE_CLIENT_ID = import.meta.env.VITE_GOOGLE_CLIENT_ID
const GOOGLE_CLIENT_SECRET = import.meta.env.VITE_GOOGLE_SECRET_KEY

class AuthService {
    /**
     * Login con email y password
     */
    async login(credentials: LoginCredentials): Promise<AuthResponse> {
        try {
            const response = await api.post<AuthResponse>('/auth/login', credentials)
            const { token, user } = response.data

            // Guardar token y usuario en localStorage
            StorageService.set('token', token)
            StorageService.set('user', user)

            return response.data
        } catch (error) {
            console.error('Error en login:', error)
            throw error
        }
    }

    /**
     * Registro de nuevo usuario
     */
    async register(data: RegisterData): Promise<AuthResponse> {
        try {
            const response = await api.post<AuthResponse>('/auth/register', data)
            const { token, user } = response.data

            // Guardar token y usuario en localStorage
            StorageService.set('token', token)
            StorageService.set('user', user)

            return response.data
        } catch (error) {
            console.error('Error en registro:', error)
            throw error
        }
    }

    /**
     * Logout - Limpia el almacenamiento local
     */
    logout(): void {
        StorageService.remove('token')
        StorageService.remove('user')
    }

    /**
     * Obtiene el usuario actual desde localStorage
     */
    getCurrentUser(): User | null {
        return StorageService.get<User>('user')
    }

    /**
     * Obtiene el token actual desde localStorage
     */
    getToken(): string | null {
        return StorageService.get<string>('token')
    }

    /**
     * Verifica si el usuario está autenticado
     */
    isAuthenticated(): boolean {
        return this.getToken() !== null
    }

    /**
     * Inicia el flujo de autenticación con Google OAuth
     */
    initiateGoogleLogin(): void {
        const googleAuthUrl = 'https://accounts.google.com/o/oauth2/v2/auth'
        const redirectUri = `${window.location.origin}/auth/google/callback`

        const params = {
            client_id: GOOGLE_CLIENT_ID,
            redirect_uri: redirectUri,
            response_type: 'code',
            scope: 'openid email profile',
            access_type: 'offline',
            prompt: 'consent',
        }

        const queryString = Object.entries(params)
            .map(([key, value]) => `${key}=${encodeURIComponent(value)}`)
            .join('&')

        window.location.href = `${googleAuthUrl}?${queryString}`
    }

    /**
     * Obtiene la información del usuario de Google usando el código de autorización
     */
    async getGoogleUserInfo(code: string) {
        try {
            // Intercambiar el código por un token de acceso
            const tokenResponse = await axios.post('https://oauth2.googleapis.com/token', {
                code,
                client_id: GOOGLE_CLIENT_ID,
                client_secret: GOOGLE_CLIENT_SECRET,
                redirect_uri: `${window.location.origin}/auth/google/callback`,
                grant_type: 'authorization_code',
            })

            const { access_token } = tokenResponse.data

            // Obtener información del usuario con el token
            const userInfoResponse = await axios.get(
                'https://www.googleapis.com/oauth2/v2/userinfo',
                {
                    headers: {
                        Authorization: `Bearer ${access_token}`,
                    },
                },
            )

            return userInfoResponse.data
        } catch (error) {
            console.error('Error al obtener información del usuario de Google:', error)
            throw error
        }
    }

    /**
     * Autentica al usuario con Google en tu backend
     * (Envía la info de Google a tu API para crear/obtener el usuario)
     */
    async loginWithGoogle(googleUserData: unknown): Promise<AuthResponse> {
        try {
            const response = await api.post<AuthResponse>('/auth/google', googleUserData)
            const { token, user } = response.data

            // Guardar token y usuario en localStorage
            StorageService.set('token', token)
            StorageService.set('user', user)

            return response.data
        } catch (error) {
            console.error('Error en login con Google:', error)
            throw error
        }
    }
}

export default new AuthService()
