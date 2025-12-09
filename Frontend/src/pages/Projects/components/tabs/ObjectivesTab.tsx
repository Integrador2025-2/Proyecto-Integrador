import { useState } from 'react'
import type { Objective, Project } from '../../../../types'
import { useProjectStore } from '../../../../store/projectStore'

interface ObjectivesTabProps {
    project: Project
}

export function ObjectivesTab({ project }: ObjectivesTabProps) {
    const { addObjective, updateObjective, removeObjective } = useProjectStore()
    const [descripcion, setDescripcion] = useState('')
    const [tipo, setTipo] = useState<Objective['tipo']>('General')

    const objetivos = project.objetivos ?? []

    const handleAdd = () => {
        if (!descripcion.trim()) return

        addObjective(project.id, {
            id: Date.now(),
            tipo,
            descripcion,
            estado: 'Pendiente',
            progreso: 0,
        })

        setDescripcion('')
        setTipo('General')
    }

    const toggleEstado = (obj: Objective) => {
        const nextEstado: Objective['estado'] =
            obj.estado === 'Completado' ? 'En progreso' : 'Completado'

        updateObjective(project.id, obj.id, {
            estado: nextEstado,
            progreso: nextEstado === 'Completado' ? 100 : obj.progreso ?? 0,
        })
    }

    const handleRemove = (obj: Objective) => {
        removeObjective(project.id, obj.id)
    }

    return (
        <div className="space-y-4">
            {/* Formulario simple */}
            <div className="rounded-md border p-4 space-y-3">
                <h3 className="text-sm font-semibold">Nuevo objetivo</h3>
                <div className="flex flex-col gap-2 md:flex-row">
                    <select
                        value={tipo}
                        onChange={(e) => setTipo(e.target.value as Objective['tipo'])}
                        className="border rounded px-2 py-1 text-sm"
                    >
                        <option value="General">General</option>
                        <option value="Específico">Específico</option>
                    </select>
                    <input
                        type="text"
                        value={descripcion}
                        onChange={(e) => setDescripcion(e.target.value)}
                        placeholder="Descripción del objetivo"
                        className="flex-1 border rounded px-2 py-1 text-sm"
                    />
                    <button
                        onClick={handleAdd}
                        className="bg-primary text-white text-sm px-3 py-1 rounded"
                    >
                        Agregar
                    </button>
                </div>
            </div>

            {/* Lista de objetivos */}
            {objetivos.length === 0 ? (
                <p className="text-sm text-gray-500">
                    Este proyecto aún no tiene objetivos registrados.
                </p>
            ) : (
                <ul className="space-y-2">
                    {objetivos.map((obj) => (
                        <li
                            key={obj.id}
                            className="flex items-start justify-between rounded border px-3 py-2 gap-3"
                        >
                            <div className="space-y-1">
                                <div className="flex items-center gap-2">
                                    <span className="text-xs uppercase tracking-wide text-gray-500">
                                        {obj.tipo}
                                    </span>
                                    <span
                                        className={`text-xs px-2 py-0.5 rounded-full ${
                                            obj.estado === 'Completado'
                                                ? 'bg-green-100 text-green-700'
                                                : obj.estado === 'En progreso'
                                                ? 'bg-blue-100 text-blue-700'
                                                : 'bg-yellow-100 text-yellow-700'
                                        }`}
                                    >
                                        {obj.estado}
                                    </span>
                                </div>
                                <p className="text-sm">{obj.descripcion}</p>
                                <div className="w-full bg-gray-100 rounded h-1.5 overflow-hidden">
                                    <div
                                        className="h-1.5 bg-primary"
                                        style={{ width: `${obj.progreso ?? 0}%` }}
                                    />
                                </div>
                            </div>

                            <div className="flex flex-col items-end gap-1">
                                <button
                                    onClick={() => toggleEstado(obj)}
                                    className="text-xs text-blue-600 hover:underline"
                                >
                                    {obj.estado === 'Completado'
                                        ? 'Marcar en progreso'
                                        : 'Marcar completado'}
                                </button>
                                <button
                                    onClick={() => handleRemove(obj)}
                                    className="text-xs text-red-600 hover:underline"
                                >
                                    Eliminar
                                </button>
                            </div>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    )
}
