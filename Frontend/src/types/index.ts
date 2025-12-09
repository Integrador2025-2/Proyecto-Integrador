// ============================================
// AUTENTICACIÓN - Basado en Backend/Models/DTOs/
// ============================================

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

export interface Role {
    id: number // Id
    name: string // Name
    description: string // Description
    permissions: string // Permissions
    isActive: boolean // IsActive
    createdAt: string // CreatedAt
    updatedAt: string | null // UpdatedAt
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
// Backend REAL: ProyectoId, FechaCreacion, UsuarioId (3 campos)
// Campos adicionales: MOCK para desarrollo frontend
// ============================================

export interface Project {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Proyecto.cs
    id: number // ProyectoId
    fechaCreacion: string // FechaCreacion (DateTime)
    usuarioId: number // UsuarioId

    // 🎭 CAMPOS MOCK (OPCIONALES - solo para desarrollo)
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
    objetivos?: Objective[]
    documentos?: Document[]
}

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

export interface Document {
    id: number
    nombre: string
    tipo: string
    tamano: number
    fechaSubida: string
    url: string
}

// ============================================
// ACTIVIDAD - Basado en Backend/Models/Domain/Actividad.cs
// Backend REAL: Estructura de presupuesto con rubros
// ============================================

export interface Activity {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Actividad.cs + ActividadDto.cs
    actividadId: number // ActividadId
    proyectoId: number // ProyectoId
    nombre: string // Nombre
    descripcion: string // Descripcion
    justificacion: string // Justificacion
    totalxAnios: number[] // TotalxAnios (array de decimales)
    cantidadAnios: number // CantidadAnios
    especificacionesTecnicas: string // EspecificacionesTecnicas
    valorUnitario: number // ValorUnitario
    valorTotal: number // ValorTotal (calculado)
    rubros: RubroItem[] // Rubros (unified list)

    // 🎭 CAMPOS MOCK (OPCIONALES - solo para desarrollo)
    id?: number // Alias de actividadId
    objectiveId?: number
    estado?: 'Pendiente' | 'En curso' | 'Completada' | 'Cancelada'
    responsable?: string
    responsableId?: number
    progreso?: number
    fechaInicio?: string
    fechaFin?: string
}

// ============================================
// RUBRO ITEM - Basado en Backend/Models/DTOs/ActividadDto.cs
// ============================================

export interface RubroItem {
    tipo: string // Tipo: TalentoHumano, EquiposSoftware, etc.
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

// ============================================
// TAREA - Basado en Backend/Models/Domain/Tarea.cs
// Backend REAL: TareaId, Nombre, Descripcion, Periodo, Monto, ActividadId
// ============================================

export interface Task {
    // ✅ CAMPOS DEL BACKEND (OBLIGATORIOS) - Tarea.cs + TareaDto.cs
    tareaId: number // TareaId
    nombre: string // Nombre
    descripcion: string // Descripcion
    periodo: string // Periodo (string, ej: "Mes 1-3")
    monto: number // Monto (decimal)
    actividadId: number // ActividadId

    // 🎭 CAMPOS MOCK (OPCIONALES - solo para desarrollo)
    id?: number // Alias de tareaId
    estado?: 'Pendiente' | 'En progreso' | 'Completada'
    responsable?: string
    fechaInicio?: string
    fechaFin?: string
}

// ============================================
// NOTIFICACIONES (Mock - no hay backend aún)
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

// ============================================
// DASHBOARD (Mock - no hay backend aún)
// ============================================

export interface DashboardStats {
    proyectosActivos: number
    actividadesPendientes: number
    tareasCompletadas: number
    progresoGeneral: number
    proximosVencimientos: Activity[]
    notificacionesRecientes: Notification[]
}

// ============================================
// TIPOS DE CREACIÓN/ACTUALIZACIÓN
// ============================================

export interface CreateProject {
    usuarioId: number
}

export interface UpdateProject {
    proyectoId: number
    usuarioId: number
}

export interface CreateActivity {
    proyectoId: number
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
    proyectoId: number
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
