import { Link } from 'react-router-dom'
import { Calendar, Users, DollarSign, TrendingUp } from 'lucide-react'
import type { Project } from '../../types'

interface ProjectCardProps {
    project: Project
}

export default function ProjectCard({ project }: ProjectCardProps) {
    const getStatusColor = (estado: Project['estado']) => {
        switch (estado) {
            case 'En ejecución':
                return 'bg-green-100 text-green-800'
            case 'En revisión':
                return 'bg-yellow-100 text-yellow-800'
            case 'Finalizado':
                return 'bg-blue-100 text-blue-800'
            case 'Planificación':
                return 'bg-purple-100 text-purple-800'
            default:
                return 'bg-gray-100 text-gray-800'
        }
    }

    const formatCurrency = (amount?: number) => {
        if (!amount || Number.isNaN(amount)) return 'Sin presupuesto'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const formatDate = (dateString?: string) => {
        if (!dateString) return 'Sin fecha'
        const d = new Date(dateString)
        if (Number.isNaN(d.getTime())) return 'Sin fecha'
        return d.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
        })
    }

    const progreso = project.progreso ?? 0
    const participantes = project.participantes ?? []
    const objetivos = project.objetivos ?? []
    const presupuestoTotal = project.presupuestoTotal ?? 0
    const presupuestoEjecutado = project.presupuestoEjecutado ?? 0
    const ejecutadoPct =
        presupuestoTotal > 0 ? ((presupuestoEjecutado / presupuestoTotal) * 100).toFixed(0) : '0'

    return (
        <Link
            to={`/projects/${project.id}`}
            className="block bg-white rounded-xl shadow-sm border border-gray-200 hover:shadow-md transition-all duration-200 overflow-hidden group"
        >
            {/* Header con estado y progreso */}
            <div className="p-6 border-b border-gray-100">
                <div className="flex items-start justify-between mb-3">
                    <div className="flex-1">
                        <div className="flex items-center gap-2 mb-2">
                            <span className="text-xs font-medium text-gray-500">
                                {project.codigo || `PROY-${project.id}`}
                            </span>
                            <span
                                className={`px-2 py-1 rounded-full text-xs font-medium ${getStatusColor(
                                    project.estado,
                                )}`}
                            >
                                {project.estado || 'Sin estado'}
                            </span>
                        </div>
                        <h3 className="text-lg font-bold text-gray-900 group-hover:text-blue-600 transition-colors line-clamp-2">
                            {project.nombre || 'Proyecto sin nombre'}
                        </h3>
                    </div>
                    <div className="ml-4 text-right">
                        <div className="text-3xl font-bold text-blue-600">{progreso}%</div>
                        <div className="text-xs text-gray-500 mt-1">Progreso</div>
                    </div>
                </div>

                <p className="text-sm text-gray-600 line-clamp-2">
                    {project.descripcion || 'Sin descripción.'}
                </p>

                {/* Barra de progreso */}
                <div className="mt-4">
                    <div className="flex items-center justify-between text-xs text-gray-600 mb-1">
                        <span>Progreso del proyecto</span>
                        <span className="font-medium">{progreso}%</span>
                    </div>
                    <div className="w-full bg-gray-200 rounded-full h-2">
                        <div
                            className="bg-blue-600 h-2 rounded-full transition-all duration-300"
                            style={{ width: `${progreso}%` }}
                        />
                    </div>
                </div>
            </div>

            {/* Información adicional */}
            <div className="p-6">
                <div className="grid grid-cols-2 gap-4">
                    {/* Fechas */}
                    <div className="flex items-start gap-3">
                        <div className="w-8 h-8 bg-blue-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Calendar className="w-4 h-4 text-blue-600" />
                        </div>
                        <div className="flex-1 min-w-0">
                            <p className="text-xs text-gray-500">Período</p>
                            <p className="text-sm font-medium text-gray-900 truncate">
                                {formatDate(project.fechaInicio)}
                            </p>
                            <p className="text-xs text-gray-500">
                                hasta {formatDate(project.fechaFin)}
                            </p>
                        </div>
                    </div>

                    {/* Equipo */}
                    <div className="flex items-start gap-3">
                        <div className="w-8 h-8 bg-green-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Users className="w-4 h-4 text-green-600" />
                        </div>
                        <div className="flex-1 min-w-0">
                            <p className="text-xs text-gray-500">Equipo</p>
                            <p className="text-sm font-medium text-gray-900 truncate">
                                {participantes.length} miembros
                            </p>
                            <p className="text-xs text-gray-500 truncate">
                                {project.investigadorPrincipal || 'Sin investigador principal'}
                            </p>
                        </div>
                    </div>

                    {/* Presupuesto */}
                    <div className="flex items-start gap-3">
                        <div className="w-8 h-8 bg-purple-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <DollarSign className="w-4 h-4 text-purple-600" />
                        </div>
                        <div className="flex-1 min-w-0">
                            <p className="text-xs text-gray-500">Presupuesto Total</p>
                            <p className="text-sm font-medium text-gray-900 truncate">
                                {formatCurrency(presupuestoTotal)}
                            </p>
                            <p className="text-xs text-gray-500">Ejecutado: {ejecutadoPct}%</p>
                        </div>
                    </div>

                    {/* Objetivos */}
                    <div className="flex items-start gap-3">
                        <div className="w-8 h-8 bg-yellow-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <TrendingUp className="w-4 h-4 text-yellow-600" />
                        </div>
                        <div className="flex-1 min-w-0">
                            <p className="text-xs text-gray-500">Objetivos</p>
                            <p className="text-sm font-medium text-gray-900">
                                {objetivos.length} objetivos
                            </p>
                            <p className="text-xs text-gray-500">
                                {objetivos.filter((o) => o.estado === 'Completado').length}{' '}
                                completados
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Footer con avatar del equipo */}
            <div className="px-6 py-4 bg-gray-50 border-t border-gray-100">
                <div className="flex items-center justify-between">
                    <div className="flex -space-x-2">
                        {participantes.slice(0, 4).map((participant) => (
                            <img
                                key={participant.id}
                                src={
                                    participant.profilePictureUrl ||
                                    `https://i.pravatar.cc/150?img=${participant.id}`
                                }
                                alt={participant.nombre}
                                className="w-8 h-8 rounded-full border-2 border-white"
                                title={participant.nombre}
                            />
                        ))}
                        {participantes.length > 4 && (
                            <div className="w-8 h-8 rounded-full border-2 border-white bg-gray-200 flex items-center justify-center">
                                <span className="text-xs font-medium text-gray-600">
                                    +{participantes.length - 4}
                                </span>
                            </div>
                        )}
                    </div>
                    <span className="text-xs text-blue-600 font-medium group-hover:underline">
                        Ver detalles →
                    </span>
                </div>
            </div>
        </Link>
    )
}
