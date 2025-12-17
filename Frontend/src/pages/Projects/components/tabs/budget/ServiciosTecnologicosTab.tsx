import { useEffect, useState } from 'react'
import { Server, Clock, AlertCircle } from 'lucide-react'
import type { Rubro, ServiciosTecnologicos } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface ServiciosTecnologicosTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function ServiciosTecnologicosTab({
    projectId,
    rubros,
}: ServiciosTecnologicosTabProps) {
    const [serviciosData, setServiciosData] = useState<ServiciosTecnologicos[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    const serviciosRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'servicios tecnológicos'.toLowerCase(),
    )

    useEffect(() => {
        if (!serviciosRubro) {
            setServiciosData([])
            setIsLoading(false)
            return
        }

        const loadServicios = async () => {
            try {
                setIsLoading(true)
                setError(null)

                const data = await apiService.getServiciosTecnologicosByRubro(
                    serviciosRubro.rubroId,
                )
                setServiciosData(data)
            } catch (err) {
                const message =
                    err instanceof Error
                        ? err.message
                        : 'Error al cargar servicios tecnológicos del proyecto'
                setError(message)
                console.error('Error loading servicios tecnológicos:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadServicios()
    }, [serviciosRubro?.rubroId, projectId])

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const getTotalPresupuesto = () =>
        serviciosData.reduce((sum, item) => sum + (item.total || 0), 0)

    if (!serviciosRubro) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-sm text-gray-600">
                    No se encontró el rubro &quot;Servicios Tecnológicos&quot; en la configuración
                    de rubros.
                </p>
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">
                    Cargando información de servicios tecnológicos...
                </p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">
                    Error al cargar servicios tecnológicos
                </p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!serviciosData.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <Server className="w-8 h-8 text-gray-300" />
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene servicios tecnológicos registrados.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se registren servicios tecnológicos desde el backend, aparecerán aquí con
                    su descripción, período y presupuesto.
                </p>
            </div>
        )
    }

    return (
        <div className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-blue-100 flex items-center justify-center">
                        <Server className="w-5 h-5 text-blue-600" />
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
                        <Server className="w-5 h-5 text-green-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Total Servicios
                        </p>
                        <p className="text-lg font-bold text-gray-900">
                            {serviciosData.length} servicios
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
                            {formatCurrency(getTotalPresupuesto() / serviciosData.length)}
                        </p>
                    </div>
                </div>
            </div>

            <div className="bg-white rounded-xl border border-gray-200 p-4">
                <h3 className="text-sm font-semibold text-gray-900 mb-3">
                    Servicios tecnológicos del proyecto
                </h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full text-xs">
                        <thead>
                            <tr className="bg-gray-50 text-gray-600">
                                <th className="px-3 py-2 text-left font-medium">Descripción</th>
                                <th className="px-3 py-2 text-left font-medium">Período</th>
                                <th className="px-3 py-2 text-right font-medium">Presupuesto</th>
                                <th className="px-3 py-2 text-left font-medium">Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {serviciosData.map((item) => (
                                <tr
                                    key={item.serviciosTecnologicosId}
                                    className="border-t border-gray-100"
                                >
                                    <td className="px-3 py-2">
                                        <div className="flex flex-col">
                                            <span className="font-medium text-gray-900">
                                                {item.descripcion}
                                            </span>
                                            <span className="text-[11px] text-gray-500">
                                                ID: {item.serviciosTecnologicosId}
                                            </span>
                                        </div>
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

                {serviciosData.some((s) => s.ragEstado && s.ragEstado.toLowerCase() !== 'ok') && (
                    <div className="mt-3 flex items-start gap-2 text-xs text-yellow-700 bg-yellow-50 border border-yellow-200 rounded-lg px-3 py-2">
                        <AlertCircle className="w-4 h-4 mt-0.5 flex-shrink-0" />
                        <p>
                            Existen servicios tecnológicos marcados con un estado distinto a
                            &quot;OK&quot;. Revisa los detalles para ajustar el presupuesto o la
                            planificación de estos servicios.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
