// src/hooks/useActivities.ts
import { useEffect, useState } from 'react'
import type { Activity } from '../types'
import {apiService} from '../services/api.service'

interface UseActivitiesOptions {
    proyectoId: number
    autoLoad?: boolean
}

interface UseActivitiesResult {
    activities: Activity[]
    loading: boolean
    error: string | null
    reload: () => Promise<void>
}

/**
 * Hook para obtener actividades de un proyecto desde la API real.
 * Útil para vistas globales o tabs que necesiten refrescar actividades.
 */
export function useActivities({
    proyectoId,
    autoLoad = true,
}: UseActivitiesOptions): UseActivitiesResult {
    const [activities, setActivities] = useState<Activity[]>([])
    const [loading, setLoading] = useState<boolean>(false)
    const [error, setError] = useState<string | null>(null)

    const load = async () => {
        if (!proyectoId) return
        setLoading(true)
        setError(null)

        try {
            // Asumiendo que apiService.getActivitiesByProjectId ya existe
            const data = await apiService.getActivitiesByProjectId(proyectoId)
            setActivities(data ?? [])
        } catch (err) {
            console.error('Error loading activities:', err)
            setError('No se pudieron cargar las actividades.')
        } finally {
            setLoading(false)
        }
    }

    useEffect(() => {
        if (autoLoad) {
            void load()
        }
        // eslint-disable-next-line react-hooks/exhaustive-deps
    }, [proyectoId])

    return {
        activities,
        loading,
        error,
        reload: load,
    }
}

export default useActivities
