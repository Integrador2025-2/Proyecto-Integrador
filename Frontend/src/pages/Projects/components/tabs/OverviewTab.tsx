import { Calendar, User, Building, MapPin, Tag } from 'lucide-react'
import type { Project } from '../../../../types'

interface OverviewTabProps {
    project: Project
}

export default function OverviewTab({ project }: OverviewTabProps) {
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
        <div className="p-6 space-y-8">
            {/* Descripción del Proyecto */}
            <div>
                <h2 className="text-xl font-bold text-gray-900 mb-4">Descripción del Proyecto</h2>
                <p className="text-gray-700 leading-relaxed">{project.descripcion}</p>
            </div>

            {/* Información General */}
            <div>
                <h2 className="text-xl font-bold text-gray-900 mb-4">Información General</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Código del Proyecto */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-blue-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Tag className="w-5 h-5 text-blue-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">Código del Proyecto</p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {project.codigo}
                            </p>
                        </div>
                    </div>

                    {/* Investigador Principal */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-green-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <User className="w-5 h-5 text-green-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">
                                Investigador Principal
                            </p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {project.investigadorPrincipal}
                            </p>
                        </div>
                    </div>

                    {/* Entidad Ejecutora */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-purple-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Building className="w-5 h-5 text-purple-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">Entidad Ejecutora</p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {project.entidadEjecutora}
                            </p>
                        </div>
                    </div>

                    {/* Ubicación */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-yellow-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <MapPin className="w-5 h-5 text-yellow-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">Ubicación</p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {project.ubicacion}
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Fechas del Proyecto */}
            <div>
                <h2 className="text-xl font-bold text-gray-900 mb-4">Cronograma</h2>
                <div className="grid grid-cols-1 md:grid-cols-2 gap-6">
                    {/* Fecha de Inicio */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-blue-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Calendar className="w-5 h-5 text-blue-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">Fecha de Inicio</p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {formatDate(project.fechaInicio)}
                            </p>
                        </div>
                    </div>

                    {/* Fecha de Finalización */}
                    <div className="flex items-start gap-4">
                        <div className="w-10 h-10 bg-red-50 rounded-lg flex items-center justify-center flex-shrink-0">
                            <Calendar className="w-5 h-5 text-red-600" />
                        </div>
                        <div>
                            <p className="text-sm font-medium text-gray-500">
                                Fecha de Finalización
                            </p>
                            <p className="text-base font-semibold text-gray-900 mt-1">
                                {formatDate(project.fechaFin)}
                            </p>
                        </div>
                    </div>
                </div>
            </div>

            {/* Presupuesto Detallado */}
            <div>
                <h2 className="text-xl font-bold text-gray-900 mb-4">Presupuesto</h2>
                <div className="bg-gray-50 rounded-lg p-6">
                    <div className="space-y-4">
                        {/* Presupuesto Total */}
                        <div className="flex items-center justify-between">
                            <span className="text-gray-700 font-medium">Presupuesto Total</span>
                            <span className="text-xl font-bold text-gray-900">
                                {formatCurrency(project.presupuestoTotal)}
                            </span>
                        </div>

                        {/* Presupuesto Ejecutado */}
                        <div className="flex items-center justify-between">
                            <span className="text-gray-700 font-medium">Presupuesto Ejecutado</span>
                            <span className="text-xl font-bold text-green-600">
                                {formatCurrency(project.presupuestoEjecutado)}
                            </span>
                        </div>

                        {/* Presupuesto Disponible */}
                        <div className="flex items-center justify-between pt-4 border-t border-gray-200">
                            <span className="text-gray-700 font-medium">
                                Presupuesto Disponible
                            </span>
                            <span className="text-xl font-bold text-blue-600">
                                {formatCurrency(
                                    project.presupuestoTotal - project.presupuestoEjecutado,
                                )}
                            </span>
                        </div>

                        {/* Barra de progreso del presupuesto */}
                        <div className="pt-4">
                            <div className="flex items-center justify-between text-sm text-gray-600 mb-2">
                                <span>Ejecución Presupuestal</span>
                                <span className="font-medium">
                                    {(
                                        (project.presupuestoEjecutado / project.presupuestoTotal) *
                                        100
                                    ).toFixed(1)}
                                    %
                                </span>
                            </div>
                            <div className="w-full bg-gray-200 rounded-full h-3">
                                <div
                                    className="bg-green-600 h-3 rounded-full transition-all duration-500"
                                    style={{
                                        width: `${
                                            (project.presupuestoEjecutado /
                                                project.presupuestoTotal) *
                                            100
                                        }%`,
                                    }}
                                />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Estado del Proyecto */}
            <div>
                <h2 className="text-xl font-bold text-gray-900 mb-4">Estado del Proyecto</h2>
                <div className="bg-gray-50 rounded-lg p-6">
                    <div className="flex items-center justify-between mb-4">
                        <span className="text-gray-700 font-medium">Estado Actual</span>
                        <span
                            className={`px-4 py-2 rounded-full text-sm font-semibold ${
                                project.estado === 'En ejecución'
                                    ? 'bg-green-100 text-green-800'
                                    : project.estado === 'En revisión'
                                    ? 'bg-yellow-100 text-yellow-800'
                                    : project.estado === 'Finalizado'
                                    ? 'bg-blue-100 text-blue-800'
                                    : 'bg-purple-100 text-purple-800'
                            }`}
                        >
                            {project.estado}
                        </span>
                    </div>

                    <div className="space-y-3">
                        <div className="flex items-center justify-between">
                            <span className="text-gray-700">Progreso General</span>
                            <span className="text-2xl font-bold text-blue-600">
                                {project.progreso}%
                            </span>
                        </div>
                        <div className="w-full bg-gray-200 rounded-full h-3">
                            <div
                                className="bg-blue-600 h-3 rounded-full transition-all duration-500"
                                style={{ width: `${project.progreso}%` }}
                            />
                        </div>
                    </div>
                </div>
            </div>
        </div>
    )
}
