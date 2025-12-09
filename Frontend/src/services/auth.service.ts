import type {
    AuthResponse,
    TwoFactorInitResponse,
    User,
    BackendAuthResponse,
    BackendUserDto,
    BackendTwoFactorInitResponse,
    BackendErrorResponse,
} from '../types'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5043/api'

const AUTH_TOKEN_KEY = 'auth_token'
const REFRESH_TOKEN_KEY = 'refresh_token'
const USER_KEY = 'user'

class AuthService {
    /**
     * Simula un pequeño delay de red
     */
    private async simulateNetworkDelay(minMs: number = 100, maxMs: number = 300): Promise<void> {
        const delay = Math.random() * (maxMs - minMs) + minMs
        return new Promise((resolve) => setTimeout(resolve, delay))
    }

    /**
     * Paso 1: Inicia el proceso de login 2FA
     */
    async initLogin(email: string, password: string): Promise<TwoFactorInitResponse> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/auth/login/init`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ email, password }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Error al iniciar sesión' }))
            throw new Error(errorData.message || 'Credenciales inválidas')
        }

        const data: BackendTwoFactorInitResponse = await response.json()

        return {
            twoFactorRequired: data.twoFactorRequired,
            twoFactorToken: data.twoFactorToken,
            deliveryChannel: data.deliveryChannel,
            maskedDestination: data.maskedDestination,
        }
    }

    /**
     * Paso 2: Verifica el código 2FA y completa el login
     */
    async verifyTwoFactor(twoFactorToken: string, code: string): Promise<AuthResponse> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/auth/2fa/verify`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ twoFactorToken, code }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Código incorrecto' }))
            throw new Error(errorData.message || 'Código incorrecto')
        }

        const data: BackendAuthResponse = await response.json()

        const authResponse: AuthResponse = {
            token: data.token,
            refreshToken: data.refreshToken,
            expiresAt: data.expiresAt,
            user: this.mapUserFromBackend(data.user),
        }

        this.saveAuthData(authResponse)
        return authResponse
    }

    /**
     * Registra un nuevo usuario
     */
    async register(
        firstName: string,
        lastName: string,
        email: string,
        password: string,
        roleId: number = 2,
    ): Promise<AuthResponse> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/auth/register`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ firstName, lastName, email, password, roleId }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Error al registrarse' }))
            throw new Error(errorData.message || 'Error al registrarse')
        }

        const data: BackendAuthResponse = await response.json()

        const authResponse: AuthResponse = {
            token: data.token,
            refreshToken: data.refreshToken,
            expiresAt: data.expiresAt,
            user: this.mapUserFromBackend(data.user),
        }

        this.saveAuthData(authResponse)
        return authResponse
    }

    /**
     * Cierra sesión
     */
    async logout(): Promise<void> {
        const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)

        if (refreshToken) {
            try {
                await fetch(`${API_URL}/auth/logout`, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${this.getToken()}`,
                    },
                    body: JSON.stringify({ refreshToken }),
                })
            } catch (error) {
                console.error('Error during logout:', error)
            }
        }

        localStorage.removeItem(AUTH_TOKEN_KEY)
        localStorage.removeItem(REFRESH_TOKEN_KEY)
        localStorage.removeItem(USER_KEY)
    }

    /**
     * Obtiene el usuario actual desde el backend
     */
    async getCurrentUser(): Promise<User | null> {
        const token = this.getToken()
        if (!token) {
            return null
        }

        try {
            const response = await fetch(`${API_URL}/auth/me`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })

            if (!response.ok) {
                return null
            }

            const data: BackendUserDto = await response.json()
            return this.mapUserFromBackend(data)
        } catch (error) {
            console.error('Error getting current user:', error)
            return null
        }
    }

    /**
     * Actualiza información del usuario
     */
    async updateUser(userId: number, updates: Partial<User>): Promise<User> {
        const token = this.getToken()
        if (!token) {
            throw new Error('No autenticado')
        }

        const backendUpdates: Record<string, any> = { Id: userId }

        if (updates.firstName !== undefined) backendUpdates.FirstName = updates.firstName
        if (updates.lastName !== undefined) backendUpdates.LastName = updates.lastName
        if (updates.email !== undefined) backendUpdates.Email = updates.email
        if (updates.isActive !== undefined) backendUpdates.IsActive = updates.isActive
        if (updates.roleId !== undefined) backendUpdates.RoleId = updates.roleId

        const response = await fetch(`${API_URL}/users/${userId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify(backendUpdates),
        })

        if (!response.ok) {
            throw new Error('Error al actualizar usuario')
        }

        const data: BackendUserDto = await response.json()
        const updatedUser = this.mapUserFromBackend(data)

        localStorage.setItem(USER_KEY, JSON.stringify(updatedUser))
        return updatedUser
    }

    /**
     * Cambia la contraseña del usuario
     */
    async changePassword(currentPassword: string, newPassword: string): Promise<void> {
        const token = this.getToken()
        if (!token) {
            throw new Error('No autenticado')
        }

        const response = await fetch(`${API_URL}/auth/change-password`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({ currentPassword, newPassword }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Error al cambiar contraseña' }))
            throw new Error(errorData.message || 'Error al cambiar contraseña')
        }
    }

    /**
     * Refresca el token de autenticación
     */
    async refreshToken(): Promise<AuthResponse> {
        const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)
        if (!refreshToken) {
            throw new Error('No hay refresh token')
        }

        const response = await fetch(`${API_URL}/auth/refresh-token`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({ refreshToken }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Error al refrescar token' }))
            throw new Error(errorData.message || 'Error al refrescar token')
        }

        const data: BackendAuthResponse = await response.json()

        const authResponse: AuthResponse = {
            token: data.token,
            refreshToken: data.refreshToken,
            expiresAt: data.expiresAt,
            user: this.mapUserFromBackend(data.user),
        }

        this.saveAuthData(authResponse)
        return authResponse
    }

    /**
     * Login con Google - obtiene URL de autenticación
     */
    async getGoogleAuthUrl(): Promise<string> {
        const response = await fetch(`${API_URL}/auth/google-auth-url`, {
            method: 'GET',
        })

        if (!response.ok) {
            throw new Error('No se pudo obtener la URL de autenticación de Google')
        }

        const data = (await response.json()) as { authUrl: string }
        return data.authUrl
    }

    /**
     * Login con Google usando token
     */
    async loginWithGoogle(googleToken: string): Promise<AuthResponse> {
        const response = await fetch(`${API_URL}/auth/google-login`, {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify({ googleToken }),
        })

        if (!response.ok) {
            const errorData: BackendErrorResponse = await response
                .json()
                .catch(() => ({ message: 'Error al iniciar sesión con Google' }))
            throw new Error(errorData.message || 'Error al iniciar sesión con Google')
        }

        const data: BackendAuthResponse = await response.json()

        const authResponse: AuthResponse = {
            token: data.token,
            refreshToken: data.refreshToken,
            expiresAt: data.expiresAt,
            user: this.mapUserFromBackend(data.user),
        }

        this.saveAuthData(authResponse)
        return authResponse
    }

    /**
     * Verifica si el usuario está autenticado
     */
    isAuthenticated(): boolean {
        return !!this.getToken() && !!this.getUserFromStorage()
    }

    /**
     * Obtiene el token del localStorage
     */
    getToken(): string | null {
        return localStorage.getItem(AUTH_TOKEN_KEY)
    }

    /**
     * Obtiene el usuario del localStorage
     */
    getUser(): User | null {
        return this.getUserFromStorage()
    }

    // === Métodos privados ===

    /**
     * Mapea usuario del backend (PascalCase o camelCase) a frontend (camelCase)
     */
    private mapUserFromBackend(backendUser: BackendUserDto): User {
        return {
            id: backendUser.id,
            firstName: backendUser.firstName,
            lastName: backendUser.lastName,
            email: backendUser.email,
            fullName: backendUser.fullName,
            createdAt: backendUser.createdAt,
            updatedAt: backendUser.updatedAt,
            isActive: backendUser.isActive,
            roleId: backendUser.roleId,
            roleName: backendUser.roleName,
            provider: backendUser.provider,
            profilePictureUrl: backendUser.profilePictureUrl ?? undefined,
        }
    }

    /**
     * Guarda datos de autenticación en localStorage
     */
    private saveAuthData(response: AuthResponse): void {
        localStorage.setItem(AUTH_TOKEN_KEY, response.token)
        localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken)
        localStorage.setItem(USER_KEY, JSON.stringify(response.user))
    }

    /**
     * Obtiene usuario del localStorage
     */
    private getUserFromStorage(): User | null {
        const userStr = localStorage.getItem(USER_KEY)

        if (!userStr) {
            return null
        }

        try {
            return JSON.parse(userStr) as User
        } catch {
            return null
        }
    }
}

export const authService = new AuthService()
