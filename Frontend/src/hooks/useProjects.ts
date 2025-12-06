import { useState, useEffect } from 'react'
import { apiServiceMock } from '../mocks/api.service.mock'
import { useAuthStore } from '../store/authStore'
import type { Project } from '../types'

type ProjectFilter = 'all' | 'En ejecución' | 'En revisión' | 'Finalizado' | 'Planificación'

export const useProjects = () => {
    const user = useAuthStore((state) => state.user)

    const [projects, setProjects] = useState<Project[]>([])
    const [filteredProjects, setFilteredProjects] = useState<Project[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [searchQuery, setSearchQuery] = useState('')
    const [statusFilter, setStatusFilter] = useState<ProjectFilter>('all')

    // Cargar proyectos del usuario
    useEffect(() => {
        loadProjects()
    }, [user])

    // Aplicar filtros cuando cambien
    useEffect(() => {
        applyFilters()
    }, [projects, searchQuery, statusFilter])

    const loadProjects = async () => {
        if (!user) return

        try {
            setIsLoading(true)
            setError(null)
            const data = await apiServiceMock.getProjectsByUserId(user.id)
            setProjects(data)
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al cargar proyectos'
            setError(errorMessage)
            console.error('Error loading projects:', err)
        } finally {
            setIsLoading(false)
        }
    }

    const applyFilters = () => {
        let filtered = [...projects]

        // Filtrar por búsqueda (nombre o código)
        if (searchQuery.trim()) {
            const query = searchQuery.toLowerCase()
            filtered = filtered.filter(
                (project) =>
                    project.nombre.toLowerCase().includes(query) ||
                    project.codigo.toLowerCase().includes(query) ||
                    project.descripcion.toLowerCase().includes(query),
            )
        }

        // Filtrar por estado
        if (statusFilter !== 'all') {
            filtered = filtered.filter((project) => project.estado === statusFilter)
        }

        setFilteredProjects(filtered)
    }

    const getProjectById = async (id: number): Promise<Project | null> => {
        try {
            return await apiServiceMock.getProjectById(id)
        } catch (err) {
            console.error('Error getting project by id:', err)
            return null
        }
    }

    const createProject = async (projectData: Omit<Project, 'id'>): Promise<Project | null> => {
        try {
            const newProject = await apiServiceMock.createProject(projectData)
            await loadProjects() // Recargar lista
            return newProject
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al crear proyecto'
            setError(errorMessage)
            console.error('Error creating project:', err)
            return null
        }
    }

    const updateProject = async (
        id: number,
        updates: Partial<Project>,
    ): Promise<Project | null> => {
        try {
            const updatedProject = await apiServiceMock.updateProject(id, updates)
            await loadProjects() // Recargar lista
            return updatedProject
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al actualizar proyecto'
            setError(errorMessage)
            console.error('Error updating project:', err)
            return null
        }
    }

    const deleteProject = async (id: number): Promise<boolean> => {
        try {
            await apiServiceMock.deleteProject(id)
            await loadProjects() // Recargar lista
            return true
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al eliminar proyecto'
            setError(errorMessage)
            console.error('Error deleting project:', err)
            return false
        }
    }

    // Estadísticas rápidas
    const getProjectStats = () => {
        return {
            total: projects.length,
            enEjecucion: projects.filter((p) => p.estado === 'En ejecución').length,
            enRevision: projects.filter((p) => p.estado === 'En revisión').length,
            finalizados: projects.filter((p) => p.estado === 'Finalizado').length,
            planificacion: projects.filter((p) => p.estado === 'Planificación').length,
        }
    }

    return {
        // Estado
        projects: filteredProjects,
        allProjects: projects,
        isLoading,
        error,

        // Filtros
        searchQuery,
        setSearchQuery,
        statusFilter,
        setStatusFilter,

        // Métodos
        loadProjects,
        getProjectById,
        createProject,
        updateProject,
        deleteProject,
        getProjectStats,
    }
}
