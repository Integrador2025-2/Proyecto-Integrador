import { Plus, Search, Filter, FolderKanban } from 'lucide-react'
import { useNavigate } from 'react-router-dom'

import { useProjects } from '../../hooks/useProjects'
import { useProjectsStore } from '../../store/projectStore'
import ProjectCard from './ProjectCard'

type ProjectFilter = 'all' | 'En ejecución' | 'En revisión' | 'Finalizado' | 'Planificación'

export default function ProjectsPage() {
    const {
        projects,
        isLoading,
        error,
        searchQuery,
        setSearchQuery,
        statusFilter,
        setStatusFilter,
        getProjectStats,
    } = useProjects()

    const { projects: enrichedProjects } = useProjectsStore((state) => state)

    const mergedProjects = projects.map((p) => {
        const enriched = enrichedProjects.find((ep) => ep.id === p.id)
        return enriched ?? p
    })

    const navigate = useNavigate()
    const stats = getProjectStats(mergedProjects)

    const filters: { label: string; value: ProjectFilter; count: number }[] = [
        { label: 'Todos', value: 'all', count: stats.total },
        { label: 'En Ejecución', value: 'En ejecución', count: stats.enEjecucion },
        { label: 'En Revisión', value: 'En revisión', count: stats.enRevision },
        { label: 'Finalizados', value: 'Finalizado', count: stats.finalizados },
        { label: 'Planificación', value: 'Planificación', count: stats.planificacion },
    ]

    if (isLoading) {
        return (
            <div className="min-h-[60vh] flex items-center justify-center">
                <p className="text-gray-600 text-lg">Cargando proyectos...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[60vh] flex flex-col items-center justify-center gap-2">
                <p className="text-red-600 font-medium">Error al cargar los proyectos</p>
                <p className="text-sm text-gray-600">{String(error)}</p>
            </div>
        )
    }

    const hasProjects = mergedProjects && mergedProjects.length > 0

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4">
                <div>
                    <h1 className="text-2xl font-bold text-gray-900 flex items-center gap-2">
                        <FolderKanban className="w-6 h-6 text-blue-600" />
                        Proyectos
                    </h1>
                    <p className="text-sm text-gray-600">
                        Gestiona tus proyectos de investigación desde una sola vista.
                    </p>
                </div>

                <button
                    type="button"
                    onClick={() => navigate('/projects/new')}
                    className="inline-flex items-center gap-2 px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition"
                >
                    <Plus className="w-4 h-4" />
                    Nuevo proyecto
                </button>
            </div>

            {/* Filtros y búsqueda */}
            <div className="flex flex-col lg:flex-row lg:items-center lg:justify-between gap-4">
                {/* Búsqueda */}
                <div className="relative max-w-md w-full">
                    <Search className="absolute left-3 top-2.5 w-4 h-4 text-gray-400" />
                    <input
                        type="text"
                        placeholder="Buscar por nombre, código o investigador..."
                        value={searchQuery}
                        onChange={(e) => setSearchQuery(e.target.value)}
                        className="w-full pl-9 pr-3 py-2 border border-gray-300 rounded-lg text-sm focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-blue-500"
                    />
                </div>

                {/* Filtros por estado */}
                <div className="flex flex-wrap items-center gap-2">
                    <span className="inline-flex items-center gap-1 text-xs font-medium text-gray-500 uppercase tracking-wide">
                        <Filter className="w-3 h-3" />
                        Estado
                    </span>

                    {filters.map((filter) => (
                        <button
                            key={filter.value}
                            type="button"
                            onClick={() => setStatusFilter(filter.value)}
                            className={`inline-flex items-center gap-2 px-3 py-1.5 rounded-full text-xs font-medium border transition ${
                                statusFilter === filter.value
                                    ? 'bg-blue-50 border-blue-500 text-blue-700'
                                    : 'bg-white border-gray-200 text-gray-600 hover:bg-gray-50'
                            }`}
                        >
                            <span>{filter.label}</span>
                            <span className="px-1.5 py-0.5 rounded-full bg-gray-100 text-[10px] font-semibold text-gray-700">
                                {filter.count}
                            </span>
                        </button>
                    ))}
                </div>
            </div>

            {/* Lista de proyectos */}
            {hasProjects ? (
                <div className="grid gap-4 md:grid-cols-2 xl:grid-cols-3">
                    {mergedProjects.map((project) => (
                        <ProjectCard key={project.id} project={project} />
                    ))}
                </div>
            ) : (
                <div className="min-h-[40vh] flex flex-col items-center justify-center text-center gap-3 border border-dashed border-gray-300 rounded-2xl bg-gray-50">
                    <p className="text-lg font-semibold text-gray-800">
                        No se encontraron proyectos
                    </p>
                    <p className="text-sm text-gray-600 max-w-md">
                        {searchQuery || statusFilter !== 'all'
                            ? 'Intenta cambiar los filtros o limpiar la búsqueda.'
                            : 'Comienza creando tu primer proyecto para gestionar actividades, presupuesto y equipo.'}
                    </p>
                    {!searchQuery && statusFilter === 'all' && (
                        <button
                            type="button"
                            onClick={() => navigate('/projects/new')}
                            className="inline-flex items-center gap-2 px-4 py-2 rounded-lg bg-blue-600 text-white hover:bg-blue-700 transition"
                        >
                            <Plus className="w-4 h-4" />
                            Crear proyecto
                        </button>
                    )}
                </div>
            )}
        </div>
    )
}
