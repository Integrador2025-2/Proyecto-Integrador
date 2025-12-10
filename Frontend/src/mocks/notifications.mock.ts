import type { Notification } from '../types'

export const mockNotifications: Notification[] = [
    {
        id: 1,
        userId: 1,
        tipo: 'info',
        titulo: 'Nueva actividad asignada',
        mensaje: 'Se te ha asignado la actividad "Despliegue y Puesta en Producción"',
        leida: false,
        fecha: '2024-12-05T10:30:00Z',
        accionUrl: '/projects/1/activities/6',
    },
    {
        id: 2,
        userId: 1,
        tipo: 'warning',
        titulo: 'Actividad próxima a vencer',
        mensaje: 'La actividad "Desarrollo e Implementación" vence en 3 días',
        leida: false,
        fecha: '2024-12-04T14:20:00Z',
        accionUrl: '/projects/1/activities/3',
    },
    {
        id: 3,
        userId: 1,
        tipo: 'success',
        titulo: 'Actividad completada',
        mensaje: 'María González completó "Diseño de Arquitectura del Sistema"',
        leida: true,
        fecha: '2024-12-03T09:15:00Z',
        accionUrl: '/projects/1/activities/2',
    },
    {
        id: 4,
        userId: 1,
        tipo: 'info',
        titulo: 'Nuevo documento subido',
        mensaje: 'Juan Pérez subió "Presupuesto Detallado.pdf"',
        leida: true,
        fecha: '2024-12-02T16:45:00Z',
        accionUrl: '/projects/1/documents',
    },
    {
        id: 5,
        userId: 1,
        tipo: 'warning',
        titulo: 'Presupuesto al 80%',
        mensaje: 'El proyecto ha ejecutado el 80% de su presupuesto total',
        leida: true,
        fecha: '2024-12-01T11:30:00Z',
        accionUrl: '/projects/1/budget',
    },
    {
        id: 6,
        userId: 1,
        tipo: 'error',
        titulo: 'Tarea vencida',
        mensaje: 'La tarea "Corrección de bugs" venció sin completarse',
        leida: false,
        fecha: '2024-11-30T08:00:00Z',
        accionUrl: '/projects/1/activities/4',
    },
]

export const getNotificationsByUserId = (userId: number): Notification[] => {
    return mockNotifications.filter((n) => n.userId === userId)
}

export const getUnreadNotifications = (userId: number): Notification[] => {
    return mockNotifications.filter((n) => n.userId === userId && !n.leida)
}

export const markNotificationAsRead = (notificationId: number): void => {
    const notification = mockNotifications.find((n) => n.id === notificationId)
    if (notification) {
        notification.leida = true
    }
}

export const markAllAsRead = (userId: number): void => {
    mockNotifications.filter((n) => n.userId === userId).forEach((n) => (n.leida = true))
}
