// src/hooks/useTasks.ts
import { useEffect, useState } from 'react'
import type { Task } from '../types'
import { apiService } from '../services/api.service'

interface UseTasksOptions {
    actividadId: number
    autoLoad?: boolean
}

interface UseTasksResult {
    tasks: Task[]
    loading: boolean
    error: string | null
    reload: () => Promise<void>
    createTask: (input: Omit<Task, 'tareaId'>) => Promise<void>
    updateTask: (taskId: number, input: Partial<Task>) => Promise<void>
    deleteTask: (taskId: number) => Promise<void>
}

/**
 * Hook para gestionar tareas de una actividad usando la API real.
 * Encapsula carga, creación, actualización y eliminación.
 */
export function useTasks({ actividadId, autoLoad = true }: UseTasksOptions): UseTasksResult {
    const [tasks, setTasks] = useState<Task[]>([])
    const [loading, setLoading] = useState<boolean>(false)
    const [error, setError] = useState<string | null>(null)

    const load = async () => {
        if (!actividadId) return
        setLoading(true)
        setError(null)

        try {
            // Asumiendo que apiService tiene este método:
            // getTasksByActivityId(actividadId: number): Promise<Task[]>
            const data = await apiService.getTareasByActividad(actividadId)
            setTasks(data ?? [])
        } catch (err) {
            console.error('Error loading tasks:', err)
            setError('No se pudieron cargar las tareas.')
        } finally {
            setLoading(false)
        }
    }

    const createTask = async (input: Omit<Task, 'tareaId'>) => {
        setError(null)
        try {
            // Asumiendo que apiService.createTask devuelve la tarea creada
            const created = await apiService.createTask({
                ...input,
                actividadId,
            })

            if (created) {
                setTasks((prev) => [...prev, created])
            } else {
                // Si el backend no devuelve nada, recargar lista
                await load()
            }
        } catch (err) {
            console.error('Error creating task:', err)
            setError('No se pudo crear la tarea.')
        }
    }

    const updateTask = async (taskId: number, input: Partial<Task>) => {
        setError(null)
        try {
            // Asumiendo que apiService.updateTask no necesariamente devuelve la tarea
            const updated = await apiService.updateTask(taskId, input)

            if (updated) {
                setTasks((prev) =>
                    prev.map((t) => (t.tareaId === taskId ? { ...t, ...updated } : t)),
                )
            } else {
                await load()
            }
        } catch (err) {
            console.error('Error updating task:', err)
            setError('No se pudo actualizar la tarea.')
        }
    }

    const deleteTask = async (taskId: number) => {
        setError(null)
        try {
            await apiService.deleteTask(taskId)
            setTasks((prev) => prev.filter((t) => t.tareaId !== taskId))
        } catch (err) {
            console.error('Error deleting task:', err)
            setError('No se pudo eliminar la tarea.')
        }
    }

    useEffect(() => {
        if (autoLoad) {
            void load()
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [actividadId])

    return {
        tasks,
        loading,
        error,
        reload: load,
        createTask,
        updateTask,
        deleteTask,
    }
}

export default useTasks
