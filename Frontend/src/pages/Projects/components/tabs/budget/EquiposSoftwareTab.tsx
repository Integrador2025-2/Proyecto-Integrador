import { useEffect, useState } from 'react'
import { Monitor, Package, Clock, AlertCircle } from 'lucide-react'
import type { EquiposSoftware, Rubro } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface EquiposSoftwareTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function EquiposSoftwareTab({ projectId, rubros }: EquiposSoftwareTabProps) {
    const [equiposData, setEquiposData] = useState<EquiposSoftware[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    const equiposRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'equipos y software'.toLowerCase(),
    )

    useEffect(() => {
        if (!equiposRubro) {
            setEquiposData([])
            setIsLoading(false)
            return
        }

        const loadEquipos = async () => {
            try {
                setIsLoading(true)
                setError(null)

                const data = await apiService.getEquiposSoftwareByRubro(equiposRubro.rubroId)
                setEquiposData(data)
            } catch (err) {
                const message =
                    err instanceof Error
                        ? err.message
                        : 'Error al cargar equipos y software del proyecto'
                setError(message)
                console.error('Error loading equipos software:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadEquipos()
    }, [equiposRubro?.rubroId, projectId])

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const getTotalPresupuesto = () => equiposData.reduce((sum, item) => sum + (item.total || 0), 0)

    if (!equiposRubro) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-sm text-gray-600">
                    No se encontró el rubro &quot;Equipos y Software&quot; en la configuración de
                    rubros.
                </p>
            </div>
        )
    }

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">
                    Cargando información de equipos y software...
                </p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">
                    Error al cargar equipos y software
                </p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    if (!equiposData.length) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center border border-dashed border-gray-300 rounded-2xl bg-gray-50 px-4">
                <Monitor className="w-8 h-8 text-gray-300" />
                <p className="text-sm font-semibold text-gray-800">
                    Este proyecto aún no tiene equipos o software asignado.
                </p>
                <p className="text-xs text-gray-600 max-w-md">
                    Cuando se registren equipos o licencias de software desde el backend, aparecerán
                    aquí con sus especificaciones, cantidad y presupuesto.
                </p>
            </div>
        )
    }

    const totalUnidades = equiposData.reduce((sum, item) => sum + (item.cantidad || 0), 0)

    return (
        <div className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-blue-100 flex items-center justify-center">
                        <Monitor className="w-5 h-5 text-blue-600" />
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
                        <Package className="w-5 h-5 text-green-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">Total Items</p>
                        <p className="text-lg font-bold text-gray-900">
                            {equiposData.length} items
                        </p>
                    </div>
                </div>

                <div className="bg-white rounded-xl border border-gray-200 p-4 flex items-center gap-3">
                    <div className="w-10 h-10 rounded-lg bg-yellow-100 flex items-center justify-center">
                        <Clock className="w-5 h-5 text-yellow-600" />
                    </div>
                    <div>
                        <p className="text-xs text-gray-500 uppercase tracking-wide">
                            Total Unidades
                        </p>
                        <p className="text-lg font-bold text-gray-900">{totalUnidades}</p>
                    </div>
                </div>
            </div>

            <div className="bg-white rounded-xl border border-gray-200 p-4">
                <h3 className="text-sm font-semibold text-gray-900 mb-3">
                    Infraestructura tecnológica del proyecto
                </h3>
                <div className="overflow-x-auto">
                    <table className="min-w-full text-xs">
                        <thead>
                            <tr className="bg-gray-50 text-gray-600">
                                <th className="px-3 py-2 text-left font-medium">
                                    Especificaciones Técnicas
                                </th>
                                <th className="px-3 py-2 text-left font-medium">Cantidad</th>
                                <th className="px-3 py-2 text-left font-medium">Período</th>
                                <th className="px-3 py-2 text-right font-medium">Presupuesto</th>
                                <th className="px-3 py-2 text-left font-medium">Estado</th>
                            </tr>
                        </thead>
                        <tbody>
                            {equiposData.map((item) => (
                                <tr
                                    key={item.equiposSoftwareId}
                                    className="border-t border-gray-100"
                                >
                                    <td className="px-3 py-2">
                                        <div className="flex flex-col">
                                            <span className="font-medium text-gray-900">
                                                {item.especificacionesTecnicas}
                                            </span>
                                            <span className="text-[11px] text-gray-500">
                                                ID: {item.equiposSoftwareId}
                                            </span>
                                        </div>
                                    </td>
                                    <td className="px-3 py-2 text-gray-700">{item.cantidad}</td>
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

                {equiposData.some((e) => e.ragEstado && e.ragEstado.toLowerCase() !== 'ok') && (
                    <div className="mt-3 flex items-start gap-2 text-xs text-yellow-700 bg-yellow-50 border border-yellow-200 rounded-lg px-3 py-2">
                        <AlertCircle className="w-4 h-4 mt-0.5 flex-shrink-0" />
                        <p>
                            Existen equipos o licencias marcados con un estado distinto a
                            &quot;OK&quot;. Revisa los detalles para ajustar el presupuesto o la
                            planificación de compras.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
