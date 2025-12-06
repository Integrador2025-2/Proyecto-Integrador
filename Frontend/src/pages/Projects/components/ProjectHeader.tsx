import { Calendar, DollarSign, Users, TrendingUp } from 'lucide-react'
import type { Project } from '../../../types'

interface ProjectHeaderProps {
    project: Project
}

export default function ProjectHeader({ project }: ProjectHeaderProps) {
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

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const formatDate = (dateString: string) => {
        return new Date(dateString).toLocaleDateString('es-ES', {
            day: '2-digit',
            month: 'long',
            year: 'numeric',
        })
    }

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200 overflow-hidden">
            {/* Header Principal */}
            <div className="p-6 border-b border-gray-200">
                <div className="flex items-start justify-between mb-4">
                    <div className="flex-1">
                        <div className="flex items-center gap-3 mb-2">
                            <span className="text-sm font-medium text-gray-500">
                                {project.codigo}
                            </span>
                            <span
                                className={`px-3 py-1 rounded-full text-sm font-medium ${getStatusColor(
                                    project.estado,
                                )}`}
                            >
                                {project.estado}
                            </span>
                        </div>
                        <h1 className="text-3xl font-bold text-gray-900 mb-2">{project.nombre}</h1>
                        <p className="text-gray-600">{project.descripcion}</p>
                    </div>
                    <div className="ml-6 text-right">
                        <div className="text-5xl font-bold text-blue-600">{project.progreso}%</div>
                        <div className="text-sm text-gray-500 mt-1">Progreso Total</div>
                    </div>
                </div>

                {/* Barra de Progreso */}
                <div className="mt-4">
                    <div className="flex items-center justify-between text-sm text-gray-600 mb-2">
                        <span className="font-medium">Progreso del Proyecto</span>
                        <span>{project.progreso}% completado</span>
                    </div>
                    <div className="w-full bg-gray-200 rounded-full h-3">
                        <div
                            className="bg-blue-600 h-3 rounded-full transition-all duration-500"
                            style={{ width: `${project.progreso}%` }}
                        />
                    </div>
                </div>
            </div>

            {/* Estadísticas Rápidas */}
            <div className="grid grid-cols-1 md:grid-cols-4 divide-y md:divide-y-0 md:divide-x divide-gray-200">
                {/* Fechas */}
                <div className="p-6">
                    <div className="flex items-center gap-3 mb-3">
                        <div className="w-10 h-10 bg-blue-50 rounded-lg flex items-center justify-center">
                            <Calendar className="w-5 h-5 text-blue-600" />
                        </div>
                        <div>
                            <p className="text-xs font-medium text-gray-500 uppercase">Período</p>
                        </div>
                    </div>
                    <div className="space-y-1">
                        <p className="text-sm text-gray-600">
                            <span className="font-medium">Inicio:</span>{' '}
                            {formatDate(project.fechaInicio)}
                        </p>
                        <p className="text-sm text-gray-600">
                            <span className="font-medium">Fin:</span> {formatDate(project.fechaFin)}
                        </p>
                    </div>
                </div>

                {/* Presupuesto */}
                <div className="p-6">
                    <div className="flex items-center gap-3 mb-3">
                        <div className="w-10 h-10 bg-green-50 rounded-lg flex items-center justify-center">
                            <DollarSign className="w-5 h-5 text-green-600" />
                        </div>
                        <div>
                            <p className="text-xs font-medium text-gray-500 uppercase">
                                Presupuesto
                            </p>
                        </div>
                    </div>
                    <div className="space-y-1">
                        <p className="text-lg font-bold text-gray-900">
                            {formatCurrency(project.presupuestoTotal)}
                        </p>
                        <p className="text-sm text-gray-600">
                            Ejecutado: {formatCurrency(project.presupuestoEjecutado)}
                        </p>
                        <div className="w-full bg-gray-200 rounded-full h-1.5 mt-2">
                            <div
                                className="bg-green-600 h-1.5 rounded-full"
                                style={{
                                    width: `${
                                        (project.presupuestoEjecutado / project.presupuestoTotal) *
                                        100
                                    }%`,
                                }}
                            />
                        </div>
                    </div>
                </div>

                {/* Equipo */}
                <div className="p-6">
                    <div className="flex items-center gap-3 mb-3">
                        <div className="w-10 h-10 bg-purple-50 rounded-lg flex items-center justify-center">
                            <Users className="w-5 h-5 text-purple-600" />
                        </div>
                        <div>
                            <p className="text-xs font-medium text-gray-500 uppercase">Equipo</p>
                        </div>
                    </div>
                    <div className="space-y-1">
                        <p className="text-lg font-bold text-gray-900">
                            {project.participantes.length} miembros
                        </p>
                        <p className="text-sm text-gray-600 truncate">
                            IP: {project.investigadorPrincipal}
                        </p>
                    </div>
                </div>

                {/* Objetivos */}
                <div className="p-6">
                    <div className="flex items-center gap-3 mb-3">
                        <div className="w-10 h-10 bg-yellow-50 rounded-lg flex items-center justify-center">
                            <TrendingUp className="w-5 h-5 text-yellow-600" />
                        </div>
                        <div>
                            <p className="text-xs font-medium text-gray-500 uppercase">Objetivos</p>
                        </div>
                    </div>
                    <div className="space-y-1">
                        <p className="text-lg font-bold text-gray-900">
                            {project.objetivos.length} objetivos
                        </p>
                        <p className="text-sm text-gray-600">
                            {project.objetivos.filter((o) => o.estado === 'Completado').length}{' '}
                            completados
                        </p>
                        <div className="w-full bg-gray-200 rounded-full h-1.5 mt-2">
                            <div
                                className="bg-yellow-600 h-1.5 rounded-full"
                                style={{
                                    width: `${
                                        (project.objetivos.filter((o) => o.estado === 'Completado')
                                            .length /
                                            project.objetivos.length) *
                                        100
                                    }%`,
                                }}
                            />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
