import { useState, useEffect } from 'react'
import { Server, DollarSign, Loader2 } from 'lucide-react'
import type { ServiciosTecnologicos, Rubro } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface ServiciosTecnologicosTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function ServiciosTecnologicosTab({ projectId, rubros }: ServiciosTecnologicosTabProps) {
    const [serviciosData, setServiciosData] = useState<ServiciosTecnologicos[]>([])
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        const loadData = async () => {
            try {
                setIsLoading(true)
                const projectRubros = rubros.filter((r) => r.actividadId)
                const allServicios: ServiciosTecnologicos[] = []
                for (const rubro of projectRubros) {
                    const data = await apiService.getServiciosTecnologicosByRubro(rubro.rubroId)
                    allServicios.push(...data)
                }
                setServiciosData(allServicios)
            } catch (error) {
                console.error('Error loading servicios:', error)
                setServiciosData([])
            } finally {
                setIsLoading(false)
            }
        }
        loadData()
    }, [projectId, rubros])

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', minimumFractionDigits: 0 }).format(amount)
    }

    const getTotalPresupuesto = () => serviciosData.reduce((sum, item) => sum + item.total, 0)

    const getRagBadgeClass = (ragEstado: string) => {
        switch (ragEstado.toLowerCase()) {
            case 'green': case 'verde': return 'bg-green-100 text-green-800'
            case 'yellow': case 'amarillo': return 'bg-yellow-100 text-yellow-800'
            case 'red': case 'rojo': return 'bg-red-100 text-red-800'
            default: return 'bg-gray-100 text-gray-800'
        }
    }

    if (isLoading) {
        return (
            <div className="min-h-[300px] flex items-center justify-center">
                <Loader2 className="w-8 h-8 text-blue-600 animate-spin" />
            </div>
        )
    }

    return (
        <div className="space-y-6">
            <div className="bg-gradient-to-br from-indigo-50 to-violet-50 rounded-xl border border-indigo-200 p-6">
                <div className="flex items-center justify-between">
                    <div>
                        <h3 className="text-lg font-bold text-gray-900 mb-2">Servicios Tecnológicos</h3>
                        <p className="text-sm text-gray-600">Servicios externos y consultoría</p>
                    </div>
                    <div className="text-right">
                        <p className="text-sm text-gray-600 uppercase tracking-wide">Total Presupuesto</p>
                        <p className="text-2xl font-bold text-gray-900">{formatCurrency(getTotalPresupuesto())}</p>
                        <p className="text-xs text-gray-600 mt-1">{serviciosData.length} servicios</p>
                    </div>
                </div>
            </div>

            {serviciosData.length === 0 ? (
                <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
                    <Server className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                    <h4 className="text-lg font-semibold text-gray-900 mb-2">No hay servicios asignados</h4>
                    <p className="text-sm text-gray-600">Este proyecto aún no tiene servicios tecnológicos contratados.</p>
                </div>
            ) : (
                <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
                    <div className="overflow-x-auto">
                        <table className="w-full">
                            <thead className="bg-gray-50 border-b border-gray-200">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Descripción</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Período</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Presupuesto</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {serviciosData.map((item) => (
                                    <tr key={item.serviciosTecnologicosId} className="hover:bg-gray-50 transition-colors">
                                        <td className="px-6 py-4">
                                            <div className="flex items-center">
                                                <div className="w-10 h-10 bg-indigo-100 rounded-full flex items-center justify-center mr-3">
                                                    <Server className="w-5 h-5 text-indigo-600" />
                                                </div>
                                                <div>
                                                    <p className="text-sm font-medium text-gray-900">{item.descripcion}</p>
                                                    <p className="text-xs text-gray-500">ID: {item.serviciosTecnologicosId}</p>
                                                </div>
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span className="inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium bg-blue-100 text-blue-800">
                                                {item.periodoTipo} {item.periodoNum}
                                            </span>
                                        </td>
                                        <td className="px-6 py-4">
                                            <div className="flex items-center text-sm font-semibold text-gray-900">
                                                <DollarSign className="w-4 h-4 text-green-600 mr-1" />
                                                {formatCurrency(item.total)}
                                            </div>
                                        </td>
                                        <td className="px-6 py-4">
                                            <span className={`inline-flex items-center px-2.5 py-0.5 rounded-full text-xs font-medium ${getRagBadgeClass(item.ragEstado)}`}>
                                                {item.ragEstado}
                                            </span>
                                        </td>
                                    </tr>
                                ))}
                            </tbody>
                        </table>
                    </div>
                </div>
            )}
        </div>
    )
}
