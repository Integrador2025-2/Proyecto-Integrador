import type { AuthResponse, User, TwoFactorInitResponse } from '../types'
import { mockLogin, mockRegister, mockGetCurrentUser, mockUsers } from '../mocks/auth.mock'

const AUTH_TOKEN_KEY = 'auth_token'
const REFRESH_TOKEN_KEY = 'refresh_token'
const USER_KEY = 'user'
const TEMP_TOKEN_KEY = 'temp_2fa_token'
const TWO_FA_CODE_KEY = 'temp_2fa_code'

class AuthServiceMock {
    /**
     * Inicia el proceso de login 2FA (Paso 1)
     */
    async initLogin(email: string): Promise<TwoFactorInitResponse> {
        await this.simulateNetworkDelay()

        const response = mockLogin(email)

        if (!response) {
            throw new Error('Credenciales inválidas')
        }

        // Generar código de 6 dígitos
        const code = this.generateCode()
        const tempToken = this.generateTempToken()

        // Guardar temporalmente (simula Redis)
        sessionStorage.setItem(TEMP_TOKEN_KEY, tempToken)
        sessionStorage.setItem(TWO_FA_CODE_KEY, code)
        sessionStorage.setItem('temp_user_id', response.user.id.toString())

        // Simular envío de email (mostrar en consola)
        console.log('━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━')
        console.log('📧 CÓDIGO 2FA GENERADO (SIMULATED EMAIL)')
        console.log(`Código: ${code}`)
        console.log(`Usuario: ${response.user.email}`)
        console.log(`Expira en: 10 minutos`)
        console.log('━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━')

        return {
            twoFactorRequired: true,
            twoFactorToken: tempToken,
            deliveryChannel: 'email',
            maskedDestination: this.maskEmail(response.user.email),
        }
    }

    /**
     * Verifica el código 2FA (Paso 2)
     */
    async verifyTwoFactor(twoFactorToken: string, code: string): Promise<AuthResponse> {
        await this.simulateNetworkDelay()

        const storedToken = sessionStorage.getItem(TEMP_TOKEN_KEY)
        const storedCode = sessionStorage.getItem(TWO_FA_CODE_KEY)
        const userIdStr = sessionStorage.getItem('temp_user_id')

        if (!storedToken || !storedCode || !userIdStr) {
            throw new Error('Código inválido o expirado')
        }

        if (storedToken !== twoFactorToken) {
            throw new Error('Token inválido')
        }

        if (storedCode !== code) {
            throw new Error('Código incorrecto')
        }

        // Obtener el usuario por ID
        const userId = parseInt(userIdStr)
        const user = mockUsers.find((u) => u.id === userId)

        if (!user) {
            throw new Error('Usuario no encontrado')
        }

        // Limpiar datos temporales
        sessionStorage.removeItem(TEMP_TOKEN_KEY)
        sessionStorage.removeItem(TWO_FA_CODE_KEY)
        sessionStorage.removeItem('temp_user_id')

        // Generar respuesta de autenticación
        const response: AuthResponse = {
            token: `mock-jwt-token-${user.id}-${Date.now()}`,
            refreshToken: `mock-refresh-token-${user.id}-${Date.now()}`,
            expiresAt: new Date(Date.now() + 3600000).toISOString(),
            user,
        }

        // Guardar en localStorage
        this.saveAuthData(response)

        return response
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

        const response = mockRegister(firstName, lastName, email, roleId)
        this.saveAuthData(response)

        return response
    }

    /**
     * Cierra sesión
     */
    async logout(): Promise<void> {
        await this.simulateNetworkDelay()

        localStorage.removeItem(AUTH_TOKEN_KEY)
        localStorage.removeItem(REFRESH_TOKEN_KEY)
        localStorage.removeItem(USER_KEY)
    }

    /**
     * Obtiene el usuario actual desde el token
     */
    async getCurrentUser(): Promise<User | null> {
        const token = this.getToken()

        if (!token) {
            return null
        }

        await this.simulateNetworkDelay()

        return mockGetCurrentUser(token)
    }

    /**
     * Verifica si el usuario está autenticado
     */
    isAuthenticated(): boolean {
        const token = this.getToken()
        const user = this.getUserFromStorage()

        return !!(token && user)
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

    /**
     * Actualiza la información del usuario
     */
    async updateUser(userId: number, updates: Partial<User>): Promise<User> {
        await this.simulateNetworkDelay()

        const currentUser = this.getUserFromStorage()

        if (!currentUser || currentUser.id !== userId) {
            throw new Error('Usuario no autorizado')
        }

        const updatedUser: User = {
            ...currentUser,
            ...updates,
            id: currentUser.id,
            updatedAt: new Date().toISOString(),
        }

        localStorage.setItem(USER_KEY, JSON.stringify(updatedUser))

        return updatedUser
    }

    /**
     * Cambia la contraseña del usuario
     */
    async changePassword(currentPassword: string, newPassword: string): Promise<void> {
        await this.simulateNetworkDelay()

        if (!this.isAuthenticated()) {
            throw new Error('Usuario no autenticado')
        }

        console.log('Password changed successfully', { currentPassword, newPassword })
    }

    /**
     * Refresca el token de autenticación
     */
    async refreshToken(): Promise<AuthResponse> {
        await this.simulateNetworkDelay()

        const refreshToken = localStorage.getItem(REFRESH_TOKEN_KEY)
        const user = this.getUserFromStorage()

        if (!refreshToken || !user) {
            throw new Error('No hay sesión activa')
        }

        const response: AuthResponse = {
            token: `mock-jwt-token-${user.id}-${Date.now()}`,
            refreshToken: `mock-refresh-token-${user.id}-${Date.now()}`,
            expiresAt: new Date(Date.now() + 3600000).toISOString(),
            user,
        }

        this.saveAuthData(response)

        return response
    }

    // === Métodos privados ===

    private saveAuthData(response: AuthResponse): void {
        localStorage.setItem(AUTH_TOKEN_KEY, response.token)
        localStorage.setItem(REFRESH_TOKEN_KEY, response.refreshToken)
        localStorage.setItem(USER_KEY, JSON.stringify(response.user))
    }

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

    private generateCode(): string {
        return Math.floor(100000 + Math.random() * 900000).toString()
    }

    private generateTempToken(): string {
        return `temp-2fa-${Date.now()}-${Math.random().toString(36).substring(7)}`
    }

    private maskEmail(email: string): string {
        const [name, domain] = email.split('@')
        const visibleChars = Math.min(2, name.length)
        const masked =
            name.substring(0, visibleChars) + '*'.repeat(Math.max(0, name.length - visibleChars))
        return `${masked}@${domain}`
    }

    private async simulateNetworkDelay(minMs: number = 300, maxMs: number = 800): Promise<void> {
        const delay = Math.random() * (maxMs - minMs) + minMs
        return new Promise((resolve) => setTimeout(resolve, delay))
    }
    async getGoogleAuthUrl(): Promise<string> {
        // URL falsa solo para desarrollo con mocks
        return 'https://accounts.google.com/o/oauth2/v2/auth?mock=1'
    }

    async loginWithGoogle(googleToken: string): Promise<AuthResponse> {
        console.log('🔁 Mock loginWithGoogle called with token:', googleToken)

        const mockUser: User = {
            id: 1,
            firstName: 'Mock',
            lastName: 'User',
            email: 'mock.google@sigpi.edu.co',
            fullName: 'Mock User',
            createdAt: new Date().toISOString(),
            updatedAt: null,
            isActive: true,
            roleId: 2,
            roleName: 'Investigador',
            provider: 'google',
            profilePictureUrl: undefined,
        }

        return {
            token: 'mock-google-token',
            refreshToken: 'mock-google-refresh-token',
            expiresAt: new Date(Date.now() + 60 * 60 * 1000).toISOString(),
            user: mockUser,
        }
    }
}

export const authServiceMock = new AuthServiceMock()
