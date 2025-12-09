import { Calendar, DollarSign, MapPin, Building2, User, Target } from 'lucide-react'
import type { Project } from '../../../../types'

interface OverviewTabProps {
    project: Project
}

export default function OverviewTab({ project }: OverviewTabProps) {
    const formatCurrency = (amount?: number) => {
        if (!amount) return '$0'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const formatDate = (date?: string) => {
        if (!date) return 'No especificada'
        return new Date(date).toLocaleDateString('es-ES', {
            day: 'numeric',
            month: 'long',
            year: 'numeric',
        })
    }

    // Estados con colores
    const getEstadoBadge = (estado?: string) => {
        const estadoActual = estado || 'En ejecución'
        const colors: Record<string, string> = {
            'En ejecución': 'bg-blue-100 text-blue-800',
            'En revisión': 'bg-yellow-100 text-yellow-800',
            Finalizado: 'bg-green-100 text-green-800',
            Planificación: 'bg-purple-100 text-purple-800',
        }

        return (
            <span
                className={`inline-flex items-center px-3 py-1 rounded-full text-sm font-medium ${
                    colors[estadoActual] || 'bg-gray-100 text-gray-800'
                }`}
            >
                {estadoActual}
            </span>
        )
    }

    return (
        <div className="space-y-6">
            {/* Descripción del Proyecto */}
            <div className="bg-white rounded-xl border border-gray-200 p-6">
                <h3 className="text-lg font-bold text-gray-900 mb-3">Descripción del Proyecto</h3>
                <p className="text-gray-700 leading-relaxed">
                    {project.descripcion || 'Sin descripción disponible.'}
                </p>
            </div>

            {/* Información General - Grid 2x2 */}
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                {/* Código del Proyecto */}
                <div className="bg-white rounded-xl border border-gray-200 p-5">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-10 h-10 bg-blue-100 rounded-lg flex items-center justify-center">
                            <Target className="w-5 h-5 text-blue-600" />
                        </div>
                        <div>
                            <p className="text-sm text-gray-600">Código del Proyecto</p>
                            <p className="text-lg font-bold text-gray-900">
                                {project.codigo || `SIGPI-${project.id}`}
                            </p>
                        </div>
                    </div>
                </div>

                {/* Investigador Principal */}
                <div className="bg-white rounded-xl border border-gray-200 p-5">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-10 h-10 bg-purple-100 rounded-lg flex items-center justify-center">
                            <User className="w-5 h-5 text-purple-600" />
                        </div>
                        <div>
                            <p className="text-sm text-gray-600">Investigador Principal</p>
                            <p className="text-lg font-bold text-gray-900">
                                {project.investigadorPrincipal || 'No asignado'}
                            </p>
                        </div>
                    </div>
                </div>

                {/* Entidad Ejecutora */}
                <div className="bg-white rounded-xl border border-gray-200 p-5">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-10 h-10 bg-green-100 rounded-lg flex items-center justify-center">
                            <Building2 className="w-5 h-5 text-green-600" />
                        </div>
                        <div>
                            <p className="text-sm text-gray-600">Entidad Ejecutora</p>
                            <p className="text-lg font-bold text-gray-900">
                                {project.entidadEjecutora || 'No especificada'}
                            </p>
                        </div>
                    </div>
                </div>

                {/* Ubicación */}
                <div className="bg-white rounded-xl border border-gray-200 p-5">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-10 h-10 bg-orange-100 rounded-lg flex items-center justify-center">
                            <MapPin className="w-5 h-5 text-orange-600" />
                        </div>
                        <div>
                            <p className="text-sm text-gray-600">Ubicación</p>
                            <p className="text-lg font-bold text-gray-900">
                                {project.ubicacion || 'No especificada'}
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Fechas y Presupuesto */}
            <div className="bg-gradient-to-br from-blue-50 to-indigo-50 rounded-xl border border-blue-200 p-6">
                <h3 className="text-lg font-bold text-gray-900 mb-4">Información Administrativa</h3>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Fechas */}
                    <div>
                        <div className="flex items-center gap-2 mb-3">
                            <Calendar className="w-5 h-5 text-blue-600" />
                            <span className="font-semibold text-gray-900">
                                Periodo de Ejecución
                            </span>
                        </div>
                        <div className="space-y-2">
                            <div className="flex justify-between">
                                <span className="text-sm text-gray-600">Fecha de inicio:</span>
                                <span className="text-sm font-medium text-gray-900">
                                    {formatDate(project.fechaInicio)}
                                </span>
                            </div>
                            <div className="flex justify-between">
                                <span className="text-sm text-gray-600">Fecha de fin:</span>
                                <span className="text-sm font-medium text-gray-900">
                                    {formatDate(project.fechaFin)}
                                </span>
                            </div>
                        </div>
                    </div>

                    {/* Presupuesto */}
                    <div>
                        <div className="flex items-center gap-2 mb-3">
                            <DollarSign className="w-5 h-5 text-green-600" />
                            <span className="font-semibold text-gray-900">Presupuesto</span>
                        </div>
                        <div className="space-y-2">
                            <div className="flex justify-between">
                                <span className="text-sm text-gray-600">Presupuesto total:</span>
                                <span className="text-sm font-bold text-green-600">
                                    {formatCurrency(project.presupuestoTotal)}
                                </span>
                            </div>
                            <div className="flex justify-between">
                                <span className="text-sm text-gray-600">Ejecutado:</span>
                                <span className="text-sm font-medium text-gray-900">
                                    {formatCurrency(project.presupuestoEjecutado)}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Estado del Proyecto */}
            <div className="bg-white rounded-xl border border-gray-200 p-6">
                <div className="flex items-center justify-between">
                    <div>
                        <h3 className="text-lg font-bold text-gray-900 mb-2">
                            Estado del Proyecto
                        </h3>
                        <p className="text-sm text-gray-600">
                            Última actualización: {formatDate(project.fechaCreacion)}
                        </p>
                    </div>
                    {getEstadoBadge(project.estado)}
                </div>

                {/* Barra de progreso si existe */}
                {project.progreso !== undefined && (
                    <div className="mt-4">
                        <div className="flex items-center justify-between text-sm mb-2">
                            <span className="font-medium text-gray-700">Progreso General</span>
                            <span className="font-bold text-gray-900">{project.progreso}%</span>
                        </div>
                        <div className="w-full bg-gray-200 rounded-full h-3">
                            <div
                                className="bg-blue-600 h-3 rounded-full transition-all duration-300"
                                style={{ width: `${project.progreso}%` }}
                            />
                        </div>
                    </div>
                )}
            </div>
        </div>
    )
}
