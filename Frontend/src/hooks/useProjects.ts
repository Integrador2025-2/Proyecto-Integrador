import { useState, useEffect } from 'react'
import { apiService } from '../services/api.service'
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

    useEffect(() => {
        loadProjects()
    }, [user])

    useEffect(() => {
        applyFilters()
    }, [projects, searchQuery, statusFilter])

    const loadProjects = async () => {
        if (!user) return

        try {
            setIsLoading(true)
            setError(null)
            const data = await apiService.getProjectsByUserId(user.id)
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

        if (searchQuery.trim()) {
            const query = searchQuery.toLowerCase()
            filtered = filtered.filter(
                (project) =>
                    (project.nombre || '').toLowerCase().includes(query) ||
                    (project.codigo || '').toLowerCase().includes(query) ||
                    (project.descripcion || '').toLowerCase().includes(query),
            )
        }

        if (statusFilter !== 'all') {
            filtered = filtered.filter((project) => project.estado === statusFilter)
        }

        setFilteredProjects(filtered)
    }

    const getProjectById = async (id: number): Promise<Project | null> => {
        return apiService.getProjectById(id)
    }

    const createProject = async (projectData: { usuarioId: number }): Promise<Project | null> => {
        try {
            const newProject = await apiService.createProject(projectData)
            await loadProjects()
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
            const updatedProject = await apiService.updateProject(id, updates)
            await loadProjects()
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
            await apiService.deleteProject(id)
            await loadProjects()
            return true
        } catch (err) {
            const errorMessage = err instanceof Error ? err.message : 'Error al eliminar proyecto'
            setError(errorMessage)
            console.error('Error deleting project:', err)
            return false
        }
    }

    const getProjectStats = (target: Project[] = projects) => ({
        total: target.length,
        enEjecucion: target.filter((p) => p.estado === 'En ejecución').length,
        enRevision: target.filter((p) => p.estado === 'En revisión').length,
        finalizados: target.filter((p) => p.estado === 'Finalizado').length,
        planificacion: target.filter((p) => p.estado === 'Planificación').length,
    })

    return {
        projects: filteredProjects,
        allProjects: projects,
        isLoading,
        error,
        searchQuery,
        setSearchQuery,
        statusFilter,
        setStatusFilter,
        loadProjects,
        getProjectById,
        createProject,
        updateProject,
        deleteProject,
        getProjectStats,
    }
}
