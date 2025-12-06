import type { Project, Activity, Task, Notification, DashboardStats } from '../types'

const API_URL = import.meta.env.VITE_API_URL || 'http://localhost:5043/api'

class ApiService {
    /**
     * Simula un pequeño delay de red
     */
    private async simulateNetworkDelay(minMs: number = 100, maxMs: number = 300): Promise<void> {
        const delay = Math.random() * (maxMs - minMs) + minMs
        return new Promise((resolve) => setTimeout(resolve, delay))
    }

    /**
     * Obtiene el token de autenticación
     */
    private getAuthToken(): string | null {
        return localStorage.getItem('auth_token')
    }

    /**
     * Headers comunes para todas las peticiones
     */
    private getHeaders(): HeadersInit {
        const token = this.getAuthToken()
        return {
            'Content-Type': 'application/json',
            ...(token ? { Authorization: `Bearer ${token}` } : {}),
        }
    }

    /**
     * Maneja errores de respuesta HTTP
     */
    private async handleResponse<T>(response: Response): Promise<T> {
        if (!response.ok) {
            const errorData = await response
                .json()
                .catch(() => ({ message: 'Error en la petición' }))
            throw new Error(errorData.message || `HTTP Error: ${response.status}`)
        }
        return await response.json()
    }

    // ============================================
    // PROYECTOS
    // ============================================

    /**
     * Obtiene todos los proyectos
     */
    async getProjects(): Promise<Project[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/proyectos`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)

        // Mapear de backend (PascalCase) a frontend (camelCase)
        return data.map((p) => this.mapProjectFromBackend(p))
    }

    /**
     * Obtiene un proyecto por ID
     */
    async getProjectById(id: number): Promise<Project | null> {
        await this.simulateNetworkDelay()

        try {
            const response = await fetch(`${API_URL}/proyectos/${id}`, {
                headers: this.getHeaders(),
            })

            if (!response.ok) {
                return null
            }

            const data = await response.json()
            return this.mapProjectFromBackend(data)
        } catch (error) {
            console.error('Error getting project by id:', error)
            return null
        }
    }

    /**
     * Obtiene proyectos por ID de usuario
     */
    async getProjectsByUserId(userId: number): Promise<Project[]> {
        await this.simulateNetworkDelay()

        // Backend no tiene este endpoint específico, filtrar del lado del cliente
        const allProjects = await this.getProjects()
        return allProjects.filter((p) => p.usuarioId === userId)
    }

    /**
     * Crea un nuevo proyecto
     */
    async createProject(projectData: { usuarioId: number }): Promise<Project> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/proyectos`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify({
                UsuarioId: projectData.usuarioId,
            }),
        })

        const data = await this.handleResponse<any>(response)
        return this.mapProjectFromBackend(data)
    }

    /**
     * Actualiza un proyecto existente
     */
    async updateProject(id: number, updates: Partial<Project>): Promise<Project> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/proyectos/${id}`, {
            method: 'PUT',
            headers: this.getHeaders(),
            body: JSON.stringify({
                ProyectoId: id,
                UsuarioId: updates.usuarioId,
            }),
        })

        const data = await this.handleResponse<any>(response)
        return this.mapProjectFromBackend(data)
    }

    /**
     * Elimina un proyecto
     */
    async deleteProject(id: number): Promise<void> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/proyectos/${id}`, {
            method: 'DELETE',
            headers: this.getHeaders(),
        })

        if (!response.ok) {
            throw new Error('Error al eliminar proyecto')
        }
    }

    // ============================================
    // ACTIVIDADES
    // ============================================

    /**
     * Obtiene todas las actividades
     */
    async getActivities(): Promise<Activity[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/actividades`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)
        return data.map((a) => this.mapActivityFromBackend(a))
    }

    /**
     * Obtiene actividades por ID de proyecto
     */
    async getActivitiesByProjectId(projectId: number): Promise<Activity[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/actividades/proyecto/${projectId}`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)
        return data.map((a) => this.mapActivityFromBackend(a))
    }

    /**
     * Obtiene actividades por ID de objetivo
     */
    async getActivitiesByObjectiveId(_objectiveId: number): Promise<Activity[]> {
        // Backend no tiene este endpoint aún
        await this.simulateNetworkDelay()
        return []
    }

    /**
     * Obtiene una actividad por ID
     */
    async getActivityById(id: number): Promise<Activity | null> {
        await this.simulateNetworkDelay()

        try {
            const response = await fetch(`${API_URL}/actividades/${id}`, {
                headers: this.getHeaders(),
            })

            if (!response.ok) {
                return null
            }

            const data = await response.json()
            return this.mapActivityFromBackend(data)
        } catch (error) {
            console.error('Error getting activity by id:', error)
            return null
        }
    }

    /**
     * Obtiene actividades por estado
     */
    async getActivitiesByStatus(_status: string): Promise<Activity[]> {
        // Backend no tiene este endpoint aún
        await this.simulateNetworkDelay()
        return []
    }

    /**
     * Crea una nueva actividad
     */
    async createActivity(activityData: any): Promise<Activity> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/actividades`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify({
                ProyectoId: activityData.proyectoId,
                Nombre: activityData.nombre,
                Descripcion: activityData.descripcion,
                Justificacion: activityData.justificacion,
                TotalxAnios: activityData.totalxAnios,
                CantidadAnios: activityData.cantidadAnios,
                EspecificacionesTecnicas: activityData.especificacionesTecnicas,
                ValorUnitario: activityData.valorUnitario,
            }),
        })

        const data = await this.handleResponse<any>(response)
        return this.mapActivityFromBackend(data)
    }

    /**
     * Actualiza una actividad existente
     */
    async updateActivity(id: number, updates: any): Promise<Activity> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/actividades/${id}`, {
            method: 'PUT',
            headers: this.getHeaders(),
            body: JSON.stringify({
                ActividadId: id,
                ProyectoId: updates.proyectoId,
                Nombre: updates.nombre,
                Descripcion: updates.descripcion,
                Justificacion: updates.justificacion,
                TotalxAnios: updates.totalxAnios,
                CantidadAnios: updates.cantidadAnios,
                EspecificacionesTecnicas: updates.especificacionesTecnicas,
                ValorUnitario: updates.valorUnitario,
            }),
        })

        const data = await this.handleResponse<any>(response)
        return this.mapActivityFromBackend(data)
    }

    /**
     * Elimina una actividad
     */
    async deleteActivity(id: number): Promise<void> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/actividades/${id}`, {
            method: 'DELETE',
            headers: this.getHeaders(),
        })

        if (!response.ok) {
            throw new Error('Error al eliminar actividad')
        }
    }

    // ============================================
    // TAREAS
    // ============================================

    /**
     * Obtiene tareas por ID de actividad
     */
    async getTareasByActividad(actividadId: number): Promise<Task[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/tareas/actividad/${actividadId}`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)
        return data.map((t) => this.mapTaskFromBackend(t))
    }

    // ============================================
    // NOTIFICACIONES (Mock - backend no implementado)
    // ============================================

    async getNotifications(_userId: number): Promise<Notification[]> {
        await this.simulateNetworkDelay()
        // Backend no tiene este endpoint aún
        return []
    }

    async getUnreadNotifications(_userId: number): Promise<Notification[]> {
        await this.simulateNetworkDelay()
        // Backend no tiene este endpoint aún
        return []
    }

    async markNotificationAsRead(_notificationId: number): Promise<void> {
        await this.simulateNetworkDelay()
        // Backend no tiene este endpoint aún
        console.log('markNotificationAsRead: Not implemented in backend')
    }

    async markAllNotificationsAsRead(_userId: number): Promise<void> {
        await this.simulateNetworkDelay()
        // Backend no tiene este endpoint aún
        console.log('markAllNotificationsAsRead: Not implemented in backend')
    }

    // ============================================
    // DASHBOARD (Mock - backend no implementado)
    // ============================================

    async getDashboardStats(userId: number): Promise<DashboardStats> {
        await this.simulateNetworkDelay()

        // Backend no tiene este endpoint, construir stats manualmente
        const projects = await this.getProjectsByUserId(userId)

        return {
            proyectosActivos: projects.length,
            actividadesPendientes: 0,
            tareasCompletadas: 0,
            progresoGeneral: 0,
            proximosVencimientos: [],
            notificacionesRecientes: [],
        }
    }

    // ============================================
    // MAPPERS (Backend PascalCase → Frontend camelCase)
    // ============================================

    /**
     * Mapea proyecto del backend a frontend
     */
    private mapProjectFromBackend(backendProject: any): Project {
        return {
            // Campos del backend (obligatorios)
            id: backendProject.proyectoId,
            fechaCreacion: backendProject.fechaCreacion,
            usuarioId: backendProject.usuarioId,

            // Campos mock (opcionales - no vienen del backend)
            codigo: undefined,
            nombre: undefined,
            descripcion: undefined,
            estado: undefined,
            investigadorPrincipal: undefined,
            entidadEjecutora: undefined,
            ubicacion: undefined,
            fechaInicio: undefined,
            fechaFin: undefined,
            presupuestoTotal: undefined,
            presupuestoEjecutado: undefined,
            progreso: undefined,
            participantes: undefined,
            objetivos: undefined,
            documentos: undefined,
        }
    }

    /**
     * Mapea actividad del backend a frontend
     */
    private mapActivityFromBackend(backendActivity: any): Activity {
        return {
            // Campos del backend (obligatorios)
            actividadId: backendActivity.actividadId,
            proyectoId: backendActivity.proyectoId,
            nombre: backendActivity.nombre,
            descripcion: backendActivity.descripcion,
            justificacion: backendActivity.justificacion,
            totalxAnios: backendActivity.totalxAnios || [],
            cantidadAnios: backendActivity.cantidadAnios,
            especificacionesTecnicas: backendActivity.especificacionesTecnicas,
            valorUnitario: backendActivity.valorUnitario,
            valorTotal: backendActivity.valorTotal,
            rubros: backendActivity.rubros || [],

            // Campos mock (opcionales)
            id: backendActivity.actividadId,
            objectiveId: undefined,
            estado: undefined,
            responsable: undefined,
            responsableId: undefined,
            progreso: undefined,
            fechaInicio: undefined,
            fechaFin: undefined,
        }
    }

    /**
     * Mapea tarea del backend a frontend
     */
    private mapTaskFromBackend(backendTask: any): Task {
        return {
            // Campos del backend (obligatorios)
            tareaId: backendTask.tareaId,
            nombre: backendTask.nombre,
            descripcion: backendTask.descripcion,
            periodo: backendTask.periodo,
            monto: backendTask.monto,
            actividadId: backendTask.actividadId,

            // Campos mock (opcionales)
            id: backendTask.tareaId,
            estado: undefined,
            responsable: undefined,
            fechaInicio: undefined,
            fechaFin: undefined,
        }
    }
}

export const apiService = new ApiService()
