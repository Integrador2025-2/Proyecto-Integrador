import { useEffect, useState } from 'react'
import { Plane, MapPin, Clock, AlertCircle } from 'lucide-react'
import type { GastosViaje, Rubro } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface GastosViajeTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function GastosViajeTab({ projectId, rubros }: GastosViajeTabProps) {
    const [viajesData, setViajesData] = useState<GastosViaje[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    const viajesRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'gastos de viaje'.toLowerCase(),
    )

    useEffect(() => {
        if (!viajesRubro) {
            setViajesData([])
            setIsLoading(false)
            return
        }

        const loadViajes = async () => {
            try {
                setIsLoading(true)
                setError(null)

                const data = await apiService.getGastosViajeByRubro(viajesRubro.rubroId)
                setViajesData(data)
            } catch (err) {
                const message =
                    err instanceof Error
                        ? err.message
                        : 'Error al cargar gastos de viaje del proyecto'
                setError(message)
                console.error('Error loading gastos de viaje:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadViajes()
    }, [viajesRubro?.rubroId, projectId])

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const getTotalPresupuesto = () => viajesData.reduce((sum, item) => sum + (item.costo || 0), 0)

    if (!viajesRubro) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-sm text-gray-600">
                    No se encontró el rubro &quot;Gastos de Viaje&quot; en la configuración de
                    rubros.
                </p>
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">Cargando información de gastos de viaje...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">Error al cargar gastos de viaje</p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!viajesData.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <Plane className="w-8 h-8 text-gray-300" />
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene gastos de viaje registrados.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se registren viajes desde el backend, aparecerán aquí con su costo y
                    período.
                </p>
            </div>
        )
    }

    return (
        <div className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-blue-100 flex items-center justify-center">
                        <Plane className="w-5 h-5 text-blue-600" />
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
                        <MapPin className="w-5 h-5 text-green-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Total Viajes
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {viajesData.length} viajes
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-yellow-100 flex items-center justify-center">
                        <Clock className="w-5 h-5 text-yellow-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Costo Promedio
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {formatCurrency(getTotalPresupuesto() / viajesData.length)}
                        </p>
                    </div>
                </div>
            </div>

            <div className="bg-white rounded-xl border border-gray-200 p-4">
                <h3 className="text-sm font-semibold text-gray-900 mb-3">
                    Gastos de viaje del proyecto
                </h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full text-xs">
                        <thead>
                            <tr className="bg-gray-50 text-gray-600">
                                <th className="px-3 py-2 text-left font-medium">ID</th>
                                <th className="px-3 py-2 text-left font-medium">Período</th>
                                <th className="px-3 py-2 text-right font-medium">Costo</th>
                                <th className="px-3 py-2 text-left font-medium">Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {viajesData.map((item) => (
                                <tr key={item.gastosViajeId} className="border-t border-gray-100">
                                    <td className="px-3 py-2 text-gray-900 font-medium">
                                        {item.gastosViajeId}
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">
                                        {item.periodoTipo} {item.periodoNum}
                                    </td>
                                    <td className="px-3 py-2 text-right font-medium text-gray-900">
                                        {formatCurrency(item.costo)}
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">{item.ragEstado}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                </div>

                {viajesData.some((v) => v.ragEstado && v.ragEstado.toLowerCase() !== 'ok') && (
                    <div className="mt-3 flex items-start gap-2 text-xs text-yellow-700 bg-yellow-50 border border-yellow-200 rounded-lg px-3 py-2">
                        <AlertCircle className="w-4 h-4 mt-0.5 flex-shrink-0" />
                        <p>
                            Existen viajes marcados con un estado distinto a &quot;OK&quot;. Revisa
                            los detalles para ajustar el presupuesto o la planificación de
                            desplazamientos.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
