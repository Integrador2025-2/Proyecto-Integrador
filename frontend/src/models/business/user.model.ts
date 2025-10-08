// src/models/business/user.model.ts

/**
 * Modelo de Usuario
 * Define la estructura de datos de un usuario en el sistema
 */

export interface User {
    id: string
    email: string
    name: string
    lastName?: string
    avatar?: string
    role?: UserRole
    createdAt?: string
    updatedAt?: string
}

/**
 * Roles de usuario disponibles en el sistema
 */
export const UserRole = {
    ADMIN: 'admin',
    USER: 'user',
    MODERATOR: 'moderator',
} as const

export type UserRole = (typeof UserRole)[keyof typeof UserRole]

/**
 * Datos para login
 */
export interface LoginCredentials {
    email: string
    password: string
}

/**
 * Datos para registro
 */
export interface RegisterData {
    email: string
    password: string
    name: string
    lastName?: string
}

/**
 * Respuesta de autenticación del backend
 */
export interface AuthResponse {
    token: string
    user: User
}

/**
 * Datos para actualizar perfil
 */
export interface UpdateProfileData {
    name?: string
    lastName?: string
    avatar?: string
}
