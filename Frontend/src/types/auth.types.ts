// Tipos de request
export interface LoginRequest {
    email: string
    password: string
}

export interface RegisterRequest {
    firstName: string
    lastName: string
    email: string
    password: string
    roleId?: number
}

export interface TwoFactorVerifyRequest {
    tempToken: string
    code: string
}

export interface RefreshTokenRequest {
    refreshToken: string
}

export interface GoogleAuthRequest {
    googleToken: string
}

export interface ChangePasswordRequest {
    currentPassword: string
    newPassword: string
}

// Tipos de response
export interface TwoFactorInitResponse {
    tempToken: string
    maskedDestination: string
    expiresAt: string
}

export interface AuthResponse {
    token: string
    refreshToken: string
    expiresAt: string
    user: User
}

export interface User {
    id: number
    email: string
    firstName: string
    lastName: string
    fullName: string
    isActive: boolean
    roleId: number
    roleName: string
    provider: string
    profilePictureUrl?: string
    createdAt: string
}
