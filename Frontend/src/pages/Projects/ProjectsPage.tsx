import { Plus, Search, Filter, FolderKanban } from 'lucide-react'
import { useProjects } from '../../hooks/useProjects'
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

    const stats = getProjectStats()

    const filters: { label: string; value: ProjectFilter; count: number }[] = [
        { label: 'Todos', value: 'all', count: stats.total },
        { label: 'En Ejecución', value: 'En ejecución', count: stats.enEjecucion },
        { label: 'En Revisión', value: 'En revisión', count: stats.enRevision },
        { label: 'Finalizados', value: 'Finalizado', count: stats.finalizados },
        { label: 'Planificación', value: 'Planificación', count: stats.planificacion },
    ]

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-96">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Cargando proyectos...</p>
                </div>
            </div>
        )
    }

    if (error) {
        return (
            <div className="flex items-center justify-center h-96">
                <div className="text-center">
                    <div className="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
                        <span className="text-2xl">❌</span>
                    </div>
                    <p className="text-red-600 font-medium">{error}</p>
                </div>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            {/* Header */}
            <div className="flex items-center justify-between">
                <div>
                    <h1 className="text-3xl font-bold text-gray-900">Proyectos</h1>
                    <p className="text-gray-600 mt-1">Gestiona tus proyectos de investigación</p>
                </div>
                <button className="flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition">
                    <Plus className="w-5 h-5" />
                    Nuevo Proyecto
                </button>
            </div>

            {/* Search and Filters */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-6">
                {/* Search Bar */}
                <div className="mb-6">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
                        <input
                            type="text"
                            placeholder="Buscar por nombre, código o descripción..."
                            value={searchQuery}
                            onChange={(e) => setSearchQuery(e.target.value)}
                            className="w-full pl-10 pr-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        />
                    </div>
                </div>

                {/* Status Filters */}
                <div>
                    <div className="flex items-center gap-2 mb-3">
                        <Filter className="w-4 h-4 text-gray-500" />
                        <span className="text-sm font-medium text-gray-700">
                            Filtrar por estado:
                        </span>
                    </div>
                    <div className="flex flex-wrap gap-2">
                        {filters.map((filter) => (
                            <button
                                key={filter.value}
                                onClick={() => setStatusFilter(filter.value)}
                                className={`px-4 py-2 rounded-lg text-sm font-medium transition ${
                                    statusFilter === filter.value
                                        ? 'bg-blue-600 text-white'
                                        : 'bg-gray-100 text-gray-700 hover:bg-gray-200'
                                }`}
                            >
                                {filter.label}
                                <span className="ml-2 px-2 py-0.5 rounded-full text-xs bg-white/20">
                                    {filter.count}
                                </span>
                            </button>
                        ))}
                    </div>
                </div>
            </div>

            {/* Projects Grid */}
            {projects.length === 0 ? (
                <div className="bg-white rounded-xl shadow-sm border border-gray-200 p-12">
                    <div className="text-center">
                        <FolderKanban className="w-16 h-16 mx-auto mb-4 text-gray-300" />
                        <h3 className="text-lg font-semibold text-gray-900 mb-2">
                            No se encontraron proyectos
                        </h3>
                        <p className="text-gray-600 mb-6">
                            {searchQuery || statusFilter !== 'all'
                                ? 'Intenta cambiar los filtros de búsqueda'
                                : 'Comienza creando tu primer proyecto'}
                        </p>
                        {!searchQuery && statusFilter === 'all' && (
                            <button className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition">
                                <Plus className="w-5 h-5" />
                                Crear Proyecto
                            </button>
                        )}
                    </div>
                </div>
            ) : (
                <div className="grid grid-cols-1 lg:grid-cols-2 xl:grid-cols-3 gap-6">
                    {projects.map((project) => (
                        <ProjectCard key={project.id} project={project} />
                    ))}
                </div>
            )}

            {/* Results Summary */}
            {projects.length > 0 && (
                <div className="text-center text-sm text-gray-600">
                    Mostrando {projects.length} de {stats.total} proyecto
                    {stats.total !== 1 ? 's' : ''}
                </div>
            )}
        </div>
    )
}
