import type { DashboardStats } from '../types'
import { mockProjects } from './projects.mock'
import { mockActivities, getUpcomingActivities } from './activities.mock'
import { getNotificationsByUserId } from './notifications.mock'

export const getDashboardStats = (userId: number): DashboardStats => {
    // Filtrar proyectos del usuario
    const userProjects = mockProjects.filter((p) =>
        p.participantes.some((participant) => participant.userId === userId),
    )

    // Contar proyectos activos
    const proyectosActivos = userProjects.filter(
        (p) => p.estado === 'En ejecución' || p.estado === 'En revisión',
    ).length

    // Contar actividades pendientes del usuario
    const actividadesPendientes = mockActivities.filter(
        (a) => a.responsableId === userId && (a.estado === 'Pendiente' || a.estado === 'En curso'),
    ).length

    // Contar tareas completadas
    const tareasCompletadas = mockActivities
        .filter((a) => a.responsableId === userId)
        .reduce((total, activity) => {
            return total + activity.tareas.filter((t) => t.completada).length
        }, 0)

    // Calcular progreso general (promedio de progreso de proyectos activos)
    const progresoGeneral =
        userProjects.length > 0
            ? Math.round(userProjects.reduce((sum, p) => sum + p.progreso, 0) / userProjects.length)
            : 0

    // Obtener próximos vencimientos
    const proximosVencimientos = getUpcomingActivities(7)

    // Obtener notificaciones recientes
    const notificacionesRecientes = getNotificationsByUserId(userId)
        .sort((a, b) => new Date(b.fecha).getTime() - new Date(a.fecha).getTime())
        .slice(0, 5)

    return {
        proyectosActivos,
        actividadesPendientes,
        tareasCompletadas,
        progresoGeneral,
        proximosVencimientos,
        notificacionesRecientes,
    }
}
