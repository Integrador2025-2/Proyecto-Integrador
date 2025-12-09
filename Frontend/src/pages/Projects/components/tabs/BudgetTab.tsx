import { useState, useEffect } from 'react'
import { DollarSign, TrendingUp, TrendingDown, AlertCircle, Activity } from 'lucide-react'
import type { Project, Activity as ActivityType } from '../../../../types'
import { apiService } from '../../../../services/api.service'

interface BudgetTabProps {
    project: Project
}

interface BudgetStats {
    total: number
    ejecutado: number
    disponible: number
    porcentajeEjecutado: number
    porcentajeDisponible: number
}

interface ActivityBudget {
    actividadId: number
    nombre: string
    valorTotal: number
    porcentaje: number
}

export default function BudgetTab({ project }: BudgetTabProps) {
    const [activities, setActivities] = useState<ActivityType[]>([])
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        const loadActivities = async () => {
            try {
                setIsLoading(true)
                const data = await apiService.getActivitiesByProjectId(project.id)
                setActivities(data)
            } catch (error) {
                console.error('Error loading activities:', error)
                setActivities([])
            } finally {
                setIsLoading(false)
            }
        }

        loadActivities()
    }, [project.id])

    // Calcular estadísticas del presupuesto
    const calculateBudgetStats = (): BudgetStats => {
        let total = project.presupuestoTotal ?? 0

        if (!total && activities.length > 0) {
            total = activities.reduce((sum, act) => sum + (act.valorUnitario || 0), 0)
        }

        const ejecutado = project.presupuestoEjecutado ?? Math.round(total * 0.4)
        const disponible = total - ejecutado
        const porcentajeEjecutado = total > 0 ? (ejecutado / total) * 100 : 0
        const porcentajeDisponible = total > 0 ? (disponible / total) * 100 : 0

        return {
            total,
            ejecutado,
            disponible,
            porcentajeEjecutado,
            porcentajeDisponible,
        }
    }

    // Calcular presupuesto por actividades
    const calculateActivityBudgets = (): ActivityBudget[] => {
        const budgetStats = calculateBudgetStats()

        return activities.map((activity) => {
            // Usar valorUnitario si valorTotal no existe
            const valor = activity.valorTotal || activity.valorUnitario || 0

            return {
                actividadId: activity.actividadId,
                nombre: activity.nombre,
                valorTotal: valor,
                porcentaje: budgetStats.total > 0 ? (valor / budgetStats.total) * 100 : 0,
            }
        })
    }

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const stats = calculateBudgetStats()
    const activityBudgets = calculateActivityBudgets()

    // Determinar estado de salud del presupuesto
    const getBudgetHealthStatus = () => {
        if (stats.porcentajeEjecutado <= 50) {
            return {
                color: 'text-green-600',
                bg: 'bg-green-50',
                label: 'Saludable',
                icon: TrendingUp,
            }
        } else if (stats.porcentajeEjecutado <= 80) {
            return {
                color: 'text-yellow-600',
                bg: 'bg-yellow-50',
                label: 'Atención',
                icon: AlertCircle,
            }
        } else {
            return { color: 'text-red-600', bg: 'bg-red-50', label: 'Crítico', icon: TrendingDown }
        }
    }

    const healthStatus = getBudgetHealthStatus()
    const HealthIcon = healthStatus.icon

    if (isLoading) {
        return (
            <div className="min-h-[300px] flex items-center justify-center">
                <p className="text-gray-600">Cargando información presupuestal...</p>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            {/* Resumen General */}
            <div className="bg-gradient-to-br from-blue-50 to-indigo-50 rounded-xl border border-blue-200 p-6">
                <div className="flex items-center justify-between mb-4">
                    <h3 className="text-lg font-bold text-gray-900">Resumen Presupuestal</h3>
                    <div
                        className={`flex items-center gap-2 px-3 py-1 rounded-full ${healthStatus.bg}`}
                    >
                        <HealthIcon className={`w-4 h-4 ${healthStatus.color}`} />
                        <span className={`text-sm font-medium ${healthStatus.color}`}>
                            {healthStatus.label}
                        </span>
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                    {/* Presupuesto Total */}
                    <div className="bg-white rounded-lg p-4 border border-gray-200">
                        <div className="flex items-center gap-3 mb-2">
                            <div className="w-10 h-10 bg-blue-100 rounded-lg flex items-center justify-center">
                                <DollarSign className="w-5 h-5 text-blue-600" />
                            </div>
                            <div>
                                <p className="text-xs text-gray-600 uppercase tracking-wide">
                                    Presupuesto Total
                                </p>
                            </div>
                        </div>
                        <p className="text-2xl font-bold text-gray-900">
                            {formatCurrency(stats.total)}
                        </p>
                    </div>

                    {/* Presupuesto Ejecutado */}
                    <div className="bg-white rounded-lg p-4 border border-gray-200">
                        <div className="flex items-center gap-3 mb-2">
                            <div className="w-10 h-10 bg-green-100 rounded-lg flex items-center justify-center">
                                <TrendingUp className="w-5 h-5 text-green-600" />
                            </div>
                            <div>
                                <p className="text-xs text-gray-600 uppercase tracking-wide">
                                    Ejecutado
                                </p>
                            </div>
                        </div>
                        <p className="text-2xl font-bold text-gray-900">
                            {formatCurrency(stats.ejecutado)}
                        </p>
                        <p className="text-xs text-gray-600 mt-1">
                            {stats.porcentajeEjecutado.toFixed(1)}% del total
                        </p>
                    </div>

                    {/* Presupuesto Disponible */}
                    <div className="bg-white rounded-lg p-4 border border-gray-200">
                        <div className="flex items-center gap-3 mb-2">
                            <div className="w-10 h-10 bg-purple-100 rounded-lg flex items-center justify-center">
                                <Activity className="w-5 h-5 text-purple-600" />
                            </div>
                            <div>
                                <p className="text-xs text-gray-600 uppercase tracking-wide">
                                    Disponible
                                </p>
                            </div>
                        </div>
                        <p className="text-2xl font-bold text-gray-900">
                            {formatCurrency(stats.disponible)}
                        </p>
                        <p className="text-xs text-gray-600 mt-1">
                            {stats.porcentajeDisponible.toFixed(1)}% restante
                        </p>
                    </div>
                </div>

                {/* Barra de progreso general */}
                <div className="mt-4">
                    <div className="flex items-center justify-between text-sm mb-2">
                        <span className="font-medium text-gray-700">Ejecución Presupuestal</span>
                        <span className="font-bold text-gray-900">
                            {stats.porcentajeEjecutado.toFixed(1)}%
                        </span>
                    </div>
                    <div className="w-full bg-gray-200 rounded-full h-4 overflow-hidden">
                        <div
                            className={`h-4 rounded-full transition-all duration-500 ${
                                stats.porcentajeEjecutado <= 50
                                    ? 'bg-green-500'
                                    : stats.porcentajeEjecutado <= 80
                                    ? 'bg-yellow-500'
                                    : 'bg-red-500'
                            }`}
                            style={{ width: `${stats.porcentajeEjecutado}%` }}
                        />
                    </div>
                </div>
            </div>

            {/* Desglose por Actividades */}
            <div className="bg-white rounded-xl border border-gray-200 p-6">
                <h3 className="text-lg font-bold text-gray-900 mb-4">Desglose por Actividades</h3>

                {activityBudgets.length === 0 ? (
                    <div className="text-center py-8">
                        <Activity className="w-12 h-12 text-gray-300 mx-auto mb-3" />
                        <p className="text-sm text-gray-600">
                            Este proyecto aún no tiene actividades con presupuesto asignado.
                        </p>
                    </div>
                ) : (
                    <div className="space-y-4">
                        {activityBudgets.map((activity) => (
                            <div
                                key={activity.actividadId}
                                className="border border-gray-200 rounded-lg p-4 hover:border-blue-300 transition"
                            >
                                <div className="flex items-center justify-between mb-2">
                                    <h4 className="text-sm font-semibold text-gray-900">
                                        {activity.nombre}
                                    </h4>
                                    <span className="text-sm font-bold text-gray-900">
                                        {formatCurrency(activity.valorTotal)}
                                    </span>
                                </div>

                                <div className="flex items-center gap-3">
                                    <div className="flex-1">
                                        <div className="w-full bg-gray-200 rounded-full h-2">
                                            <div
                                                className="bg-blue-600 h-2 rounded-full transition-all duration-300"
                                                style={{ width: `${activity.porcentaje}%` }}
                                            />
                                        </div>
                                    </div>
                                    <span className="text-xs font-medium text-gray-600 min-w-[50px] text-right">
                                        {activity.porcentaje.toFixed(1)}%
                                    </span>
                                </div>
                            </div>
                        ))}
                    </div>
                )}
            </div>

            {/* Gráfico Visual Simple (Barras) */}
            <div className="bg-white rounded-xl border border-gray-200 p-6">
                <h3 className="text-lg font-bold text-gray-900 mb-4">Distribución Presupuestal</h3>

                <div className="space-y-3">
                    {/* Barra Ejecutado */}
                    <div>
                        <div className="flex items-center justify-between text-sm mb-1">
                            <span className="text-gray-700">Presupuesto Ejecutado</span>
                            <span className="font-medium text-green-600">
                                {formatCurrency(stats.ejecutado)}
                            </span>
                        </div>
                        <div className="w-full bg-gray-200 rounded-full h-8 overflow-hidden">
                            <div
                                className="bg-green-500 h-8 rounded-full flex items-center justify-end px-3 text-white text-xs font-bold transition-all duration-500"
                                style={{ width: `${stats.porcentajeEjecutado}%` }}
                            >
                                {stats.porcentajeEjecutado > 10 &&
                                    `${stats.porcentajeEjecutado.toFixed(0)}%`}
                            </div>
                        </div>
                    </div>

                    {/* Barra Disponible */}
                    <div>
                        <div className="flex items-center justify-between text-sm mb-1">
                            <span className="text-gray-700">Presupuesto Disponible</span>
                            <span className="font-medium text-purple-600">
                                {formatCurrency(stats.disponible)}
                            </span>
                        </div>
                        <div className="w-full bg-gray-200 rounded-full h-8 overflow-hidden">
                            <div
                                className="bg-purple-500 h-8 rounded-full flex items-center justify-end px-3 text-white text-xs font-bold transition-all duration-500"
                                style={{ width: `${stats.porcentajeDisponible}%` }}
                            >
                                {stats.porcentajeDisponible > 10 &&
                                    `${stats.porcentajeDisponible.toFixed(0)}%`}
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            {/* Alertas y Recomendaciones */}
            {stats.porcentajeEjecutado > 80 && (
                <div className="bg-red-50 border border-red-200 rounded-xl p-4">
                    <div className="flex items-start gap-3">
                        <AlertCircle className="w-5 h-5 text-red-600 mt-0.5 flex-shrink-0" />
                        <div>
                            <h4 className="text-sm font-semibold text-red-900 mb-1">
                                Alerta de Presupuesto
                            </h4>
                            <p className="text-sm text-red-700">
                                El proyecto ha ejecutado más del 80% de su presupuesto. Se
                                recomienda revisar las actividades pendientes y ajustar la
                                planificación financiera.
                            </p>
                        </div>
                    </div>
                </div>
            )}

            {stats.porcentajeEjecutado <= 30 && project.progreso && project.progreso > 50 && (
                <div className="bg-yellow-50 border border-yellow-200 rounded-xl p-4">
                    <div className="flex items-start gap-3">
                        <AlertCircle className="w-5 h-5 text-yellow-600 mt-0.5 flex-shrink-0" />
                        <div>
                            <h4 className="text-sm font-semibold text-yellow-900 mb-1">
                                Ejecución por Debajo del Progreso
                            </h4>
                            <p className="text-sm text-yellow-700">
                                El progreso del proyecto ({project.progreso}%) es mayor que la
                                ejecución presupuestal ({stats.porcentajeEjecutado.toFixed(1)}%).
                                Verifica que los gastos estén actualizados.
                            </p>
                        </div>
                    </div>
                </div>
            )}
        </div>
    )
}
