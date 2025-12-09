// src/pages/ProjectDetail/components/tabs/ActivitiesTab.tsx

import { useEffect, useState } from 'react'
import { ChevronDown, ChevronRight, ClipboardList, Clock, User } from 'lucide-react'
import { apiService } from '../../../../services/api.service'
import type { Activity, Task } from '../../../../types'

interface ActivitiesTabProps {
    projectId: number
}

type ActivityWithExtras = Activity & {
    estado?: Activity['estado']
    responsable?: string
    progreso?: number
    fechaInicio?: string
    fechaFin?: string
    tareas?: Task[]
}

export default function ActivitiesTab({ projectId }: ActivitiesTabProps) {
    const [activities, setActivities] = useState<ActivityWithExtras[]>([])
    const [expandedIds, setExpandedIds] = useState<number[]>([])
    const [isLoading, setIsLoading] = useState<boolean>(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        if (!projectId) return

        const loadActivities = async () => {
            try {
                setIsLoading(true)
                setError(null)

                // 1) Cargar actividades reales desde backend
                const backendActivities = await apiService.getActivitiesByProjectId(projectId)

                // 2) Simular campos mock en frontend
                const enrichedActivities: ActivityWithExtras[] = backendActivities.map(
                    (a, index) => {
                        const estados: Activity['estado'][] = [
                            'Pendiente',
                            'En curso',
                            'Completada',
                        ]
                        const estado = estados[index % estados.length]

                        const progreso =
                            estado === 'Completada' ? 100 : estado === 'En curso' ? 50 : 10

                        const today = new Date()
                        const startOffset = index * 7
                        const endOffset = startOffset + 30

                        const fechaInicioMock = new Date(
                            today.getFullYear(),
                            today.getMonth(),
                            today.getDate() + startOffset,
                        ).toISOString()

                        const fechaFinMock = new Date(
                            today.getFullYear(),
                            today.getMonth(),
                            today.getDate() + endOffset,
                        ).toISOString()

                        return {
                            ...a,
                            estado,
                            responsable: a.responsable || `Responsable ${index + 1}`,
                            progreso: a.progreso ?? progreso,
                            fechaInicio: a.fechaInicio ?? fechaInicioMock,
                            fechaFin: a.fechaFin ?? fechaFinMock,
                            tareas: [],
                        }
                    },
                )

                setActivities(enrichedActivities)
            } catch (err) {
                const message =
                    err instanceof Error ? err.message : 'Error al cargar actividades del proyecto'
                setError(message)
                console.error('Error loading activities:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadActivities()
    }, [projectId])

    const toggleExpand = (actividadId: number) => {
        setExpandedIds((prev) =>
            prev.includes(actividadId)
                ? prev.filter((id) => id !== actividadId)
                : [...prev, actividadId],
        )
    }

    const loadTasksForActivity = async (actividadId: number) => {
        try {
            const tareas = await apiService.getTareasByActividad(actividadId)
            setActivities((prev) =>
                prev.map((a) =>
                    a.actividadId === actividadId
                        ? {
                              ...a,
                              tareas,
                          }
                        : a,
                ),
            )
        } catch (err) {
            const message =
                err instanceof Error ? err.message : 'Error al cargar tareas de la actividad'
            setError(message)
            console.error('Error loading tasks:', err)
        }
    }

    const handleExpandClick = (actividadId: number, alreadyExpanded: boolean) => {
        toggleExpand(actividadId)
        if (!alreadyExpanded) {
            void loadTasksForActivity(actividadId)
        }
    }

    const formatDate = (dateString?: string) => {
        if (!dateString) return 'Sin fecha'
        const date = new Date(dateString)
        if (Number.isNaN(date.getTime())) return 'Sin fecha'
        return date.toLocaleDateString('es-ES', {
            day: '2-digit',
            month: 'short',
            year: 'numeric',
        })
    }

    const formatCurrency = (amount?: number) => {
        if (amount == null || Number.isNaN(amount)) return 'Sin informar'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">Cargando actividades...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">Error al cargar actividades</p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!activities.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene actividades registradas.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se creen actividades desde el backend, aparecerán aquí con su presupuesto
                    y tareas asociadas.
                </p>
            </div>
        )
    }

    return (
        <div className="space-y-4">
            {activities.map((activity) => {
                const isExpanded = expandedIds.includes(activity.actividadId)
                const tareas = activity.tareas ?? []
                const totalTareas = tareas.length

                return (
                    <div
                        key={activity.actividadId}
                        className="bg-white border border-gray-200 rounded-xl overflow-hidden"
                    >
                        <button
                            type="button"
                            onClick={() => handleExpandClick(activity.actividadId, isExpanded)}
                            className="w-full flex items-start justify-between px-4 py-3 hover:bg-gray-50 transition text-left"
                        >
                            <div className="flex items-start gap-3">
                                <div className="mt-1">
                                    {isExpanded ? (
                                        <ChevronDown className="w-4 h-4 text-gray-500" />
                                    ) : (
                                        <ChevronRight className="w-4 h-4 text-gray-500" />
                                    )}
                                </div>
                                <div>
                                    <div className="flex flex-wrap items-center gap-2 mb-1">
                                        <span className="text-sm font-semibold text-gray-900">
                                            {activity.nombre}
                                        </span>
                                        {activity.estado && (
                                            <span className="text-[11px] px-2 py-0.5 rounded-full bg-blue-50 text-blue-700 font-medium">
                                                {activity.estado}
                                            </span>
                                        )}
                                    </div>
                                    <p className="text-xs text-gray-600 line-clamp-2">
                                        {activity.descripcion || 'Sin descripción.'}
                                    </p>
                                    <div className="mt-2 flex flex-wrap gap-4 text-[11px] text-gray-500">
                                        <span className="inline-flex items-center gap-1">
                                            <Clock className="w-3 h-3" />
                                            {formatDate(activity.fechaInicio)} –{' '}
                                            {formatDate(activity.fechaFin)}
                                        </span>
                                        <span className="inline-flex items-center gap-1">
                                            <User className="w-3 h-3" />
                                            {activity.responsable || 'Sin responsable'}
                                        </span>
                                        <span className="inline-flex items-center gap-1">
                                            <ClipboardList className="w-3 h-3" />
                                            {totalTareas} tareas
                                        </span>
                                    </div>
                                </div>
                            </div>

                            <div className="ml-4 flex flex-col items-end gap-1">
                                <span className="text-xs text-gray-500 uppercase tracking-wide">
                                    Valor total
                                </span>
                                <span className="text-sm font-semibold text-gray-900">
                                    {formatCurrency(activity.valorTotal)}
                                </span>
                                <div className="mt-1 w-24 bg-gray-200 rounded-full h-1.5 overflow-hidden">
                                    <div
                                        className="h-1.5 rounded-full bg-blue-600"
                                        style={{ width: `${activity.progreso ?? 0}%` }}
                                    />
                                </div>
                            </div>
                        </button>

                        {isExpanded && (
                            <div className="border-t border-gray-200 bg-gray-50 px-4 py-3 text-xs">
                                <p className="text-gray-700 mb-2">
                                    <span className="font-semibold">Justificación:</span>{' '}
                                    {activity.justificacion || 'Sin justificación registrada.'}
                                </p>
                                <p className="text-gray-700 mb-2">
                                    <span className="font-semibold">
                                        Especificaciones técnicas:
                                    </span>{' '}
                                    {activity.especificacionesTecnicas ||
                                        'Sin especificaciones técnicas.'}
                                </p>

                                {tareas.length > 0 ? (
                                    <div className="mt-3">
                                        <p className="font-semibold text-gray-800 mb-1">Tareas</p>
                                        <ul className="space-y-1">
                                            {tareas.map((tarea) => (
                                                <li
                                                    key={tarea.tareaId}
                                                    className="flex items-center justify-between bg-white border border-gray-200 rounded-lg px-3 py-1.5"
                                                >
                                                    <div className="flex flex-col">
                                                        <span className="text-xs font-medium text-gray-900">
                                                            {tarea.nombre}
                                                        </span>
                                                        <span className="text-[11px] text-gray-500">
                                                            {tarea.periodo} •{' '}
                                                            {formatCurrency(tarea.monto)}
                                                        </span>
                                                    </div>
                                                </li>
                                            ))}
                                        </ul>
                                    </div>
                                ) : (
                                    <p className="text-gray-500 italic mt-2">
                                        No hay tareas registradas para esta actividad.
                                    </p>
                                )}
                            </div>
                        )}
                    </div>
                )
            })}
        </div>
    )
}
