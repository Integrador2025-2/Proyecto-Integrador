import { useState, useEffect } from 'react'
import { Link } from 'react-router-dom'
import {
    FolderKanban,
    CheckSquare,
    ListChecks,
    TrendingUp,
    Calendar,
    AlertCircle,
} from 'lucide-react'
import { apiServiceMock } from '../../mocks/api.service.mock'
import { useAuthStore } from '../../store/authStore'
import type { DashboardStats, Project } from '../../types'

export default function DashboardPage() {
    const user = useAuthStore((state) => state.user)

    const [stats, setStats] = useState<DashboardStats | null>(null)
    const [projects, setProjects] = useState<Project[]>([])
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        loadDashboardData()
    }, [user])

    const loadDashboardData = async () => {
        if (!user) return

        try {
            setIsLoading(true)
            const [dashboardStats, userProjects] = await Promise.all([
                apiServiceMock.getDashboardStats(user.id),
                apiServiceMock.getProjectsByUserId(user.id),
            ])

            setStats(dashboardStats)
            setProjects(userProjects)
        } catch (error) {
            console.error('Error loading dashboard data:', error)
        } finally {
            setIsLoading(false)
        }
    }

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-96">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Cargando dashboard...</p>
                </div>
            </div>
        )
    }

    const getStatusColor = (estado: Project['estado']) => {
        switch (estado) {
            case 'En ejecución':
                return 'bg-green-100 text-green-800'
            case 'En revisión':
                return 'bg-yellow-100 text-yellow-800'
            case 'Finalizado':
                return 'bg-blue-100 text-blue-800'
            default:
                return 'bg-gray-100 text-gray-800'
        }
    }

    return (
        <div className="space-y-6">
            {/* Welcome Section */}
            <div>
                <h1 className="text-3xl font-bold text-gray-900">
                    ¡Bienvenido, {user?.firstName}!
                </h1>
                <p className="text-gray-600 mt-1">
                    Aquí tienes un resumen de tus proyectos y actividades
                </p>
            </div>

            {/* Stats Cards */}
            <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <div className="flex items-center justify-between">
                        <div>
                            <p className="text-sm font-medium text-gray-600">Proyectos Activos</p>
                            <p className="text-3xl font-bold text-gray-900 mt-2">
                                {stats?.proyectosActivos || 0}
                            </p>
                        </div>
                        <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                            <FolderKanban className="w-6 h-6 text-blue-600" />
                        </div>
                    </div>
                </div>

                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <div className="flex items-center justify-between">
                        <div>
                            <p className="text-sm font-medium text-gray-600">
                                Actividades Pendientes
                            </p>
                            <p className="text-3xl font-bold text-gray-900 mt-2">
                                {stats?.actividadesPendientes || 0}
                            </p>
                        </div>
                        <div className="w-12 h-12 bg-yellow-100 rounded-lg flex items-center justify-center">
                            <CheckSquare className="w-6 h-6 text-yellow-600" />
                        </div>
                    </div>
                </div>

                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <div className="flex items-center justify-between">
                        <div>
                            <p className="text-sm font-medium text-gray-600">Tareas Completadas</p>
                            <p className="text-3xl font-bold text-gray-900 mt-2">
                                {stats?.tareasCompletadas || 0}
                            </p>
                        </div>
                        <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center">
                            <ListChecks className="w-6 h-6 text-green-600" />
                        </div>
                    </div>
                </div>

                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                    <div className="flex items-center justify-between">
                        <div>
                            <p className="text-sm font-medium text-gray-600">Progreso General</p>
                            <p className="text-3xl font-bold text-gray-900 mt-2">
                                {stats?.progresoGeneral || 0}%
                            </p>
                        </div>
                        <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                            <TrendingUp className="w-6 h-6 text-purple-600" />
                        </div>
                    </div>
                </div>
            </div>

            {/* Two Column Layout */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Projects List - Takes 2 columns */}
                <div className="lg:col-span-2">
                    <div className="bg-white rounded-xl shadow-sm border border-gray-200">
                        <div className="p-6 border-b border-gray-200">
                            <div className="flex items-center justify-between">
                                <h2 className="text-xl font-bold text-gray-900">Mis Proyectos</h2>
                                <Link
                                    to="/projects"
                                    className="text-sm text-blue-600 hover:text-blue-700 font-medium"
                                >
                                    Ver todos →
                                </Link>
                            </div>
                        </div>

                        <div className="divide-y divide-gray-100">
                            {projects.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    <FolderKanban className="w-12 h-12 mx-auto mb-3 text-gray-300" />
                                    <p className="text-sm">No tienes proyectos asignados</p>
                                </div>
                            ) : (
                                projects.slice(0, 5).map((project) => (
                                    <Link
                                        key={project.id}
                                        to={`/projects/${project.id}`}
                                        className="block p-6 hover:bg-gray-50 transition"
                                    >
                                        <div className="flex items-start justify-between">
                                            <div className="flex-1">
                                                <div className="flex items-center gap-3 mb-2">
                                                    <h3 className="font-semibold text-gray-900">
                                                        {project.nombre}
                                                    </h3>
                                                    <span
                                                        className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(
                                                            project.estado,
                                                        )}`}
                                                    >
                                                        {project.estado}
                                                    </span>
                                                </div>
                                                <p className="text-sm text-gray-600 mb-3 line-clamp-2">
                                                    {project.descripcion}
                                                </p>
                                                <div className="flex items-center gap-4 text-xs text-gray-500">
                                                    <span>Código: {project.codigo}</span>
                                                    <span>•</span>
                                                    <span>
                                                        Responsable: {project.investigadorPrincipal}
                                                    </span>
                                                </div>
                                            </div>
                                            <div className="ml-4 text-right">
                                                <div className="text-2xl font-bold text-blue-600">
                                                    {project.progreso}%
                                                </div>
                                                <div className="text-xs text-gray-500 mt-1">
                                                    Progreso
                                                </div>
                                            </div>
                                        </div>
                                    </Link>
                                ))
                            )}
                        </div>
                    </div>
                </div>

                {/* Right Column - Upcoming Activities & Recent Notifications */}
                <div className="space-y-6">
                    {/* Upcoming Activities */}
                    <div className="bg-white rounded-xl shadow-sm border border-gray-200">
                        <div className="p-6 border-b border-gray-200">
                            <h2 className="text-lg font-bold text-gray-900 flex items-center gap-2">
                                <Calendar className="w-5 h-5 text-blue-600" />
                                Próximos Vencimientos
                            </h2>
                        </div>

                        <div className="divide-y divide-gray-100">
                            {stats?.proximosVencimientos &&
                            stats.proximosVencimientos.length > 0 ? (
                                stats.proximosVencimientos.slice(0, 5).map((activity) => (
                                    <div key={activity.id} className="p-4">
                                        <p className="font-medium text-gray-900 text-sm">
                                            {activity.nombre}
                                        </p>
                                        <div className="flex items-center gap-2 mt-2 text-xs text-gray-500">
                                            <Calendar className="w-3 h-3" />
                                            <span>
                                                {new Date(activity.fechaFin).toLocaleDateString(
                                                    'es-ES',
                                                    {
                                                        day: '2-digit',
                                                        month: 'short',
                                                    },
                                                )}
                                            </span>
                                        </div>
                                    </div>
                                ))
                            ) : (
                                <div className="p-8 text-center text-gray-500">
                                    <Calendar className="w-10 h-10 mx-auto mb-2 text-gray-300" />
                                    <p className="text-xs">No hay vencimientos próximos</p>
                                </div>
                            )}
                        </div>
                    </div>

                    {/* Recent Notifications */}
                    <div className="bg-white rounded-xl shadow-sm border border-gray-200">
                        <div className="p-6 border-b border-gray-200">
                            <h2 className="text-lg font-bold text-gray-900 flex items-center gap-2">
                                <AlertCircle className="w-5 h-5 text-blue-600" />
                                Notificaciones Recientes
                            </h2>
                        </div>

                        <div className="divide-y divide-gray-100">
                            {stats?.notificacionesRecientes &&
                            stats.notificacionesRecientes.length > 0 ? (
                                stats.notificacionesRecientes.map((notification) => (
                                    <div key={notification.id} className="p-4">
                                        <p className="font-medium text-gray-900 text-sm">
                                            {notification.titulo}
                                        </p>
                                        <p className="text-xs text-gray-600 mt-1">
                                            {notification.mensaje}
                                        </p>
                                        <p className="text-xs text-gray-400 mt-1">
                                            {new Date(notification.fecha).toLocaleDateString(
                                                'es-ES',
                                                {
                                                    day: '2-digit',
                                                    month: 'short',
                                                    hour: '2-digit',
                                                    minute: '2-digit',
                                                },
                                            )}
                                        </p>
                                    </div>
                                ))
                            ) : (
                                <div className="p-8 text-center text-gray-500">
                                    <AlertCircle className="w-10 h-10 mx-auto mb-2 text-gray-300" />
                                    <p className="text-xs">No hay notificaciones</p>
                                </div>
                            )}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
