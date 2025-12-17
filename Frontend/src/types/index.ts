// ============================================
// AUTENTICACIÓN - Basado en Backend/Models/DTOs/
// ============================================

export interface Role {
    id: number
    name: string
    description: string
    permissions: string
    isActive: boolean
    createdAt: string
    updatedAt: string | null
}

export interface User {
    // ✅ Campos del backend (UserDto.cs)
    id: number // Id
    firstName: string // FirstName
    lastName: string // LastName
    email: string // Email
    fullName: string // FullName (calculado en backend)
    createdAt: string // CreatedAt (DateTime)
    updatedAt: string | null // UpdatedAt (DateTime?)
    isActive: boolean // IsActive
    roleId: number // RoleId
    roleName: string // RoleName
    provider: string // Provider ("local" | "google")
    profilePictureUrl?: string // ProfilePictureUrl (nullable)
    role?: Role // Role (navegación)
}

export interface AuthResponse {
    // ✅ Campos del backend (AuthResponseDto.cs)
    token: string // Token
    refreshToken: string // RefreshToken
    expiresAt: string // ExpiresAt (DateTime)
    user: User // User
}

export interface TwoFactorInitResponse {
    // ✅ Campos del backend (TwoFactorInitResponseDto.cs)
    twoFactorRequired: boolean // TwoFactorRequired
    twoFactorToken: string // TwoFactorToken
    deliveryChannel: string // DeliveryChannel
    maskedDestination: string // MaskedDestination
}

export interface TwoFactorVerifyRequest {
    // ✅ Campos del backend (TwoFactorVerifyRequestDto.cs)
    twoFactorToken: string // TwoFactorToken
    code: string // Code
}

export interface LoginRequest {
    email: string
    password: string
}

export interface RegisterRequest {
    firstName: string
    lastName: string
    email: string
    password: string
    roleId?: number // Default: 2 (Usuario)
}

export interface ChangePasswordRequest {
    currentPassword: string
    newPassword: string
}

// ============================================
// PROYECTO - Basado en Backend/Models/Domain/Proyecto.cs
// Backend REAL: ProyectoId, FechaCreacion, UsuarioId
// Campos adicionales: solo frontend (enriquecimiento en memoria)
// ============================================

export interface ProjectParticipant {
    id: number
    nombre: string
    rol: string
    email: string
    profilePictureUrl?: string
}

export interface Objective {
    id: number
    tipo: 'General' | 'Específico'
    descripcion: string
    estado: 'Pendiente' | 'En progreso' | 'Completado'
    progreso: number
}

export interface ProjectDocument {
    id: number
    nombre: string
    tipo: string
    descripcion?: string
    tamano?: number
    fechaSubida: string
    url: string
}

// ============================================
// OBJETIVO - Basado en Backend/Models/Domain/Objetivo.cs
// Estructura real: Proyecto → Objetivo → CadenaDeValor → Actividad
// ============================================

export interface Objetivo {
    objetivoId: number
    proyectoId: number
    nombre: string
    descripcion?: string
    cadenasDeValor?: CadenaDeValor[]
}

// ============================================
// CADENA DE VALOR - Basado en Backend/Models/Domain/CadenaDeValor.cs
// ============================================

export interface CadenaDeValor {
    cadenaDeValorId: number
    objetivoId: number
    nombre: string
    objetivoEspecifico: string
    actividades?: Activity[]
}

export interface Project {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Proyecto.cs / ProyectoDto.cs
    id: number // ProyectoId
    fechaCreacion: string // FechaCreacion (DateTime)
    usuarioId: number // UsuarioId

    // 💡 CAMPOS DE NEGOCIO (solo frontend, mock/enriquecidos)
    codigo?: string
    nombre?: string
    descripcion?: string
    estado?: 'En ejecución' | 'En revisión' | 'Finalizado' | 'Planificación'
    investigadorPrincipal?: string
    entidadEjecutora?: string
    ubicacion?: string
    fechaInicio?: string
    fechaFin?: string
    presupuestoTotal?: number
    presupuestoEjecutado?: number
    progreso?: number
    participantes?: ProjectParticipant[]
    objetivos?: Objetivo[] // ✅ Nueva jerarquía real
    documentos?: ProjectDocument[]
}

// ============================================
// ACTIVIDAD - Basado en Backend/Models/Domain/Actividad.cs
// Backend REAL: presupuesto detallado en actividad y rubros
// ============================================

export interface RubroItem {
    tipo: string // TalentoHumano, EquiposSoftware, etc.
    id: number
    descripcion: string
    total: number
    periodoTipo: string
    periodoNum: number

    // Campos opcionales según el tipo
    cargoEspecifico?: string
    semanas?: number
    cantidad?: number
}

export interface Activity {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Actividad.cs + ActividadDto.cs
    actividadId: number // ActividadId
    cadenaDeValorId: number // CadenaDeValorId - ✅ Estructura real BD
    nombre: string // Nombre
    descripcion: string // Descripcion
    justificacion: string // Justificacion
    duracionAnios: number // DuracionAnios
    especificacionesTecnicas: string // EspecificacionesTecnicas
    valorUnitario: number // ValorUnitario

    // Campos opcionales del backend
    totalxAnios?: number[] // TotalxAnios (si existe)
    cantidadAnios?: number // CantidadAnios (alias de duracionAnios)
    valorTotal?: number // ValorTotal (calculado)
    rubros?: RubroItem[] // Rubros (lista unificada por tipo)

    // 💡 CAMPOS SOLO FRONTEND (estado visual)
    id?: number // Alias de actividadId
    proyectoId?: number // ❌ No existe en BD pero se puede calcular navegando
    estado?: 'Pendiente' | 'En curso' | 'Completada' | 'Cancelada'
    responsable?: string
    responsableId?: number
    progreso?: number
    fechaInicio?: string
    fechaFin?: string
}

// ============================================
// TAREA - Basado en Backend/Models/Domain/Tarea.cs
// Backend REAL: TareaId, Nombre, Descripcion, Periodo, Monto, ActividadId
// ============================================

export interface Task {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Tarea.cs + TareaDto.cs
    tareaId: number // TareaId
    nombre: string // Nombre
    descripcion: string // Descripcion
    periodo: string // Periodo (ej: "Mes 1-3")
    monto: number // Monto
    actividadId: number // ActividadId

    // 💡 CAMPOS SOLO FRONTEND (estado visual)
    id?: number // Alias de tareaId
    estado?: 'Pendiente' | 'En progreso' | 'Completada'
    responsable?: string
    fechaInicio?: string
    fechaFin?: string
}

// ============================================
// NOTIFICACIONES / DASHBOARD (solo frontend)
// ============================================

export interface Notification {
    id: number
    userId: number
    titulo: string
    mensaje: string
    tipo: 'info' | 'success' | 'warning' | 'error'
    leida: boolean
    fecha: string
}

export interface DashboardStats {
    proyectosActivos: number
    actividadesPendientes: number
    tareasCompletadas: number
    progresoGeneral: number
    proximosVencimientos: Activity[]
    notificacionesRecientes: Notification[]
}

// ============================================
// TIPOS DE CREACIÓN/ACTUALIZACIÓN (DTOs de entrada)
// ============================================

export interface CreateProject {
    usuarioId: number
}

export interface UpdateProject {
    proyectoId: number
    usuarioId: number
}

export interface CreateActivity {
    cadenaDeValorId: number
    nombre: string
    descripcion: string
    justificacion: string
    totalxAnios: number[]
    cantidadAnios: number
    especificacionesTecnicas: string
    valorUnitario: number
}

export interface UpdateActivity {
    actividadId: number
    cadenaDeValorId: number
    nombre: string
    descripcion: string
    justificacion: string
    totalxAnios: number[]
    cantidadAnios: number
    especificacionesTecnicas: string
    valorUnitario: number
}

export interface CreateTask {
    nombre: string
    descripcion: string
    periodo: string
    monto: number
    actividadId: number
}

export interface UpdateTask {
    tareaId: number
    nombre: string
    descripcion: string
    periodo: string
    monto: number
    actividadId: number
}

// ============================================
// SHAPES DIRECTOS DEL BACKEND (opcional)
// ============================================

export interface BackendUserDto {
    id: number
    firstName: string
    lastName: string
    email: string
    fullName: string
    createdAt: string
    updatedAt: string | null
    isActive: boolean
    roleId: number
    roleName: string
    provider: string
    profilePictureUrl?: string | null
}

export interface BackendAuthResponse {
    token: string
    refreshToken: string
    expiresAt: string
    user: BackendUserDto
}

export interface BackendTwoFactorInitResponse {
    twoFactorRequired: boolean
    twoFactorToken: string
    deliveryChannel: string
    maskedDestination: string
}

export interface BackendErrorResponse {
    message?: string
}

// ============================================
// RUBROS DETALLADOS - Basado en Backend Controllers
// ============================================

export interface TalentoHumano {
    talentoHumanoId: number
    rubroId: number
    recursoEspecificoId: number
    cargoEspecifico: string
    semanas: number
    total: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface EquiposSoftware {
    equiposSoftwareId: number
    rubroId: number
    recursoEspecificoId: number
    especificacionesTecnicas: string
    cantidad: number
    total: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface MaterialesInsumos {
    materialesInsumosId: number
    recursoEspecificoId: number
    rubroId: number
    materiales: string
    total: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface ServiciosTecnologicos {
    serviciosTecnologicosId: number
    recursoEspecificoId: number
    rubroId: number
    descripcion: string
    total: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface CapacitacionEventos {
    capacitacionEventosId: number
    rubroId: number
    recursoEspecificoId: number
    tema: string
    cantidad: number
    total: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface GastosViaje {
    recursoEspecificoId: number
    gastosViajeId: number
    rubroId: number
    costo: number
    ragEstado: string
    periodoNum: number
    periodoTipo: string
    actividadId?: number
}

export interface Rubro {
    rubroId: number
    descripcion?: string
}

export interface BackendObjective {
    objetivoId: number
    proyectoId: number
    nombre: string
    descripcion: string
    resultadoEsperado: string
    proyectoNombre: string
}

// ============================================
// USUARIOS Y ROLES - Para administración
// ============================================

export interface RoleDetailed {
    id: number
    name: string
    description: string
    permissions: string
    isActive: boolean
    createdAt: string
    updatedAt: string | null
}

export interface UserDetailed extends User {
    role?: RoleDetailed
}

// ============================================
// RAG SERVICE - Para integración con IA
// ============================================

export interface RAGQueryRequest {
    question: string
    projectId?: number
    topK?: number
}

export interface RAGQueryResponse {
    answer: string
    sources: Array<{
        content: string
        metadata: Record<string, any>
    }>
}

export interface RAGBudgetGenerationRequest {
    projectId: number
    projectDescription: string
    budgetCategories: string[]
    durationYears: number
    activities?: Array<{
        actividadId: number
        nombre: string
        descripcion: string
        justificacion: string
        especificacionesTecnicas: string
        cantidadAnios: number
        valorUnitario: number
        duracionDias?: number
    }>
}
