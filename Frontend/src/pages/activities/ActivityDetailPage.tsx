// src/pages/activities/ActivityDetailPage.tsx
import { useEffect, useState } from 'react'
import { useParams, Link } from 'react-router-dom'
import { ArrowLeft, Calendar, Loader2, User, Flag, DollarSign, ListChecks } from 'lucide-react'
import type { Activity, Task } from '../../types'
import { apiService } from '../../services/api.service'
import useTasks from '../../hooks/useTasks'

export default function ActivityDetailPage() {
    const { id } = useParams<{ id: string }>()
    const actividadId = Number(id)

    const [activity, setActivity] = useState<Activity | null>(null)
    const [loadingActivity, setLoadingActivity] = useState(false)
    const [error, setError] = useState<string | null>(null)

    const {
        tasks,
        loading: loadingTasks,
        error: tasksError,
        reload: reloadTasks,
    } = useTasks({ actividadId, autoLoad: true })

    useEffect(() => {
        const loadActivity = async () => {
            if (!actividadId) return
            setLoadingActivity(true)
            setError(null)
            try {
                const data = await apiService.getActivityById(actividadId)
                if (!data) {
                    setError('Actividad no encontrada.')
                    setActivity(null)
                    return
                }

                // Pequeño enriquecimiento visual
                const estado = data.estado || 'Pendiente'
                const progreso =
                    data.progreso ??
                    (estado === 'Completada' ? 100 : estado === 'En curso' ? 50 : 10)

                setActivity({
                    ...data,
                    estado,
                    responsable: data.responsable || 'Sin responsable asignado',
                    progreso,
                })
            } catch (err) {
                console.error('Error loading activity:', err)
                setError('No se pudo cargar la actividad.')
            } finally {
                setLoadingActivity(false)
            }
        }

        void loadActivity()
    }, [actividadId])

    if (!actividadId || Number.isNaN(actividadId)) {
        return (
            <div className="space-y-4">
                <Link
                    to="/activities"
                    className="inline-flex items-center text-sm text-blue-600 hover:underline"
                >
                    <ArrowLeft className="w-4 h-4 mr-1" />
                    Volver a actividades
                </Link>
                <p className="text-sm text-red-600">ID de actividad no válido.</p>
            </div>
        )
    }

    if (loadingActivity) {
        return (
            <div className="flex flex-col items-center justify-center py-12">
                <Loader2 className="w-6 h-6 text-blue-600 animate-spin" />
                <p className="mt-2 text-sm text-gray-600">Cargando actividad...</p>
            </div>
        )
    }

    if (error || !activity) {
        return (
            <div className="space-y-4">
                <Link
                    to="/activities"
                    className="inline-flex items-center text-sm text-blue-600 hover:underline"
                >
                    <ArrowLeft className="w-4 h-4 mr-1" />
                    Volver a actividades
                </Link>
                <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md text-sm">
                    {error || 'No se encontró la actividad.'}
                </div>
            </div>
        )
    }

    const formatCurrency = (amount?: number) => {
        if (!amount || Number.isNaN(amount)) return '$0'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex items-center justify-between gap-4">
                <div className="flex items-center gap-3">
                    <Link
                        to="/activities"
                        className="inline-flex items-center text-sm text-blue-600 hover:underline"
                    >
                        <ArrowLeft className="w-4 h-4 mr-1" />
                        Volver
                    </Link>
                    <h1 className="text-2xl font-bold text-gray-900">{activity.nombre}</h1>
                </div>
                <span className="inline-flex items-center px-3 py-1 rounded-full text-xs font-medium bg-blue-50 text-blue-700">
                    {activity.estado}
                </span>
            </div>

            {/* Info principal */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="md:col-span-2 bg-white rounded-xl border border-gray-200 p-5 space-y-3">
                    <h2 className="text-sm font-semibold text-gray-900">Descripción</h2>
                    <p className="text-sm text-gray-700">
                        {activity.descripcion || 'Sin descripción disponible.'}
                    </p>

                    <h3 className="text-sm font-semibold text-gray-900 mt-3">Justificación</h3>
                    <p className="text-sm text-gray-700">
                        {activity.justificacion || 'Sin justificación definida.'}
                    </p>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-5 space-y-3">
                    <h2 className="text-sm font-semibold text-gray-900">Resumen</h2>
                    <div className="space-y-2 text-xs text-gray-600">
                        <div className="flex items-center gap-2">
                            <User className="w-4 h-4 text-gray-400" />
                            <span>{activity.responsable || 'Sin responsable'}</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <Calendar className="w-4 h-4 text-gray-400" />
                            <span>
                                {activity.fechaInicio || 'Sin fecha inicio'} –{' '}
                                {activity.fechaFin || 'Sin fecha fin'}
                            </span>
                        </div>
                        <div className="flex items-center gap-2">
                            <Flag className="w-4 h-4 text-gray-400" />
                            <span>Progreso: {activity.progreso ?? 0}%</span>
                        </div>
                        <div className="flex items-center gap-2">
                            <DollarSign className="w-4 h-4 text-gray-400" />
                            <span>Valor unitario: {formatCurrency(activity.valorUnitario)}</span>
                        </div>
                    </div>

                    <div className="mt-3">
                        <div className="flex items-center justify-between text-xs text-gray-600 mb-1">
                            <span>Progreso de la actividad</span>
                            <span className="font-medium">{activity.progreso ?? 0}%</span>
                        </div>
                        <div className="w-full bg-gray-200 rounded-full h-2">
                            <div
                                className="bg-blue-600 h-2 rounded-full transition-all duration-300"
                                style={{ width: `${activity.progreso ?? 0}%` }}
                            />
                        </div>
                    </div>
                </div>
            </div>

            {/* Especificaciones técnicas */}
            <div className="bg-white rounded-xl border border-gray-200 p-5">
                <h2 className="text-sm font-semibold text-gray-900 mb-2">
                    Especificaciones técnicas
                </h2>
                <p className="text-sm text-gray-700 whitespace-pre-line">
                    {activity.especificacionesTecnicas ||
                        'Sin especificaciones técnicas registradas.'}
                </p>
            </div>

            {/* Tareas */}
            <div className="bg-white rounded-xl border border-gray-200 p-5">
                <div className="flex items-center justify-between mb-3">
                    <div className="flex items-center gap-2">
                        <ListChecks className="w-4 h-4 text-gray-700" />
                        <h2 className="text-sm font-semibold text-gray-900">
                            Tareas de la actividad
                        </h2>
                    </div>
                    <button
                        type="button"
                        onClick={() => void reloadTasks()}
                        className="text-xs text-blue-600 hover:underline"
                    >
                        Recargar
                    </button>
                </div>

                {loadingTasks && (
                    <div className="flex items-center gap-2 text-xs text-gray-600">
                        <Loader2 className="w-4 h-4 animate-spin text-blue-600" />
                        Cargando tareas...
                    </div>
                )}

                {!loadingTasks && tasksError && (
                    <div className="text-xs text-red-600">{tasksError}</div>
                )}

                {!loadingTasks && !tasksError && tasks.length === 0 && (
                    <p className="text-xs text-gray-500">
                        No hay tareas registradas para esta actividad.
                    </p>
                )}

                {!loadingTasks && !tasksError && tasks.length > 0 && (
                    <ul className="space-y-2 mt-2">
                        {tasks.map((task: Task) => (
                            <li
                                key={task.tareaId}
                                className="flex items-center justify-between border rounded-md px-3 py-2 text-xs"
                            >
                                <div>
                                    <p className="font-medium text-gray-900">{task.nombre}</p>
                                    <p className="text-gray-600">{task.descripcion}</p>
                                    <p className="text-gray-500">
                                        Periodo: {task.periodo} · Monto:{' '}
                                        {formatCurrency(task.monto)}
                                    </p>
                                </div>
                            </li>
                        ))}
                    </ul>
                )}
            </div>
        </div>
    )
}
