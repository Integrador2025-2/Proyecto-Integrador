import { useState } from 'react'
import { DollarSign, Loader2, AlertCircle, CheckCircle2, Download, FileSpreadsheet } from 'lucide-react'
import { apiService } from '../../services/api.service'

interface GeneratedBudget {
    projectId: number
    totalBudget: number
    activities: Array<{
        name: string
        category: string
        unitValue: number
        quantity: number
        totalValue: number
        justification: string
    }>
    excelPath?: string
    summary: string
}

export default function BudgetGenerationPage() {
    const [projectId, setProjectId] = useState<number | undefined>(undefined)
    const [projectDescription, setProjectDescription] = useState('')
    const [durationYears, setDurationYears] = useState(1)
    const [isGenerating, setIsGenerating] = useState(false)
    const [budget, setBudget] = useState<GeneratedBudget | null>(null)
    const [error, setError] = useState<string | null>(null)

    const handleGenerate = async (e: React.FormEvent) => {
        e.preventDefault()

        if (!projectId) {
            setError('Por favor ingresa el ID del proyecto')
            return
        }

        if (!projectDescription.trim()) {
            setError('Por favor ingresa una descripción del proyecto')
            return
        }

        setIsGenerating(true)
        setError(null)
        setBudget(null)

        try {
            const result = await apiService.generateBudget({
                projectId,
                projectDescription: projectDescription.trim(),
                durationYears,
            })

            setBudget(result)
        } catch (err) {
            console.error('Error generating budget:', err)
            setError(err instanceof Error ? err.message : 'Error al generar el presupuesto')
        } finally {
            setIsGenerating(false)
        }
    }

    const handleReset = () => {
        setProjectId(undefined)
        setProjectDescription('')
        setDurationYears(1)
        setBudget(null)
        setError(null)
    }

    const formatCurrency = (amount: number) =>
        new Intl.NumberFormat('es-CO', {
            style: 'currency',
            currency: 'COP',
            minimumFractionDigits: 0,
        }).format(amount)

    const getCategoryColor = (category: string) => {
        const colors: Record<string, string> = {
            TalentoHumano: 'bg-blue-100 text-blue-700',
            EquiposSoftware: 'bg-purple-100 text-purple-700',
            MaterialesInsumos: 'bg-green-100 text-green-700',
            ServiciosTecnologicos: 'bg-orange-100 text-orange-700',
            CapacitacionEventos: 'bg-pink-100 text-pink-700',
            GastosViaje: 'bg-yellow-100 text-yellow-700',
        }
        return colors[category] || 'bg-gray-100 text-gray-700'
    }

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-6xl mx-auto space-y-6">
                {/* Header */}
                <div className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-12 h-12 bg-green-100 rounded-lg flex items-center justify-center">
                            <DollarSign className="w-6 h-6 text-green-600" />
                        </div>
                        <div>
                            <h1 className="text-2xl font-bold text-gray-900">
                                Generación de Presupuestos con IA
                            </h1>
                            <p className="text-sm text-gray-600">
                                Genera presupuestos completos basados en documentos del proyecto
                            </p>
                        </div>
                    </div>
                </div>

                {/* Generation Form */}
                <form onSubmit={handleGenerate} className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="space-y-4">
                        <div>
                            <label
                                htmlFor="project-id"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                ID del Proyecto *
                            </label>
                            <input
                                id="project-id"
                                type="number"
                                value={projectId || ''}
                                onChange={(e) =>
                                    setProjectId(e.target.value ? parseInt(e.target.value) : undefined)
                                }
                                placeholder="Ej: 1"
                                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                                disabled={isGenerating}
                                required
                            />
                        </div>

                        <div>
                            <label
                                htmlFor="description"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Descripción del Proyecto *
                            </label>
                            <textarea
                                id="description"
                                value={projectDescription}
                                onChange={(e) => setProjectDescription(e.target.value)}
                                placeholder="Describe brevemente el proyecto, sus objetivos, alcance y actividades principales..."
                                rows={5}
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent resize-none"
                                disabled={isGenerating}
                                required
                            />
                            <p className="text-xs text-gray-500 mt-1">
                                Mientras más detallada sea la descripción, mejor será el presupuesto generado
                            </p>
                        </div>

                        <div>
                            <label
                                htmlFor="duration"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Duración del Proyecto (años)
                            </label>
                            <input
                                id="duration"
                                type="number"
                                min="1"
                                max="10"
                                value={durationYears}
                                onChange={(e) => setDurationYears(parseInt(e.target.value) || 1)}
                                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-green-500 focus:border-transparent"
                                disabled={isGenerating}
                            />
                        </div>

                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={isGenerating || !projectId || !projectDescription.trim()}
                                className="flex-1 bg-green-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-green-700 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors flex items-center justify-center gap-2"
                            >
                                {isGenerating ? (
                                    <>
                                        <Loader2 className="w-5 h-5 animate-spin" />
                                        Generando Presupuesto...
                                    </>
                                ) : (
                                    <>
                                        <DollarSign className="w-5 h-5" />
                                        Generar Presupuesto
                                    </>
                                )}
                            </button>

                            <button
                                type="button"
                                onClick={handleReset}
                                disabled={isGenerating}
                                className="px-6 py-3 border border-gray-300 rounded-lg font-medium text-gray-700 hover:bg-gray-50 disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                            >
                                Limpiar
                            </button>
                        </div>
                    </div>
                </form>

                {/* Error Display */}
                {error && (
                    <div className="bg-red-50 border border-red-200 rounded-xl p-4">
                        <div className="flex items-start gap-3">
                            <AlertCircle className="w-5 h-5 text-red-600 mt-0.5 flex-shrink-0" />
                            <div>
                                <h4 className="text-sm font-semibold text-red-900 mb-1">Error</h4>
                                <p className="text-sm text-red-700">{error}</p>
                            </div>
                        </div>
                    </div>
                )}

                {/* Results Display */}
                {budget && (
                    <div className="space-y-6">
                        {/* Success Message */}
                        <div className="bg-green-50 border border-green-200 rounded-xl p-4">
                            <div className="flex items-start gap-3">
                                <CheckCircle2 className="w-5 h-5 text-green-600 mt-0.5 flex-shrink-0" />
                                <div className="flex-1">
                                    <h4 className="text-sm font-semibold text-green-900 mb-1">
                                        Presupuesto Generado Exitosamente
                                    </h4>
                                    <p className="text-sm text-green-700">
                                        El presupuesto ha sido generado con {budget.activities.length}{' '}
                                        actividades
                                    </p>
                                </div>
                                {budget.excelPath && (
                                    <a
                                        href={budget.excelPath}
                                        download
                                        className="flex items-center gap-2 px-4 py-2 bg-green-600 text-white rounded-lg hover:bg-green-700 transition-colors text-sm font-medium"
                                    >
                                        <Download className="w-4 h-4" />
                                        Descargar Excel
                                    </a>
                                )}
                            </div>
                        </div>

                        {/* Budget Summary */}
                        <div className="bg-gradient-to-br from-green-50 to-emerald-50 rounded-xl border border-green-200 p-6">
                            <div className="flex items-center justify-between mb-4">
                                <h3 className="text-lg font-bold text-gray-900">Resumen del Presupuesto</h3>
                                <div className="text-right">
                                    <p className="text-xs text-gray-600 uppercase tracking-wide">
                                        Presupuesto Total
                                    </p>
                                    <p className="text-3xl font-bold text-green-600">
                                        {formatCurrency(budget.totalBudget)}
                                    </p>
                                </div>
                            </div>

                            <div className="bg-white rounded-lg p-4">
                                <p className="text-sm text-gray-800 leading-relaxed whitespace-pre-wrap">
                                    {budget.summary}
                                </p>
                            </div>
                        </div>

                        {/* Activities Table */}
                        <div className="bg-white rounded-xl border border-gray-200 p-6">
                            <h3 className="text-lg font-bold text-gray-900 mb-4">
                                Actividades del Presupuesto ({budget.activities.length})
                            </h3>

                            <div className="overflow-x-auto">
                                <table className="w-full text-sm">
                                    <thead>
                                        <tr className="border-b border-gray-200">
                                            <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                Actividad
                                            </th>
                                            <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                Rubro
                                            </th>
                                            <th className="text-right py-3 px-4 font-semibold text-gray-700">
                                                Cantidad
                                            </th>
                                            <th className="text-right py-3 px-4 font-semibold text-gray-700">
                                                Valor Unitario
                                            </th>
                                            <th className="text-right py-3 px-4 font-semibold text-gray-700">
                                                Valor Total
                                            </th>
                                        </tr>
                                    </thead>
                                    <tbody>
                                        {budget.activities.map((activity, idx) => (
                                            <tr key={idx} className="border-b border-gray-100 hover:bg-gray-50">
                                                <td className="py-3 px-4">
                                                    <div>
                                                        <p className="text-gray-900 font-medium mb-1">
                                                            {activity.name}
                                                        </p>
                                                        <p className="text-xs text-gray-600">
                                                            {activity.justification}
                                                        </p>
                                                    </div>
                                                </td>
                                                <td className="py-3 px-4">
                                                    <span
                                                        className={`inline-flex px-2 py-1 rounded-full text-xs font-medium ${getCategoryColor(
                                                            activity.category
                                                        )}`}
                                                    >
                                                        {activity.category}
                                                    </span>
                                                </td>
                                                <td className="py-3 px-4 text-right text-gray-900">
                                                    {activity.quantity}
                                                </td>
                                                <td className="py-3 px-4 text-right text-gray-900">
                                                    {formatCurrency(activity.unitValue)}
                                                </td>
                                                <td className="py-3 px-4 text-right font-semibold text-gray-900">
                                                    {formatCurrency(activity.totalValue)}
                                                </td>
                                            </tr>
                                        ))}
                                    </tbody>
                                    <tfoot>
                                        <tr className="border-t-2 border-gray-300">
                                            <td colSpan={4} className="py-3 px-4 text-right font-bold text-gray-900">
                                                TOTAL:
                                            </td>
                                            <td className="py-3 px-4 text-right font-bold text-green-600 text-lg">
                                                {formatCurrency(budget.totalBudget)}
                                            </td>
                                        </tr>
                                    </tfoot>
                                </table>
                            </div>
                        </div>
                    </div>
                )}

                {/* Empty State */}
                {!budget && !error && !isGenerating && (
                    <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
                        <FileSpreadsheet className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                        <h3 className="text-lg font-semibold text-gray-900 mb-2">
                            Sin presupuesto generado
                        </h3>
                        <p className="text-sm text-gray-600 max-w-md mx-auto">
                            Completa el formulario y haz clic en "Generar Presupuesto" para crear un
                            presupuesto basado en IA.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
