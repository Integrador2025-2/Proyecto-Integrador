import { useState, useEffect } from 'react'
import { Package, DollarSign, Loader2 } from 'lucide-react'
import type { MaterialesInsumos, Rubro } from '../../../../../types'
import { apiService } from '../../../../../services/api.service'

interface MaterialesInsumosTabProps {
    projectId: number
    rubros: Rubro[]
}

export default function MaterialesInsumosTab({ projectId, rubros }: MaterialesInsumosTabProps) {
    const [materialesData, setMaterialesData] = useState<MaterialesInsumos[]>([])
    const [isLoading, setIsLoading] = useState(true)

    useEffect(() => {
        const loadData = async () => {
            try {
                setIsLoading(true)
                const projectRubros = rubros.filter((r) => r.actividadId)
                const allMateriales: MaterialesInsumos[] = []
                for (const rubro of projectRubros) {
                    const data = await apiService.getMaterialesInsumosByRubro(rubro.rubroId)
                    allMateriales.push(...data)
                }
                setMaterialesData(allMateriales)
            } catch (error) {
                console.error('Error loading materiales:', error)
                setMaterialesData([])
            } finally {
                setIsLoading(false)
            }
        }
        loadData()
    }, [projectId, rubros])

    const formatCurrency = (amount: number) => {
        return new Intl.NumberFormat('es-CO', { style: 'currency', currency: 'COP', minimumFractionDigits: 0 }).format(amount)
    }

    const getTotalPresupuesto = () => materialesData.reduce((sum, item) => sum + item.total, 0)

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
            <div className="bg-gradient-to-br from-green-50 to-emerald-50 rounded-xl border border-green-200 p-6">
                <div className="flex items-center justify-between">
                    <div>
                        <h3 className="text-lg font-bold text-gray-900 mb-2">Materiales e Insumos</h3>
                        <p className="text-sm text-gray-600">Recursos y suministros necesarios</p>
                    </div>
                    <div className="text-right">
                        <p className="text-sm text-gray-600 uppercase tracking-wide">Total Presupuesto</p>
                        <p className="text-2xl font-bold text-gray-900">{formatCurrency(getTotalPresupuesto())}</p>
                        <p className="text-xs text-gray-600 mt-1">{materialesData.length} items</p>
                    </div>
                </div>
            </div>

            {materialesData.length === 0 ? (
                <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
                    <Package className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                    <h4 className="text-lg font-semibold text-gray-900 mb-2">No hay materiales asignados</h4>
                    <p className="text-sm text-gray-600">Este proyecto aún no tiene materiales o insumos asignados.</p>
                </div>
            ) : (
                <div className="bg-white rounded-xl border border-gray-200 overflow-hidden">
                    <div className="overflow-x-auto">
                        <table className="w-full">
                            <thead className="bg-gray-50 border-b border-gray-200">
                                <tr>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Materiales</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Período</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Presupuesto</th>
                                    <th className="px-6 py-3 text-left text-xs font-medium text-gray-500 uppercase tracking-wider">Estado</th>
                                </tr>
                            </thead>
                            <tbody className="bg-white divide-y divide-gray-200">
                                {materialesData.map((item) => (
                                    <tr key={item.materialesInsumosId} className="hover:bg-gray-50 transition-colors">
                                        <td className="px-6 py-4">
                                            <div className="flex items-center">
                                                <div className="w-10 h-10 bg-green-100 rounded-full flex items-center justify-center mr-3">
                                                    <Package className="w-5 h-5 text-green-600" />
                                                </div>
                                                <div>
                                                    <p className="text-sm font-medium text-gray-900">{item.materiales}</p>
                                                    <p className="text-xs text-gray-500">ID: {item.materialesInsumosId}</p>
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
