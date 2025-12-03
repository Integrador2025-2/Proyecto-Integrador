import api from './api'
import type {
    LoginRequest,
    RegisterRequest,
    AuthResponse,
    RefreshTokenRequest,
    GoogleAuthRequest,
    ChangePasswordRequest,
    User,
} from '../types/auth.types'

class AuthService {
    private readonly TOKEN_KEY = 'access_token'
    private readonly REFRESH_TOKEN_KEY = 'refresh_token'
    private readonly USER_KEY = 'user'

    /**
     * Iniciar sesión con email y contraseña
     */
    async login(credentials: LoginRequest): Promise<AuthResponse> {
        const response = await api.post<AuthResponse>('/auth/init', credentials)
        this.setAuthData(response.data)
        return response.data
    }

    /**
     * Registrar nuevo usuario
     */
    async register(userData: RegisterRequest): Promise<AuthResponse> {
        const response = await api.post<AuthResponse>('/auth/register', userData)
        this.setAuthData(response.data)
        return response.data
    }

    /**
     * Iniciar sesión con Google OAuth
     */
    async loginWithGoogle(googleToken: string): Promise<AuthResponse> {
        const request: GoogleAuthRequest = { googleToken }
        const response = await api.post<AuthResponse>('/auth/google-login', request)
        this.setAuthData(response.data)
        return response.data
    }

    /**
     * Renovar token de acceso
     */
    async refreshToken(): Promise<AuthResponse> {
        const refreshToken = this.getRefreshToken()
        if (!refreshToken) {
            throw new Error('No refresh token available')
        }

        const request: RefreshTokenRequest = { refreshToken }
        const response = await api.post<AuthResponse>('/auth/refresh-token', request)
        this.setAuthData(response.data)
        return response.data
    }

    /**
     * Cerrar sesión
     */
    async logout(): Promise<void> {
        const refreshToken = this.getRefreshToken()

        if (refreshToken) {
            try {
                const request: RefreshTokenRequest = { refreshToken }
                await api.post('/auth/logout', request)
            } catch (error) {
                console.error('Error during logout:', error)
            }
        }

        this.clearAuthData()
    }

    /**
     * Cambiar contraseña del usuario actual
     */
    async changePassword(passwords: ChangePasswordRequest): Promise<void> {
        await api.post('/auth/change-password', passwords)
    }

    /**
     * Obtener información del usuario actual
     */
    async getCurrentUser(): Promise<User> {
        const response = await api.get<User>('/auth/me')
        this.setUser(response.data)
        return response.data
    }

    /**
     * Obtener URL de autenticación con Google
     */
    async getGoogleAuthUrl(): Promise<string> {
        const response = await api.get<{ authUrl: string }>('/auth/google-auth-url')
        return response.data.authUrl
    }

    // Métodos de manejo de tokens y datos locales

    private setAuthData(authResponse: AuthResponse): void {
        localStorage.setItem(this.TOKEN_KEY, authResponse.token)
        localStorage.setItem(this.REFRESH_TOKEN_KEY, authResponse.refreshToken)
        localStorage.setItem(this.USER_KEY, JSON.stringify(authResponse.user))
    }

    private clearAuthData(): void {
        localStorage.removeItem(this.TOKEN_KEY)
        localStorage.removeItem(this.REFRESH_TOKEN_KEY)
        localStorage.removeItem(this.USER_KEY)
    }

    private setUser(user: User): void {
        localStorage.setItem(this.USER_KEY, JSON.stringify(user))
    }

    getAccessToken(): string | null {
        return localStorage.getItem(this.TOKEN_KEY)
    }

    getRefreshToken(): string | null {
        return localStorage.getItem(this.REFRESH_TOKEN_KEY)
    }

    getUser(): User | null {
        const userStr = localStorage.getItem(this.USER_KEY)
        if (!userStr) return null

        try {
            return JSON.parse(userStr)
        } catch {
            return null
        }
    }

    isAuthenticated(): boolean {
        return !!this.getAccessToken()
    }

    hasRole(roleNames: string | string[]): boolean {
        const user = this.getUser()
        if (!user) return false

        const roles = Array.isArray(roleNames) ? roleNames : [roleNames]
        return roles.includes(user.roleName)
    }
}

export default new AuthService()
