import { useState, type ChangeEvent } from 'react'

import type { Project, ProjectDocument } from '../../../../types'
import { useProjectStore } from '../../../../store/projectStore'

interface DocumentsTabProps {
    project: Project
}

export function DocumentsTab({ project }: DocumentsTabProps) {
    const { addDocument, removeDocument } = useProjectStore()
    const [nombre, setNombre] = useState('')
    const [tipo, setTipo] = useState('Otro')

    const documentos = project.documentos ?? []

    const handleFileChange = (e: ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0]
        if (!file) return

        const now = new Date().toISOString()
        const doc: ProjectDocument = {
            id: Date.now(),
            nombre: nombre || file.name,
            tipo,
            descripcion: undefined,
            tamano: file.size,
            fechaSubida: now,
            url: URL.createObjectURL(file), // solo mock en frontend
        }

        addDocument(project.id, doc)
        setNombre('')
        setTipo('Otro')
        e.target.value = ''
    }

    const handleRemove = (docId: number) => {
        removeDocument(project.id, docId)
    }

    return (
        <div className="space-y-4">
            {/* Carga mock */}
            <div className="rounded-md border p-4 space-y-3">
                <h3 className="text-sm font-semibold">Agregar documento</h3>
                <div className="flex flex-col gap-2 md:flex-row">
                    <input
                        type="text"
                        value={nombre}
                        onChange={(e) => setNombre(e.target.value)}
                        placeholder="Nombre del documento (opcional)"
                        className="flex-1 border rounded px-2 py-1 text-sm"
                    />
                    <select
                        value={tipo}
                        onChange={(e) => setTipo(e.target.value)}
                        className="border rounded px-2 py-1 text-sm"
                    >
                        <option value="Acta">Acta</option>
                        <option value="Informe">Informe</option>
                        <option value="Contrato">Contrato</option>
                        <option value="Soporte">Soporte</option>
                        <option value="Otro">Otro</option>
                    </select>
                    <input type="file" onChange={handleFileChange} className="text-sm" />
                </div>
                <p className="text-xs text-gray-500">
                    La carga es solo simulada en frontend; los archivos no se envían aún al backend.
                </p>
            </div>

            {/* Lista */}
            {documentos.length === 0 ? (
                <p className="text-sm text-gray-500">
                    Este proyecto aún no tiene documentos registrados.
                </p>
            ) : (
                <table className="w-full text-sm border-collapse">
                    <thead>
                        <tr className="border-b text-xs text-gray-500">
                            <th className="text-left py-1 px-2">Nombre</th>
                            <th className="text-left py-1 px-2">Tipo</th>
                            <th className="text-left py-1 px-2">Fecha</th>
                            <th className="text-left py-1 px-2">Tamaño</th>
                            <th className="text-right py-1 px-2">Acciones</th>
                        </tr>
                    </thead>
                    <tbody>
                        {documentos.map((d) => (
                            <tr key={d.id} className="border-b last:border-0">
                                <td className="py-1 px-2">{d.nombre}</td>
                                <td className="py-1 px-2">{d.tipo}</td>
                                <td className="py-1 px-2">
                                    {new Date(d.fechaSubida).toLocaleDateString()}
                                </td>
                                <td className="py-1 px-2">
                                    {d.tamano ? `${(d.tamano / 1024).toFixed(1)} KB` : '-'}
                                </td>
                                <td className="py-1 px-2 text-right space-x-2">
                                    {d.url && (
                                        <a
                                            href={d.url}
                                            target="_blank"
                                            rel="noreferrer"
                                            className="text-xs text-blue-600 hover:underline"
                                        >
                                            Ver
                                        </a>
                                    )}
                                    <button
                                        onClick={() => handleRemove(d.id)}
                                        className="text-xs text-red-600 hover:underline"
                                    >
                                        Eliminar
                                    </button>
                                </td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    )
}
