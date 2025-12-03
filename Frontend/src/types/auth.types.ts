// Tipos que coinciden con los DTOs del backend

export interface LoginRequest {
    email: string
    password: string
}

export interface RegisterRequest {
    firstName: string
    lastName: string
    email: string
    password: string
    roleId?: number // Default 2 (Usuario)
}

export interface AuthResponse {
    token: string
    refreshToken: string
    expiresAt: string
    user: User
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

export interface User {
    id: number
    firstName: string
    lastName: string
    email: string
    fullName: string
    createdAt: string
    updatedAt?: string
    isActive: boolean
    roleId: number
    roleName: string
    provider: string
    profilePictureUrl?: string
    role?: Role
}

export interface Role {
    id: number
    name: string
    description: string
    permissions: string
    isActive: boolean
    createdAt: string
    updatedAt?: string
}
