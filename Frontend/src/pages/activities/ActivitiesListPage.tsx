// src/pages/activities/ActivitiesListPage.tsx
import { useEffect, useState } from 'react'
import { Link } from 'react-router-dom'
import { Filter, Loader2, Search, XCircle, Calendar, User, Flag } from 'lucide-react'
import type { Activity } from '../../types'
import { apiService } from '../../services/api.service'

type StatusFilter = 'all' | 'pendiente' | 'en-curso' | 'completada'

export default function ActivitiesListPage() {
    const [activities, setActivities] = useState<Activity[]>([])
    const [filtered, setFiltered] = useState<Activity[]>([])
    const [loading, setLoading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [search, setSearch] = useState('')
    const [statusFilter, setStatusFilter] = useState<StatusFilter>('all')

    const loadActivities = async () => {
        setLoading(true)
        setError(null)

        try {
            // Para opción A: usamos todas las actividades del sistema
            const data = await apiService.getActivities()
            const enriched = enrichActivities(data)
            setActivities(enriched)
            setFiltered(applyFilters(enriched, search, statusFilter))
        } catch (err) {
            console.error('Error loading activities:', err)
            setError('No se pudieron cargar las actividades.')
        } finally {
            setLoading(false)
        }
    }

    const enrichActivities = (data: Activity[]): Activity[] => {
        // Enriquecimiento ligero solo frontend: estado/responsable/progreso si faltan
        return data.map((act) => {
            const estado = act.estado || 'Pendiente'
            const progreso =
                act.progreso ?? (estado === 'Completada' ? 100 : estado === 'En curso' ? 50 : 10)

            return {
                ...act,
                estado,
                responsable: act.responsable || 'Sin asignar',
                progreso,
            }
        })
    }

    const applyFilters = (
        list: Activity[],
        searchText: string,
        status: StatusFilter,
    ): Activity[] => {
        let result = [...list]

        if (searchText.trim()) {
            const q = searchText.toLowerCase()
            result = result.filter(
                (a) =>
                    a.nombre.toLowerCase().includes(q) ||
                    (a.descripcion ?? '').toLowerCase().includes(q),
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
                result = result.filter((a) => a.estado === target)
            }
        }

        return result
    }

    useEffect(() => {
        void loadActivities()
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [])

    useEffect(() => {
        setFiltered(applyFilters(activities, search, statusFilter))
    }, [activities, search, statusFilter])

    const handleClearFilters = () => {
        setSearch('')
        setStatusFilter('all')
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex items-center justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900">Actividades</h1>
                    <p className="text-sm text-gray-600">
                        Vista general de las actividades registradas en los proyectos.
                    </p>
                </div>
            </div>

            {/* Filtros */}
            <div className="bg-white border border-gray-200 rounded-xl p-4 flex flex-col gap-3 md:flex-row md:items-center md:justify-between">
                <div className="flex items-center gap-2 flex-1">
                    <div className="relative w-full md:w-72">
                        <Search className="w-4 h-4 text-gray-400 absolute left-3 top-2.5" />
                        <input
                            type="text"
                            value={search}
                            onChange={(e) => setSearch(e.target.value)}
                            placeholder="Buscar por nombre o descripción..."
                            className="w-full pl-9 pr-3 py-2 border rounded-md text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                        />
                    </div>
                    <button
                        type="button"
                        onClick={handleClearFilters}
                        className="inline-flex items-center gap-1 text-xs text-gray-500 hover:text-gray-700"
                    >
                        <XCircle className="w-4 h-4" />
                        Limpiar
                    </button>
                </div>

                <div className="flex items-center gap-2">
                    <Filter className="w-4 h-4 text-gray-500" />
                    <select
                        value={statusFilter}
                        onChange={(e) => setStatusFilter(e.target.value as StatusFilter)}
                        className="border rounded-md px-2 py-1 text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    >
                        <option value="all">Todos los estados</option>
                        <option value="pendiente">Pendiente</option>
                        <option value="en-curso">En curso</option>
                        <option value="completada">Completada</option>
                    </select>
                </div>
            </div>

            {/* Contenido */}
            {loading && (
                <div className="flex items-center justify-center py-12">
                    <Loader2 className="w-6 h-6 text-blue-600 animate-spin" />
                    <span className="ml-2 text-sm text-gray-600">Cargando actividades...</span>
                </div>
            )}

            {!loading && error && (
                <div className="bg-red-50 border border-red-200 text-red-700 px-4 py-3 rounded-md text-sm">
                    {error}
                </div>
            )}

            {!loading && !error && filtered.length === 0 && (
                <div className="bg-white border border-dashed border-gray-300 rounded-xl p-8 text-center text-sm text-gray-500">
                    No se encontraron actividades con los filtros actuales.
                </div>
            )}

            {!loading && !error && filtered.length > 0 && (
                <div className="grid grid-cols-1 md:grid-cols-2 xl:grid-cols-3 gap-4">
                    {filtered.map((activity) => (
                        <Link
                            key={activity.actividadId}
                            to={`/activities/${activity.actividadId}`}
                            className="bg-white border border-gray-200 rounded-xl p-4 hover:shadow-sm transition-shadow"
                        >
                            <div className="flex items-start justify-between gap-2 mb-2">
                                <h3 className="text-sm font-semibold text-gray-900 line-clamp-2">
                                    {activity.nombre}
                                </h3>
                                <span className="inline-flex items-center px-2 py-0.5 rounded-full text-xs font-medium bg-blue-50 text-blue-700">
                                    {activity.estado || 'Pendiente'}
                                </span>
                            </div>
                            <p className="text-xs text-gray-600 line-clamp-2 mb-3">
                                {activity.descripcion || 'Sin descripción.'}
                            </p>

                            <div className="space-y-2 text-xs text-gray-600">
                                <div className="flex items-center gap-1">
                                    <User className="w-3 h-3 text-gray-400" />
                                    <span>{activity.responsable || 'Sin responsable'}</span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Calendar className="w-3 h-3 text-gray-400" />
                                    <span>
                                        {activity.fechaInicio || 'Sin fecha inicio'} –{' '}
                                        {activity.fechaFin || 'Sin fecha fin'}
                                    </span>
                                </div>
                                <div className="flex items-center gap-1">
                                    <Flag className="w-3 h-3 text-gray-400" />
                                    <span>Progreso: {activity.progreso ?? 0}%</span>
                                </div>
                            </div>

                            <div className="mt-3 w-full bg-gray-200 rounded-full h-1.5">
                                <div
                                    className="bg-blue-600 h-1.5 rounded-full"
                                    style={{ width: `${activity.progreso ?? 0}%` }}
                                />
                            </div>
                        </Link>
                    ))}
                </div>
            )}
        </div>
    )
}
