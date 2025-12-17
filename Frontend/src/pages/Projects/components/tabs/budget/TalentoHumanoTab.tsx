import { useEffect, useState } from 'react'
import { Users, Briefcase, Clock, AlertCircle } from 'lucide-react'
import type { Rubro, TalentoHumano } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface TalentoHumanoTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function TalentoHumanoTab({ projectId, rubros }: TalentoHumanoTabProps) {
    const [talentoData, setTalentoData] = useState<TalentoHumano[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    // Buscar el rubro “Talento Humano”
    const talentoRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'talento humano'.toLowerCase(),
    )

    useEffect(() => {
        if (!talentoRubro) {
            setTalentoData([])
            setIsLoading(false)
            return
        }

        const loadTalento = async () => {
            try {
                setIsLoading(true)
                setError(null)

                // Cargar talento humano por rubro
                const data = await apiService.getTalentoHumanoByRubro(talentoRubro.rubroId)

                // Opcional: si más adelante quieres filtrar por proyecto,
                // aquí podrías filtrar por actividadId vinculadas al projectId.
                setTalentoData(data)
            } catch (err) {
                const message =
                    err instanceof Error
                        ? err.message
                        : 'Error al cargar talento humano del proyecto'
                setError(message)
                console.error('Error loading talento humano:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadTalento()
    }, [talentoRubro?.rubroId, projectId])

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const getTotalPresupuesto = () => talentoData.reduce((sum, item) => sum + (item.total || 0), 0)

    if (!talentoRubro) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-sm text-gray-600">
                    No se encontró el rubro &quot;Talento Humano&quot; en la configuración de
                    rubros.
                </p>
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">Cargando información de talento humano...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">Error al cargar talento humano</p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!talentoData.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <Users className="w-8 h-8 text-gray-300" />
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene personal asignado en el rubro de Talento Humano.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se registren personas en este rubro desde el backend, aparecerán aquí con
                    su cargo, semanas y presupuesto.
                </p>
            </div>
        )
    }

    return (
        <div className="space-y-4">
            {/* Resumen */}
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-blue-100 flex items-center justify-center">
                        <Users className="w-5 h-5 text-blue-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Total Presupuesto
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {formatCurrency(getTotalPresupuesto())}
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-green-100 flex items-center justify-center">
                        <Briefcase className="w-5 h-5 text-green-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Total Personal
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {talentoData.length} personas
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-yellow-100 flex items-center justify-center">
                        <Clock className="w-5 h-5 text-yellow-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Semanas Totales
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {talentoData.reduce((sum, item) => sum + item.semanas, 0)}
                        </p>
                    </div>
                </div>
            </div>

            {/* Tabla de detalle */}
            <div className="bg-white rounded-xl border border-gray-200 p-4">
                <h3 className="text-sm font-semibold text-gray-900 mb-3">
                    Personal asignado al proyecto
                </h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full text-xs">
                        <thead>
                            <tr className="bg-gray-50 text-gray-600">
                                <th className="px-3 py-2 text-left font-medium">Cargo / Rol</th>
                                <th className="px-3 py-2 text-left font-medium">Duración</th>
                                <th className="px-3 py-2 text-left font-medium">Período</th>
                                <th className="px-3 py-2 text-right font-medium">Presupuesto</th>
                                <th className="px-3 py-2 text-left font-medium">Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {talentoData.map((item) => (
                                <tr key={item.talentoHumanoId} className="border-t border-gray-100">
                                    <td className="px-3 py-2">
                                        <div className="flex flex-col">
                                            <span className="font-medium text-gray-900">
                                                {item.cargoEspecifico}
                                            </span>
                                            <span className="text-[11px] text-gray-500">
                                                ID: {item.talentoHumanoId}
                                            </span>
                                        </div>
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">
                                        {item.semanas} semanas
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">
                                        {item.periodoTipo} {item.periodoNum}
                                    </td>
                                    <td className="px-3 py-2 text-right font-medium text-gray-900">
                                        {formatCurrency(item.total)}
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">{item.ragEstado}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                {/* Nota si hay estados de riesgo */}
                {talentoData.some((t) => t.ragEstado && t.ragEstado.toLowerCase() !== 'ok') && (
                    <div className="mt-3 flex items-start gap-2 text-xs text-yellow-700 bg-yellow-50 border border-yellow-200 rounded-lg px-3 py-2">
                        <AlertCircle className="w-4 h-4 mt-0.5 flex-shrink-0" />
                        <p>
                            Existen registros de talento humano marcados con un estado distinto a
                            &quot;OK&quot;. Revisa los detalles para ajustar el presupuesto o la
                            asignación.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
