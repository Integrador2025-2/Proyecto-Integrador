import type { User, AuthResponse } from '../types'

export const mockUsers: User[] = [
    {
        id: 1,
        firstName: 'Juan',
        lastName: 'Pérez',
        email: 'juan.perez@email.com',
        isActive: true,
        createdAt: '2024-01-15T10:00:00Z',
        updatedAt: '2024-12-01T15:30:00Z',
        roleId: 1,
        roleName: 'Administrador',
        provider: 'local',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=12',
    },
    {
        id: 2,
        firstName: 'María',
        lastName: 'González',
        email: 'maria.gonzalez@email.com',
        isActive: true,
        createdAt: '2024-02-10T08:00:00Z',
        updatedAt: '2024-12-03T12:00:00Z',
        roleId: 2,
        roleName: 'Investigador',
        provider: 'local',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=45',
    },
    {
        id: 3,
        firstName: 'Carlos',
        lastName: 'López',
        email: 'carlos.lopez@email.com',
        isActive: true,
        createdAt: '2024-03-05T14:00:00Z',
        updatedAt: '2024-11-28T09:15:00Z',
        roleId: 3,
        roleName: 'Evaluador',
        provider: 'local',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=33',
    },
]

export const mockLogin = (email: string): AuthResponse | null => {
    const user = mockUsers.find((u) => u.email === email)

    if (!user) return null

    // Simular validación de contraseña (en mock, aceptamos cualquier password)
    return {
        token: `mock-jwt-token-${user.id}-${Date.now()}`,
        refreshToken: `mock-refresh-token-${user.id}-${Date.now()}`,
        expiresAt: new Date(Date.now() + 3600000).toISOString(), // 1 hora
        user,
    }
}

export const mockRegister = (
    firstName: string,
    lastName: string,
    email: string,
    roleId: number = 2,
): AuthResponse => {
    const newUser: User = {
        id: mockUsers.length + 1,
        firstName,
        lastName,
        email,
        isActive: true,
        createdAt: new Date().toISOString(),
        updatedAt: new Date().toISOString(),
        roleId,
        roleName: roleId === 1 ? 'Administrador' : roleId === 2 ? 'Investigador' : 'Evaluador',
        provider: 'local',
        profilePictureUrl: `https://i.pravatar.cc/150?img=${mockUsers.length + 10}`,
    }

    mockUsers.push(newUser)

    return {
        token: `mock-jwt-token-${newUser.id}-${Date.now()}`,
        refreshToken: `mock-refresh-token-${newUser.id}-${Date.now()}`,
        expiresAt: new Date(Date.now() + 3600000).toISOString(),
        user: newUser,
    }
}

export const mockGetCurrentUser = (token: string): User | null => {
    // Extraer ID del token mock
    const match = token.match(/mock-jwt-token-(\d+)/)
    if (!match) return null

    const userId = parseInt(match[1])
    return mockUsers.find((u) => u.id === userId) || null
}
