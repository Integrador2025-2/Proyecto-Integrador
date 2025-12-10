import { useEffect, useState } from 'react'
import type { Rubro } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface BudgetDetailTabProps {
    projectId: number
    rubros: Rubro[]
}

type RubroKey =
    | 'talentoHumano'
    | 'equiposSoftware'
    | 'materialesInsumos'
    | 'serviciosTecnologicos'
    | 'capacitacionEventos'
    | 'gastosViaje'

interface RubroRow {
    id: number
    detalle: string
    cantidad?: number
    total: number
    periodoNum?: number
    periodoTipo?: string
}

interface RubrosData {
    talentoHumano: RubroRow[]
    equiposSoftware: RubroRow[]
    materialesInsumos: RubroRow[]
    serviciosTecnologicos: RubroRow[]
    capacitacionEventos: RubroRow[]
    gastosViaje: RubroRow[]
}

const initialRubrosData: RubrosData = {
    talentoHumano: [],
    equiposSoftware: [],
    materialesInsumos: [],
    serviciosTecnologicos: [],
    capacitacionEventos: [],
    gastosViaje: [],
}

export default function BudgetDetailTab({ projectId, rubros }: BudgetDetailTabProps) {
    const [data, setData] = useState<RubrosData>(initialRubrosData)
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)
    const [activeRubro, setActiveRubro] = useState<RubroKey>('talentoHumano')

    const talentoRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'talento humano'.toLowerCase(),
    )
    const equiposRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'equipos y software'.toLowerCase(),
    )
    const materialesRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'materiales e insumos'.toLowerCase(),
    )
    const serviciosRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'servicios tecnológicos'.toLowerCase(),
    )
    const capacitacionRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'capacitación y eventos'.toLowerCase(),
    )
    const viajesRubro = rubros.find(
        (r) => r.descripcion?.toLowerCase() === 'gastos de viaje'.toLowerCase(),
    )

    useEffect(() => {
        const loadBudgetDetail = async () => {
            try {
                setIsLoading(true)
                setError(null)

                const [talento, equipos, materiales, servicios, capacitacion, viajes] =
                    await Promise.all([
                        talentoRubro
                            ? apiService.getTalentoHumanoByRubro(talentoRubro.rubroId)
                            : Promise.resolve([]),
                        equiposRubro
                            ? apiService.getEquiposSoftwareByRubro(equiposRubro.rubroId)
                            : Promise.resolve([]),
                        materialesRubro
                            ? apiService.getMaterialesInsumosByRubro(materialesRubro.rubroId)
                            : Promise.resolve([]),
                        serviciosRubro
                            ? apiService.getServiciosTecnologicosByRubro(serviciosRubro.rubroId)
                            : Promise.resolve([]),
                        capacitacionRubro
                            ? apiService.getCapacitacionEventosByRubro(capacitacionRubro.rubroId)
                            : Promise.resolve([]),
                        viajesRubro
                            ? apiService.getGastosViajeByRubro(viajesRubro.rubroId)
                            : Promise.resolve([]),
                    ])

                const nextData: RubrosData = {
                    talentoHumano: talento.map((t) => ({
                        id: t.talentoHumanoId,
                        detalle: t.cargoEspecifico,
                        cantidad: t.semanas,
                        total: t.total,
                        periodoNum: t.periodoNum,
                        periodoTipo: t.periodoTipo,
                    })),
                    equiposSoftware: equipos.map((e) => ({
                        id: e.equiposSoftwareId,
                        detalle: e.especificacionesTecnicas,
                        cantidad: e.cantidad,
                        total: e.total,
                        periodoNum: e.periodoNum,
                        periodoTipo: e.periodoTipo,
                    })),
                    materialesInsumos: materiales.map((m) => ({
                        id: m.materialesInsumosId,
                        detalle: m.materiales,
                        total: m.total,
                        periodoNum: m.periodoNum,
                        periodoTipo: m.periodoTipo,
                    })),
                    serviciosTecnologicos: servicios.map((s) => ({
                        id: s.serviciosTecnologicosId,
                        detalle: s.descripcion,
                        total: s.total,
                        periodoNum: s.periodoNum,
                        periodoTipo: s.periodoTipo,
                    })),
                    capacitacionEventos: capacitacion.map((c) => ({
                        id: c.capacitacionEventosId,
                        detalle: c.tema,
                        cantidad: c.cantidad,
                        total: c.total,
                        periodoNum: c.periodoNum,
                        periodoTipo: c.periodoTipo,
                    })),
                    gastosViaje: viajes.map((v) => ({
                        id: v.gastosViajeId,
                        detalle: `Viaje #${v.gastosViajeId}`,
                        total: v.costo,
                        periodoNum: v.periodoNum,
                        periodoTipo: v.periodoTipo,
                    })),
                }

                setData(nextData)
            } catch (err) {
                const message =
                    err instanceof Error ? err.message : 'Error al cargar el detalle de presupuesto'
                setError(message)
                console.error('Error loading budget detail:', err)
            } finally {
                setIsLoading(false)
            }
        }

        void loadBudgetDetail()
    }, [
        projectId,
        talentoRubro?.rubroId,
        equiposRubro?.rubroId,
        materialesRubro?.rubroId,
        serviciosRubro?.rubroId,
        capacitacionRubro?.rubroId,
        viajesRubro?.rubroId,
    ])

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const rubroTabs: { key: RubroKey; label: string }[] = [
        { key: 'talentoHumano', label: 'Talento Humano' },
        { key: 'equiposSoftware', label: 'Equipos y Software' },
        { key: 'materialesInsumos', label: 'Materiales e Insumos' },
        { key: 'serviciosTecnologicos', label: 'Servicios Tecnológicos' },
        { key: 'capacitacionEventos', label: 'Capacitación y Eventos' },
        { key: 'gastosViaje', label: 'Gastos de Viaje' },
    ]

    if (isLoading) {
        return (
            <div className="min-h-[200px] flex items-center justify-center">
                <p className="text-gray-600 text-sm">Cargando detalle de presupuesto...</p>
            </div>
        )
    }

    if (error) {
        return (
            <div className="min-h-[200px] flex flex-col items-center justify-center gap-2 text-center">
                <p className="text-red-600 text-sm font-medium">
                    Error al cargar detalle de presupuesto
                </p>
                <p className="text-xs text-gray-600 max-w-md">{error}</p>
            </div>
        )
    }

    const currentRows = data[activeRubro]
    const totalRubro = currentRows.reduce((sum, r) => sum + (r.total || 0), 0)

    return (
        <div className="space-y-4">
            <div className="border-b border-gray-200">
                <div className="flex space-x-6 overflow-x-auto">
                    {rubroTabs.map((tab) => (
                        <button
                            key={tab.key}
                            type="button"
                            onClick={() => setActiveRubro(tab.key)}
                            className={`px-3 py-2 text-sm border-b-2 whitespace-nowrap ${
                                activeRubro === tab.key
                                    ? 'border-blue-600 text-blue-600'
                                    : 'border-transparent text-gray-600 hover:text-gray-800 hover:border-gray-300'
                            }`}
                        >
                            {tab.label}
                        </button>
                    ))}
                </div>
            </div>

            {currentRows.length === 0 ? (
                <p className="text-sm text-gray-500">
                    No hay ítems registrados para este tipo de rubro en el proyecto.
                </p>
            ) : (
                <div className="bg-white border border-gray-200 rounded-xl overflow-hidden">
                    <table className="min-w-full text-xs">
                        <thead className="bg-gray-50 border-b border-gray-200">
                            <tr>
                                <th className="px-3 py-2 text-left font-semibold text-gray-700">
                                    Detalle
                                </th>
                                <th className="px-3 py-2 text-left font-semibold text-gray-700">
                                    Período
                                </th>
                                <th className="px-3 py-2 text-right font-semibold text-gray-700">
                                    Cantidad
                                </th>
                                <th className="px-3 py-2 text-right font-semibold text-gray-700">
                                    Total
                                </th>
                            </tr>
                        </thead>
                        <tbody>
                            {currentRows.map((row) => (
                                <tr key={row.id} className="border-b border-gray-100">
                                    <td className="px-3 py-2 text-gray-700">{row.detalle}</td>
                                    <td className="px-3 py-2 text-gray-700">
                                        {row.periodoTipo && row.periodoNum
                                            ? `${row.periodoTipo} ${row.periodoNum}`
                                            : 'N/A'}
                                    </td>
                                    <td className="px-3 py-2 text-right text-gray-700">
                                        {row.cantidad ?? '—'}
                                    </td>
                                    <td className="px-3 py-2 text-right font-semibold text-gray-900">
                                        {formatCurrency(row.total || 0)}
                                    </td>
                                </tr>
                            ))}
                        </tbody>
                        <tfoot>
                            <tr className="bg-gray-50">
                                <td
                                    colSpan={3}
                                    className="px-3 py-2 text-right text-xs font-semibold text-gray-700"
                                >
                                    Total del rubro
                                </td>
                                <td className="px-3 py-2 text-right font-bold text-gray-900">
                                    {formatCurrency(totalRubro)}
                                </td>
                            </tr>
                        </tfoot>
                    </table>
                </div>
            )}
        </div>
    )
}
