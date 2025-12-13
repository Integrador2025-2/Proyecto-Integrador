import { useEffect, useState } from 'react'
import type { BackendObjective, Project } from '../../../../types'
import { apiService } from '../../../../services/api.service'
interface ObjectivesTabProps {
    project: Project
}

export function ObjectivesTab({ project }: ObjectivesTabProps) {
    const [objetivos, setObjetivos] = useState<BackendObjective[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        const loadObjectives = async () => {
            try {
                setIsLoading(true)
                setError(null)
                const data = await apiService.getObjectivesByProjectId(project.id)
                setObjetivos(data)
            } catch (err) {
                const message =
                    err instanceof Error ? err.message : 'Error al cargar objetivos del proyecto'
                setError(message)
                console.error('Error loading objectives:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadObjectives()
    }, [project.id])

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">Cargando objetivos del proyecto...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">Error al cargar objetivos</p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!objetivos.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene objetivos registrados en el backend.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se registren objetivos desde el backend, aparecerán aquí con su
                    descripción y resultado esperado.
                </p>
            </div>
        )
    }

    return (
        <div className="space-y-4">
            <div className="flex flex-col gap-1">
                <h3 className="text-sm font-semibold text-gray-900">Objetivos del proyecto</h3>
                <p className="text-xs text-gray-600">
                    Proyecto: <span className="font-medium">{objetivos[0]?.proyectoNombre}</span>
                </p>
            </div>

            <ul className="space-y-3">
                {objetivos.map((obj) => (
                    <li
                        key={obj.objetivoId}
                        className="rounded-xl border border-gray-200 bg-white px-4 py-3 space-y-2"
                    >
                        <div className="flex items-center justify-between gap-2">
                            <h4 className="text-sm font-semibold text-gray-900">{obj.nombre}</h4>
                            <span className="text-[11px] px-2 py-0.5 rounded-full bg-blue-50 text-blue-700 font-medium">
                                Objetivo #{obj.objetivoId}
                            </span>
                        </div>
                        <p className="text-xs text-gray-700">
                            <span className="font-semibold">Descripción: </span>
                            {obj.descripcion}
                        </p>
                        <p className="text-xs text-gray-700">
                            <span className="font-semibold">Resultado esperado: </span>
                            {obj.resultadoEsperado}
                        </p>
                    </li>
                ))}
            </ul>
        </div>
    )
}
