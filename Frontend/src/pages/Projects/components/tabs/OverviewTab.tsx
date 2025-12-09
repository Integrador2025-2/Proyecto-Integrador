import { Calendar, User, Building, MapPin, Tag } from 'lucide-react'
import type { Project } from '../../../../types'

interface OverviewTabProps {
    project: Project
}

export default function OverviewTab({ project }: OverviewTabProps) {
    const formatCurrency = (amount?: number | null) => {
        if (amount == null || Number.isNaN(amount)) return 'Sin informar'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const formatDate = (dateString?: string | null) => {
        if (!dateString) return 'Sin fecha'
        const date = new Date(dateString)
        if (Number.isNaN(date.getTime())) return 'Sin fecha'
        return date.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: 'long',
            year: 'numeric',
        })
    }

    return (
        <div className="space-y-6">
            {/* Descripción / objetivo general */}
            <div className="bg-white rounded-xl border border-gray-200 p-5">
                <h3 className="text-sm font-semibold text-gray-700 mb-2">
                    Descripción / Objetivo general
                </h3>
                <p className="text-sm text-gray-700 leading-relaxed">
                    {project.descripcion || 'Sin descripción registrada.'}
                </p>
            </div>

            {/* Infos principales */}
            <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Tag className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Código del proyecto
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {project.codigo || 'Sin código'}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <User className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Investigador principal
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {project.investigadorPrincipal || 'Sin asignar'}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Building className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Entidad ejecutora
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {project.entidadEjecutora || 'No definida'}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <MapPin className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Ubicación
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {project.ubicacion || 'No definida'}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Calendar className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Fecha de inicio
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {formatDate(project.fechaInicio)}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Calendar className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Fecha de finalización
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {formatDate(project.fechaFin)}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Tag className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Estado
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {project.estado || 'Sin estado'}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-start gap-3">
                    <Tag className="w-5 h-5 text-blue-600 mt-0.5" />
                    <div>
                        <p className="text-xs font-semibold text-gray-500 uppercase tracking-wide">
                            Presupuesto total
                        </p>
                        <p className="text-sm font-medium text-gray-900">
                            {formatCurrency(project.presupuestoTotal)}
                        </p>
                    </div>
                </div>
            </div>
        </div>
    )
}
