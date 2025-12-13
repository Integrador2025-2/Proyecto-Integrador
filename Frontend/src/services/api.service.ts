import type {
    Project,
    Activity,
    Task,
    DashboardStats,
    TalentoHumano,
    EquiposSoftware,
    MaterialesInsumos,
    ServiciosTecnologicos,
    CapacitacionEventos,
    GastosViaje,
    Rubro,
    RAGQueryRequest,
    RAGQueryResponse,
    RAGBudgetGenerationRequest,
    BackendObjective,
} from '../types'
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

        try {
            const response = await fetch(`${API_URL}/proyectos`, {
                headers: this.getHeaders(),
            })

            // Si el backend devuelve 500 (DB incompleta), usar mock
            if (response.status === 500) {
                console.warn('⚠️ Backend DB incompleta, usando datos mockeados')
                return this.getMockedProjects()
            }

            const data = await this.handleResponse<any[]>(response)

            // Mapear de backend (PascalCase) a frontend (camelCase)
            return data.map((p) => this.mapProjectFromBackend(p))
        } catch (error) {
            console.error('Error getting projects, usando mock:', error)
            return this.getMockedProjects()
        }
    }

    /**
     * Obtiene un proyecto por ID con presupuesto calculado
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
            const project = this.mapProjectFromBackend(data)

            // 💰 CALCULAR PRESUPUESTO desde actividades
            try {
                const actividades = await this.getActivitiesByProjectId(id)
                const presupuestoTotal = actividades.reduce(
                    (sum, act) => sum + (act.valorUnitario || 0),
                    0,
                )

                project.presupuestoTotal = presupuestoTotal
                project.presupuestoEjecutado = Math.round(presupuestoTotal * 0.4) // 40% ejecutado
            } catch (error) {
                console.warn('No se pudo calcular presupuesto:', error)
                project.presupuestoTotal = 0
                project.presupuestoEjecutado = 0
            }

            return project
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
     * Navega Proyecto -> Objetivos -> CadenasDeValor -> Actividades
     */
    async getActivitiesByProjectId(projectId: number): Promise<Activity[]> {
        await this.simulateNetworkDelay()

        // 1. Objetivos del proyecto
        const objetivosRes = await fetch(`${API_URL}/objetivos/proyecto/${projectId}`, {
            headers: this.getHeaders(),
        })
        const objetivos = await this.handleResponse<any[]>(objetivosRes)

        const allActivities: Activity[] = []

        // 2. Por cada objetivo, cadenas de valor
        for (const obj of objetivos) {
            const cadenasRes = await fetch(`${API_URL}/cadenasdevalor/objetivo/${obj.objetivoId}`, {
                headers: this.getHeaders(),
            })
            const cadenas = await this.handleResponse<any[]>(cadenasRes)

            // 3. Por cada cadena, actividades
            for (const cadena of cadenas) {
                const actsRes = await fetch(
                    // 🔴 antes: `${API_URL}/actividades/cadena/${cadena.cadenaDeValorId}`,
                    `${API_URL}/actividades/cadena-de-valor/${cadena.cadenaDeValorId}`,
                    {
                        headers: this.getHeaders(),
                    },
                )
                const acts = await this.handleResponse<any[]>(actsRes)
                allActivities.push(...acts.map((a) => this.mapActivityFromBackend(a)))
            }
        }

        return allActivities
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

    async getTareasByActividad(actividadId: number): Promise<Task[]> {
        const res = await fetch(`${API_URL}/tareas/actividad/${actividadId}`, {
            headers: this.getHeaders(),
        })
        return this.handleResponse<Task[]>(res)
    }

    /**
     * Crea una nueva tarea
     */
    async createTask(taskData: {
        nombre: string
        descripcion: string
        periodo: string
        monto: number
        actividadId: number
    }): Promise<Task> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/tareas`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify({
                Nombre: taskData.nombre,
                Descripcion: taskData.descripcion,
                Periodo: taskData.periodo,
                Monto: taskData.monto,
                ActividadId: taskData.actividadId,
            }),
        })

        const data = await this.handleResponse<any>(response)
        return this.mapTaskFromBackend(data)
    }

    /**
     * Actualiza una tarea existente
     */
    async updateTask(taskId: number, updates: Partial<Task>): Promise<Task | null> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/tareas/${taskId}`, {
            method: 'PUT',
            headers: this.getHeaders(),
            body: JSON.stringify({
                TareaId: taskId,
                Nombre: updates.nombre,
                Descripcion: updates.descripcion,
                Periodo: updates.periodo,
                Monto: updates.monto,
                ActividadId: updates.actividadId,
            }),
        })

        if (!response.ok) {
            return null
        }

        const data = await response.json()
        return this.mapTaskFromBackend(data)
    }

    /**
     * Elimina una tarea
     */
    async deleteTask(taskId: number): Promise<void> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/tareas/${taskId}`, {
            method: 'DELETE',
            headers: this.getHeaders(),
        })

        if (!response.ok) {
            throw new Error('Error al eliminar tarea')
        }
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
    // RUBROS
    // ============================================

    /**
     * Obtiene todos los rubros
     */
    async getRubros(): Promise<Rubro[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rubros`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)
        return data.map((r) => ({
            rubroId: r.rubroId,
            actividadId: r.actividadId,
            nombre: r.nombre,
            descripcion: r.descripcion,
        }))
    }

    /**
     * Obtiene un rubro por ID
     */
    async getRubroById(id: number): Promise<Rubro | null> {
        await this.simulateNetworkDelay()

        try {
            const response = await fetch(`${API_URL}/rubros/${id}`, {
                headers: this.getHeaders(),
            })

            if (!response.ok) {
                return null
            }

            const data = await response.json()
            return {
                rubroId: data.rubroId,
                actividadId: data.actividadId,
                nombre: data.nombre,
                descripcion: data.descripcion,
            }
        } catch (error) {
            console.error('Error getting rubro by id:', error)
            return null
        }
    }

    // ============================================
    // TALENTO HUMANO
    // ============================================

    /**
     * Obtiene todo el talento humano
     */
    async getTalentoHumano(): Promise<TalentoHumano[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/talentohumano`, {
            headers: this.getHeaders(),
        })

        const data = await this.handleResponse<any[]>(response)
        return data.map((t) => this.mapTalentoHumanoFromBackend(t))
    }

    /**
     * Obtiene talento humano por rubro
     */
    async getTalentoHumanoByRubro(rubroId: number): Promise<TalentoHumano[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/talentohumano/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((t) => this.mapTalentoHumanoFromBackend(t))
    }

    // ============================================
    // EQUIPOS Y SOFTWARE
    // ============================================

    /**
     * Obtiene equipos y software por rubro
     */
    async getEquiposSoftwareByRubro(rubroId: number): Promise<EquiposSoftware[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/equipossoftware/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((e) => this.mapEquiposSoftwareFromBackend(e))
    }

    // ============================================
    // MATERIALES E INSUMOS
    // ============================================

    /**
     * Obtiene materiales e insumos por rubro
     */
    async getMaterialesInsumosByRubro(rubroId: number): Promise<MaterialesInsumos[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/materialesinsumos/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((m) => this.mapMaterialesInsumosFromBackend(m))
    }

    // ============================================
    // SERVICIOS TECNOLÓGICOS
    // ============================================

    /**
     * Obtiene servicios tecnológicos por rubro
     */
    async getServiciosTecnologicosByRubro(rubroId: number): Promise<ServiciosTecnologicos[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/serviciostecnologicos/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((s) => this.mapServiciosTecnologicosFromBackend(s))
    }

    // ============================================
    // CAPACITACIÓN Y EVENTOS
    // ============================================

    /**
     * Obtiene capacitación y eventos por rubro
     */
    async getCapacitacionEventosByRubro(rubroId: number): Promise<CapacitacionEventos[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/capacitacioneventos/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((c) => this.mapCapacitacionEventosFromBackend(c))
    }

    // ============================================
    // GASTOS DE VIAJE
    // ============================================

    /**
     * Obtiene gastos de viaje por rubro
     */
    async getGastosViajeByRubro(rubroId: number): Promise<GastosViaje[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/gastosviaje/rubro/${rubroId}`, {
            headers: this.getHeaders(),
        })

        if (response.status === 404) {
            return []
        }

        const data = await this.handleResponse<any[]>(response)
        return data.map((g) => this.mapGastosViajeFromBackend(g))
    }

    async getObjectivesByProjectId(projectId: number): Promise<BackendObjective[]> {
        const res = await fetch(`${API_URL}/objetivos/proyecto/${projectId}`, {
            headers: this.getHeaders(),
        })
        return this.handleResponse<BackendObjective[]>(res)
    }

    // ============================================
    // RAG SERVICE (Opcional - para futuras features)
    // ============================================

    /**
     * Consulta documentos usando RAG
     */
    async queryRAG(request: RAGQueryRequest): Promise<RAGQueryResponse> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rag/query`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify(request),
        })

        return await this.handleResponse<RAGQueryResponse>(response)
    }

    /**
     * Genera presupuesto usando IA
     */
    async generateBudgetWithAI(request: RAGBudgetGenerationRequest): Promise<any> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rag/budget/generate`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify(request),
        })

        return await this.handleResponse<any>(response)
    }

    /**
     * Sube un documento para RAG
     */
    async uploadDocument(
        file: File,
        projectId?: number,
        documentType: string = 'project_document'
    ): Promise<any> {
        const formData = new FormData()
        formData.append('file', file)
        if (projectId) {
            formData.append('project_id', projectId.toString())
        }
        formData.append('document_type', documentType)

        const response = await fetch(`${API_URL}/rag/documents/upload`, {
            method: 'POST',
            headers: {
                Authorization: `Bearer ${this.getToken()}`,
            },
            body: formData,
        })

        return await this.handleResponse<any>(response)
    }

    /**
     * Obtiene documentos de un proyecto
     */
    async getProjectDocuments(projectId: number): Promise<any[]> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rag/projects/${projectId}/documents`, {
            method: 'GET',
            headers: this.getHeaders(),
        })

        return await this.handleResponse<any[]>(response)
    }

    /**
     * Elimina un documento
     */
    async deleteDocument(documentId: number): Promise<void> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rag/documents/${documentId}`, {
            method: 'DELETE',
            headers: this.getHeaders(),
        })

        await this.handleResponse<void>(response)
    }

    /**
     * Genera presupuesto (método simplificado)
     */
    async generateBudget(request: {
        projectId: number
        projectDescription: string
        durationYears: number
    }): Promise<any> {
        await this.simulateNetworkDelay()

        const response = await fetch(`${API_URL}/rag/budget/generate`, {
            method: 'POST',
            headers: this.getHeaders(),
            body: JSON.stringify(request),
        })

        return await this.handleResponse<any>(response)
    }

    // ============================================
    // MAPPERS (Backend PascalCase → Frontend camelCase)
    // ============================================

    /**
     * Mapea proyecto del backend a frontend con enriquecimiento automático
     */
    private mapProjectFromBackend(backendProject: any): Project {
        // Generar valores por defecto inteligentes
        const proyectoId = backendProject.proyectoId
        const fechaCreacion = new Date(backendProject.fechaCreacion)
        const año = fechaCreacion.getFullYear()

        return {
            // ✅ Campos del backend (obligatorios)
            id: proyectoId,
            fechaCreacion: backendProject.fechaCreacion,
            usuarioId: backendProject.usuarioId,

            // 🎨 Campos enriquecidos (calculados/defaults)
            codigo: `SIGPI-${año}-${String(proyectoId).padStart(3, '0')}`,
            nombre: backendProject.nombre || `Sistema de Gestión de Proyectos de Investigación`,
            descripcion: `Proyecto de investigación financiado por el Sistema General de Regalías (SGR) de Colombia para la gestión integral de proyectos de I+D+i en la Universidad de Caldas.`,
            estado: backendProject.estado || 'En ejecución',
            investigadorPrincipal: 'Jeronimo Toro',
            entidadEjecutora: 'Universidad de Caldas',
            ubicacion: 'Manizales, Caldas, Colombia',
            fechaInicio: '2025-01-15',
            fechaFin: '2027-12-31',

            // 💰 Presupuesto (se calculará después desde actividades)
            presupuestoTotal: undefined, // Se calculará en getProjectById
            presupuestoEjecutado: undefined,
            progreso: 40,

            // 📋 Relaciones (vacías por defecto)
            participantes: [],
            objetivos: [],
            documentos: [],
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

    /**
     * Mapea talento humano del backend a frontend
     */
    private mapTalentoHumanoFromBackend(data: any): TalentoHumano {
        return {
            talentoHumanoId: data.talentoHumanoId,
            rubroId: data.rubroId,
            cargoEspecifico: data.cargoEspecifico,
            semanas: data.semanas,
            total: data.total,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Mapea equipos y software del backend a frontend
     */
    private mapEquiposSoftwareFromBackend(data: any): EquiposSoftware {
        return {
            equiposSoftwareId: data.equiposSoftwareId,
            rubroId: data.rubroId,
            especificacionesTecnicas: data.especificacionesTecnicas,
            cantidad: data.cantidad,
            total: data.total,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Mapea materiales e insumos del backend a frontend
     */
    private mapMaterialesInsumosFromBackend(data: any): MaterialesInsumos {
        return {
            materialesInsumosId: data.materialesInsumosId,
            rubroId: data.rubroId,
            materiales: data.materiales,
            total: data.total,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Mapea servicios tecnológicos del backend a frontend
     */
    private mapServiciosTecnologicosFromBackend(data: any): ServiciosTecnologicos {
        return {
            serviciosTecnologicosId: data.serviciosTecnologicosId,
            rubroId: data.rubroId,
            descripcion: data.descripcion,
            total: data.total,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Mapea capacitación y eventos del backend a frontend
     */
    private mapCapacitacionEventosFromBackend(data: any): CapacitacionEventos {
        return {
            capacitacionEventosId: data.capacitacionEventosId,
            rubroId: data.rubroId,
            tema: data.tema,
            cantidad: data.cantidad,
            total: data.total,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Mapea gastos de viaje del backend a frontend
     */
    private mapGastosViajeFromBackend(data: any): GastosViaje {
        return {
            gastosViajeId: data.gastosViajeId,
            rubroId: data.rubroId,
            costo: data.costo,
            ragEstado: data.ragEstado,
            periodoNum: data.periodoNum,
            periodoTipo: data.periodoTipo,
            actividadId: data.actividadId,
        }
    }

    /**
     * Genera proyectos mockeados cuando el backend falla (DB incompleta)
     */
    private getMockedProjects(): Project[] {
        const userId = parseInt(localStorage.getItem('userId') || '1')

        return [
            {
                id: 1,
                codigo: 'SIGPI-2025-001',
                nombre: 'Sistema de Gestión de Proyectos de Investigación',
                descripcion:
                    'Proyecto de investigación financiado por el Sistema General de Regalías (SGR) de Colombia para la gestión integral de proyectos de I+D+i en la Universidad de Caldas.',
                estado: 'En ejecución',
                investigadorPrincipal: 'Jeronimo Toro',
                entidadEjecutora: 'Universidad de Caldas',
                ubicacion: 'Manizales, Caldas, Colombia',
                fechaCreacion: '2025-01-15T10:00:00',
                fechaInicio: '2025-01-15',
                fechaFin: '2027-12-31',
                presupuestoTotal: 500000000,
                presupuestoEjecutado: 200000000,
                progreso: 40,
                usuarioId: userId,
                participantes: [],
                objetivos: [],
                documentos: [],
            },
        ]
    }
}

export const apiService = new ApiService()
