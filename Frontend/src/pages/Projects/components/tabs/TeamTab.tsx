import { useState } from 'react'
import type { Project, ProjectParticipant } from '../../../../types'
import { useProjectStore } from '../../../../store/projectStore'

interface TeamTabProps {
    project: Project
}

export function TeamTab({ project }: TeamTabProps) {
    const { addParticipant, removeParticipant } = useProjectStore()
    const [nombre, setNombre] = useState('')
    const [rol, setRol] = useState('')
    const [email, setEmail] = useState('')

    const participantes = project.participantes ?? []

    const handleAdd = () => {
        if (!nombre.trim() || !rol.trim()) return

        const nuevo: ProjectParticipant = {
            id: Date.now(),
            nombre,
            rol,
            email,
        }

        addParticipant(project.id, nuevo)

        setNombre('')
        setRol('')
        setEmail('')
    }

    const handleRemove = (participantId: number) => {
        removeParticipant(project.id, participantId)
    }

    return (
        <div className="space-y-4">
            {/* Formulario */}
            <div className="rounded-md border p-4 space-y-3">
                <h3 className="text-sm font-semibold">Agregar miembro del equipo</h3>
                <div className="flex flex-col gap-2 md:flex-row">
                    <input
                        type="text"
                        value={nombre}
                        onChange={(e) => setNombre(e.target.value)}
                        placeholder="Nombre"
                        className="flex-1 border rounded px-2 py-1 text-sm"
                    />
                    <input
                        type="text"
                        value={rol}
                        onChange={(e) => setRol(e.target.value)}
                        placeholder="Rol (Investigador, Auxiliar, etc.)"
                        className="flex-1 border rounded px-2 py-1 text-sm"
                    />
                    <input
                        type="email"
                        value={email}
                        onChange={(e) => setEmail(e.target.value)}
                        placeholder="Correo (opcional)"
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

            {/* Lista */}
            {participantes.length === 0 ? (
                <p className="text-sm text-gray-500">
                    Este proyecto aún no tiene miembros registrados.
                </p>
            ) : (
                <ul className="space-y-2">
                    {participantes.map((p) => (
                        <li
                            key={p.id}
                            className="flex items-center justify-between rounded border px-3 py-2 gap-3"
                        >
                            <div>
                                <p className="text-sm font-medium">{p.nombre}</p>
                                <p className="text-xs text-gray-600">{p.rol}</p>
                                {p.email && <p className="text-xs text-gray-500">{p.email}</p>}
                            </div>

                            <button
                                onClick={() => handleRemove(p.id)}
                                className="text-xs text-red-600 hover:underline"
                            >
                                Eliminar
                            </button>
                        </li>
                    ))}
                </ul>
            )}
        </div>
    )
}
