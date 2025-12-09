// src/pages/Projects/components/tabs/TasksTab.tsx

import { useEffect, useState } from 'react'
import { Loader2, Search, Filter, Calendar, User, ListChecks } from 'lucide-react'
import type { Activity, Task } from '../../../../types'
import { apiService } from '../../../../services/api.service'
import useTasks from '../../../../hooks/useTasks'

interface TasksTabProps {
    projectId: number
}

type TaskWithActivity = Task & {
    actividadNombre?: string
    responsableActividad?: string
}

type StatusFilter = 'all' | 'pendiente' | 'en-curso' | 'completada'

export default function TasksTab({ projectId }: TasksTabProps) {
    const [activities, setActivities] = useState<Activity[]>([])
    const [activityId, setActivityId] = useState<number | 'all'>('all')
    const [tasks, setTasks] = useState<TaskWithActivity[]>([])
    const [filteredTasks, setFilteredTasks] = useState<TaskWithActivity[]>([])
    const [loadingActivities, setLoadingActivities] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [search, setSearch] = useState('')
    const [statusFilter, setStatusFilter] = useState<StatusFilter>('all')

    // Hook para cargar tareas de una actividad cuando se selecciona una específica
    const {
        tasks: activityTasks,
        loading: loadingTasksHook,
        error: tasksErrorHook,
        // reload: reloadTasksHook,
    } = useTasks({
        actividadId: typeof activityId === 'number' ? activityId : 0,
        autoLoad: activityId !== 'all',
    })

    useEffect(() => {
        const loadActivities = async () => {
            if (!projectId) return
            setLoadingActivities(true)
            setError(null)
            try {
                const data = await apiService.getActivitiesByProjectId(projectId)
                setActivities(data ?? [])
            } catch (err) {
                console.error('Error loading activities for tasks tab:', err)
                setError('No se pudieron cargar las actividades del proyecto.')
            } finally {
                setLoadingActivities(false)
            }
        }

        void loadActivities()
    }, [projectId])

    // Cargar tareas de todas las actividades cuando activityId = 'all'
    useEffect(() => {
        const loadAllTasks = async () => {
            if (!projectId) return
            if (activityId !== 'all') return

            setError(null)
            try {
                const allTasks: TaskWithActivity[] = []

                for (const act of activities) {
                    const tareas = await apiService.getTareasByActividad(act.actividadId)
                    tareas.forEach((t) =>
                        allTasks.push({
                            ...t,
                            actividadNombre: act.nombre,
                            responsableActividad: act.responsable,
                        }),
                    )
                }

                setTasks(allTasks)
            } catch (err) {
                console.error('Error loading tasks for all activities:', err)
                setError('No se pudieron cargar las tareas de las actividades.')
            }
        }

        void loadAllTasks()
    }, [projectId, activities, activityId])

    // Sincronizar tareas cuando se selecciona una actividad específica
    useEffect(() => {
        if (activityId === 'all') return
        if (!activityId) return

        const selected = activities.find((a) => a.actividadId === activityId)
        const nombreActividad = selected?.nombre
        const responsableActividad = selected?.responsable

        const enriched = activityTasks.map((t) => ({
            ...t,
            actividadNombre: nombreActividad,
            responsableActividad,
        }))

        setTasks(enriched)
    }, [activityId, activityTasks, activities])

    // Filtros
    useEffect(() => {
        setFilteredTasks(applyFilters(tasks, search, statusFilter))
    }, [tasks, search, statusFilter])

    const applyFilters = (
        list: TaskWithActivity[],
        searchText: string,
        status: StatusFilter,
    ): TaskWithActivity[] => {
        let result = [...list]

        if (searchText.trim()) {
            const q = searchText.toLowerCase()
            result = result.filter(
                (t) =>
                    t.nombre.toLowerCase().includes(q) ||
                    (t.descripcion ?? '').toLowerCase().includes(q) ||
                    (t.actividadNombre ?? '').toLowerCase().includes(q),
            )
        }

        if (status !== 'all') {
            const map: Record<StatusFilter, string | null> = {
                all: null,
                pendiente: 'Pendiente',
                'en-curso': 'En curso',
                completada: 'Completada',
            }
            const target = map[status]
            if (target) {
                result = result.filter((t) => t.estado === target)
            }
        }

        return result
    }

    const formatCurrency = (amount?: number) => {
        if (!amount || Number.isNaN(amount)) return '$0'
        return new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)
    }

    const isLoading = loadingActivities || loadingTasksHook
    const combinedError = error || tasksErrorHook

    return (
        <div className="space-y-4">
            {/* Filtros y selección de actividad */}
            <div className="bg-white border border-gray-200 rounded-xl p-4 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                <div className="flex items-center gap-2 flex-1">
                    <div className="relative w-full md:w-72">
                        <Search className="w-4 h-4 text-gray-400 absolute left-3 top-2.5" />
                        <input
                            type="text"
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            placeholder="Buscar por nombre de tarea o actividad..."
                            className="w-full pl-9 pr-3 py-2 border rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>
                </div>

                <div className="flex items-center gap-2">
                    <Filter className="w-4 h-4 text-gray-500" />
                    <select
                        value={activityId}
                        onChange={(e) =>
                            setActivityId(e.target.value === 'all' ? 'all' : Number(e.target.value))
                        }
                        className="border rounded-md px-2 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                        <option value="all">Todas las actividades</option>
                        {activities.map((a) => (
                            <option key={a.actividadId} value={a.actividadId}>
                                {a.nombre}
                            </option>
                        ))}
                    </select>

                    <select
                        value={statusFilter}
                        onChange={(e) => setStatusFilter(e.target.value as StatusFilter)}
                        className="border rounded-md px-2 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                        <option value="all">Todos los estados</option>
                        <option value="pendiente">Pendientes</option>
                        <option value="en-curso">En curso</option>
                        <option value="completada">Completadas</option>
                    </select>
                </div>
            </div>

            {/* Estados de carga / error */}
            {isLoading && (
                <div className="flex items-center justify-center py-8">
                    <Loader2 className="w-5 h-5 text-blue-600 animate-spin" />
                    <span className="ml-2 text-sm text-gray-600">Cargando tareas...</span>
                </div>
            )}

            {!isLoading && combinedError && (
                <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md text-sm">
                    {combinedError}
                </div>
            )}

            {!isLoading && !combinedError && filteredTasks.length === 0 && (
                <div className="bg-white border border-dashed border-gray-300 rounded-xl p-8 text-center text-sm text-gray-500">
                    No hay tareas registradas para los filtros seleccionados.
                </div>
            )}

            {/* Lista de tareas */}
            {!isLoading && !combinedError && filteredTasks.length > 0 && (
                <div className="space-y-3">
                    {filteredTasks.map((task) => (
                        <div
                            key={task.tareaId}
                            className="bg-white border border-gray-200 rounded-lg px-4 py-3 flex flex-col gap-2 md:flex-row md:items-center md:justify-between"
                        >
                            <div className="flex-1 min-w-0">
                                <div className="flex items-center gap-2 mb-1">
                                    <ListChecks className="w-4 h-4 text-gray-500" />
                                    <p className="text-sm font-semibold text-gray-900 truncate">
                                        {task.nombre}
                                    </p>
                                </div>
                                <p className="text-xs text-gray-600 line-clamp-2">
                                    {task.descripcion || 'Sin descripción.'}
                                </p>
                                <p className="text-[11px] text-gray-500 mt-1">
                                    Actividad:{' '}
                                    <span className="font-medium">
                                        {task.actividadNombre || `#${task.actividadId}`}
                                    </span>
                                </p>
                            </div>

                            <div className="mt-2 md:mt-0 flex flex-col items-start md:items-end gap-1 text-xs text-gray-600">
                                <div className="flex items-center gap-1">
                                    <Calendar className="w-3 h-3 text-gray-400" />
                                    <span>{task.periodo}</span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <User className="w-3 h-3 text-gray-400" />
                                    <span>
                                        {task.responsable ||
                                            task.responsableActividad ||
                                            'Sin responsable'}
                                    </span>
                                </div>
                                <div className="font-semibold text-gray-900">
                                    {formatCurrency(task.monto)}
                                </div>
                            </div>
                        </div>
                    ))}
                </div>
            )}
        </div>
    )
}
