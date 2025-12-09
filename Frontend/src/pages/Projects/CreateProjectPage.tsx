import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuthStore } from '../../store/authStore'
import { useProjectStore } from '../../store/projectStore'
import type { Project } from '../../types'
import { apiService } from '../../services/api.service'

type ProjectFormState = {
    codigo: string
    nombre: string
    descripcion: string
    estado: Project['estado']
    investigadorPrincipal: string
    entidadEjecutora: string
    ubicacion: string
    fechaInicio: string
    fechaFin: string
    presupuestoTotal: string
    presupuestoEjecutado: string
}

const initialFormState: ProjectFormState = {
    codigo: '',
    nombre: '',
    descripcion: '',
    estado: 'Planificación',
    investigadorPrincipal: '',
    entidadEjecutora: '',
    ubicacion: '',
    fechaInicio: '',
    fechaFin: '',
    presupuestoTotal: '',
    presupuestoEjecutado: '',
}

export default function CreateProjectPage() {
    const navigate = useNavigate()
    const user = useAuthStore((state) => state.user) // User autenticado [file:2]
    const addProject = useProjectStore((state) => state.addProject) // acción para guardar en global [file:2]

    const [form, setForm] = useState<ProjectFormState>(initialFormState)
    const [error, setError] = useState<string | null>(null)
    const [isSubmitting, setIsSubmitting] = useState(false)

    if (!user) {
        // En teoría ProtectedRoute ya impide esto, pero es un guard extra
        navigate('/login')
        return null
    }

    const handleChange = (
        e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement | HTMLSelectElement>,
    ) => {
        const { name, value } = e.target
        setForm((prev) => ({ ...prev, [name]: value }))
    }

    const validate = (): string | null => {
        if (!form.codigo.trim()) return 'El código del proyecto es obligatorio.'
        if (!form.nombre.trim()) return 'El nombre del proyecto es obligatorio.'
        if (!form.descripcion.trim()) return 'La descripción del proyecto es obligatoria.'

        if (form.presupuestoTotal) {
            const total = Number(form.presupuestoTotal)
            if (Number.isNaN(total) || total <= 0) {
                return 'El presupuesto total debe ser un número positivo.'
            }
        }

        if (form.fechaInicio && form.fechaFin && form.fechaInicio > form.fechaFin) {
            return 'La fecha de inicio no puede ser posterior a la fecha de fin.'
        }

        return null
    }

    const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault()
        setError(null)

        const validationError = validate()
        if (validationError) {
            setError(validationError)
            return
        }

        try {
            setIsSubmitting(true)

            // 1) Crear proyecto mínimo en backend
            const created = await apiService.createProject({ usuarioId: user.id })

            // Suponiendo que `created` es ProyectoDto: { proyectoId, fechaCreacion, usuarioId } [file:1]
            const enrichedProject: Project = {
                id: created.proyectoId ?? created.id, // created es el DTO del backend
                fechaCreacion: created.fechaCreacion ?? new Date().toISOString(),
                usuarioId: created.usuarioId ?? user.id,

                // Campos de negocio solo en frontend por ahora
                codigo: form.codigo.trim(),
                nombre: form.nombre.trim(),
                descripcion: form.descripcion.trim(),
                estado: form.estado,
                investigadorPrincipal: form.investigadorPrincipal.trim() || user.fullName,
                entidadEjecutora: form.entidadEjecutora.trim(),
                ubicacion: form.ubicacion.trim(),
                fechaInicio: form.fechaInicio || undefined,
                fechaFin: form.fechaFin || undefined,
                presupuestoTotal: form.presupuestoTotal ? Number(form.presupuestoTotal) : undefined,
                presupuestoEjecutado: form.presupuestoEjecutado
                    ? Number(form.presupuestoEjecutado)
                    : undefined,

                // Campos que existen en el tipo pero todavía no usamos
                progreso: 0,
                participantes: [],
                objetivos: [],
                documentos: [],
            }

            // 2) Guardar en estado global para que ProjectsPage / ProjectDetailPage lo vean enriquecido [file:1][file:2]
            addProject(enrichedProject)

            // 3) Ir al detalle del proyecto recién creado
            navigate(`/projects/${enrichedProject.id}`)
        } catch (err) {
            setError('No se pudo crear el proyecto. Intenta de nuevo en unos minutos.')
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="p-4">
            <div className="mb-4 flex items-center justify-between">
                <div>
                    <h1 className="text-2xl font-semibold">Nuevo proyecto</h1>
                    <p className="text-sm text-gray-500">
                        Completa la información para registrar un nuevo proyecto.
                    </p>
                </div>
            </div>

            {error && (
                <div className="mb-4 rounded border border-red-300 bg-red-50 px-3 py-2 text-sm text-red-700">
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-6">
                {/* Datos básicos */}
                <section className="border rounded-lg p-4 space-y-4">
                    <h2 className="text-lg font-semibold">Datos básicos</h2>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="codigo">
                                Código
                            </label>
                            <input
                                id="codigo"
                                name="codigo"
                                type="text"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.codigo}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="nombre">
                                Nombre
                            </label>
                            <input
                                id="nombre"
                                name="nombre"
                                type="text"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.nombre}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>

                    <div>
                        <label className="block text-sm font-medium mb-1" htmlFor="descripcion">
                            Descripción
                        </label>
                        <textarea
                            id="descripcion"
                            name="descripcion"
                            rows={3}
                            className="w-full rounded border px-3 py-2 text-sm"
                            value={form.descripcion}
                            onChange={handleChange}
                            disabled={isSubmitting}
                        />
                    </div>
                </section>

                {/* Contexto */}
                <section className="border rounded-lg p-4 space-y-4">
                    <h2 className="text-lg font-semibold">Contexto</h2>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="estado">
                                Estado
                            </label>
                            <select
                                id="estado"
                                name="estado"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.estado}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            >
                                <option value="Planificación">Planificación</option>
                                <option value="En ejecución">En ejecución</option>
                                <option value="En revisión">En revisión</option>
                                <option value="Finalizado">Finalizado</option>
                            </select>
                        </div>

                        <div>
                            <label
                                className="block text-sm font-medium mb-1"
                                htmlFor="investigadorPrincipal"
                            >
                                Investigador principal
                            </label>
                            <input
                                id="investigadorPrincipal"
                                name="investigadorPrincipal"
                                type="text"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.investigadorPrincipal}
                                onChange={handleChange}
                                placeholder={user.fullName}
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label
                                className="block text-sm font-medium mb-1"
                                htmlFor="entidadEjecutora"
                            >
                                Entidad ejecutora
                            </label>
                            <input
                                id="entidadEjecutora"
                                name="entidadEjecutora"
                                type="text"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.entidadEjecutora}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="ubicacion">
                                Ubicación
                            </label>
                            <input
                                id="ubicacion"
                                name="ubicacion"
                                type="text"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.ubicacion}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>
                </section>

                {/* Tiempo y presupuesto */}
                <section className="border rounded-lg p-4 space-y-4">
                    <h2 className="text-lg font-semibold">Tiempo y presupuesto</h2>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="fechaInicio">
                                Fecha de inicio
                            </label>
                            <input
                                id="fechaInicio"
                                name="fechaInicio"
                                type="date"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.fechaInicio}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>

                        <div>
                            <label className="block text-sm font-medium mb-1" htmlFor="fechaFin">
                                Fecha de fin
                            </label>
                            <input
                                id="fechaFin"
                                name="fechaFin"
                                type="date"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.fechaFin}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>

                    <div className="grid gap-4 md:grid-cols-2">
                        <div>
                            <label
                                className="block text-sm font-medium mb-1"
                                htmlFor="presupuestoTotal"
                            >
                                Presupuesto total (COP)
                            </label>
                            <input
                                id="presupuestoTotal"
                                name="presupuestoTotal"
                                type="number"
                                min={0}
                                step="0.01"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.presupuestoTotal}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>

                        <div>
                            <label
                                className="block text-sm font-medium mb-1"
                                htmlFor="presupuestoEjecutado"
                            >
                                Presupuesto ejecutado (COP)
                            </label>
                            <input
                                id="presupuestoEjecutado"
                                name="presupuestoEjecutado"
                                type="number"
                                min={0}
                                step="0.01"
                                className="w-full rounded border px-3 py-2 text-sm"
                                value={form.presupuestoEjecutado}
                                onChange={handleChange}
                                disabled={isSubmitting}
                            />
                        </div>
                    </div>
                </section>

                {/* Botones */}
                <div className="flex justify-end gap-2">
                    <button
                        type="button"
                        className="px-3 py-2 text-sm border rounded"
                        onClick={() => navigate('/projects')}
                        disabled={isSubmitting}
                    >
                        Cancelar
                    </button>
                    <button
                        type="submit"
                        className="px-3 py-2 text-sm rounded bg-blue-600 text-white disabled:opacity-60"
                        disabled={isSubmitting}
                    >
                        {isSubmitting ? 'Creando...' : 'Crear proyecto'}
                    </button>
                </div>
            </form>
        </div>
    )
}
