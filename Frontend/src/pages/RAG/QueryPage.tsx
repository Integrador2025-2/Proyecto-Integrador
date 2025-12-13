import { useState } from 'react'
import { Search, Loader2, FileText, Database, AlertCircle } from 'lucide-react'
import type { RAGQueryRequest, RAGQueryResponse } from '../../types'
import { apiService } from '../../services/api.service'

export default function QueryPage() {
    const [question, setQuestion] = useState('')
    const [projectId, setProjectId] = useState<number | undefined>(undefined)
    const [topK, setTopK] = useState(5)
    const [isLoading, setIsLoading] = useState(false)
    const [response, setResponse] = useState<RAGQueryResponse | null>(null)
    const [error, setError] = useState<string | null>(null)

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault()

        if (!question.trim()) {
            setError('Por favor ingresa una pregunta')
            return
        }

        setIsLoading(true)
        setError(null)
        setResponse(null)

        try {
            const request: RAGQueryRequest = {
                question: question.trim(),
                projectId: projectId || undefined,
                topK,
            }

            const result = await apiService.queryRAG(request)
            setResponse(result)
        } catch (err) {
            console.error('Error querying RAG:', err)
            setError(err instanceof Error ? err.message : 'Error al realizar la consulta')
        } finally {
            setIsLoading(false)
        }
    }

    const handleReset = () => {
        setQuestion('')
        setProjectId(undefined)
        setTopK(5)
        setResponse(null)
        setError(null)
    }

    // Calculate average similarity score
    const averageSimilarity = response?.sources.length
        ? response.sources.reduce((sum, src) => sum + (src.metadata.similarity || 0), 0) /
          response.sources.length
        : 0

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-6xl mx-auto space-y-6">
                {/* Header */}
                <div className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-12 h-12 bg-blue-100 rounded-lg flex items-center justify-center">
                            <Search className="w-6 h-6 text-blue-600" />
                        </div>
                        <div>
                            <h1 className="text-2xl font-bold text-gray-900">
                                Consultas RAG con IA
                            </h1>
                            <p className="text-sm text-gray-600">
                                Realiza preguntas sobre los documentos de tus proyectos
                            </p>
                        </div>
                    </div>
                </div>

                {/* Query Form */}
                <form onSubmit={handleSubmit} className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="space-y-4">
                        <div>
                            <label
                                htmlFor="question"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Pregunta
                            </label>
                            <textarea
                                id="question"
                                value={question}
                                onChange={(e) => setQuestion(e.target.value)}
                                placeholder="Ej: ¿Cuál es el objetivo principal del proyecto? o Genera un RESUMEN EJECUTIVO COMPLETO..."
                                rows={4}
                                className="w-full px-4 py-3 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent resize-none"
                                disabled={isLoading}
                            />
                            <p className="text-xs text-gray-500 mt-1">
                                Tip: Para resúmenes ejecutivos, solicita explícitamente todos los detalles
                                que necesitas
                            </p>
                        </div>

                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div>
                                <label
                                    htmlFor="projectId"
                                    className="block text-sm font-medium text-gray-700 mb-2"
                                >
                                    ID del Proyecto (Opcional)
                                </label>
                                <input
                                    id="projectId"
                                    type="number"
                                    value={projectId || ''}
                                    onChange={(e) =>
                                        setProjectId(
                                            e.target.value ? parseInt(e.target.value) : undefined
                                        )
                                    }
                                    placeholder="Ej: 1"
                                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                    disabled={isLoading}
                                />
                                <p className="text-xs text-gray-500 mt-1">
                                    Filtra documentos de un proyecto específico
                                </p>
                            </div>

                            <div>
                                <label
                                    htmlFor="topK"
                                    className="block text-sm font-medium text-gray-700 mb-2"
                                >
                                    Cantidad de Documentos a Consultar
                                </label>
                                <input
                                    id="topK"
                                    type="number"
                                    min="1"
                                    max="20"
                                    value={topK}
                                    onChange={(e) => setTopK(parseInt(e.target.value) || 5)}
                                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                                    disabled={isLoading}
                                />
                                <p className="text-xs text-gray-500 mt-1">
                                    Rango: 1-20 (recomendado: 5-10)
                                </p>
                            </div>
                        </div>

                        <div className="flex gap-3 pt-2">
                            <button
                                type="submit"
                                disabled={isLoading || !question.trim()}
                                className="flex-1 bg-blue-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-blue-700 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors flex items-center justify-center gap-2"
                            >
                                {isLoading ? (
                                    <>
                                        <Loader2 className="w-5 h-5 animate-spin" />
                                        Consultando...
                                    </>
                                ) : (
                                    <>
                                        <Search className="w-5 h-5" />
                                        Consultar
                                    </>
                                )}
                            </button>

                            <button
                                type="button"
                                onClick={handleReset}
                                disabled={isLoading}
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
                                <h4 className="text-sm font-semibold text-red-900 mb-1">
                                    Error en la Consulta
                                </h4>
                                <p className="text-sm text-red-700">{error}</p>
                            </div>
                        </div>
                    </div>
                )}

                {/* Results Display */}
                {response && (
                    <div className="space-y-6">
                        {/* Answer Card */}
                        <div className="bg-white rounded-xl border border-gray-200 p-6">
                            <div className="flex items-center gap-3 mb-4">
                                <div className="w-10 h-10 bg-green-100 rounded-lg flex items-center justify-center">
                                    <FileText className="w-5 h-5 text-green-600" />
                                </div>
                                <h3 className="text-lg font-bold text-gray-900">Respuesta</h3>
                            </div>

                            <div className="prose prose-sm max-w-none">
                                <div
                                    className="text-gray-800 leading-relaxed whitespace-pre-wrap"
                                    style={{
                                        fontFamily:
                                            '-apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, sans-serif',
                                    }}
                                >
                                    {response.answer}
                                </div>
                            </div>

                            {response.sources.length > 0 && (
                                <div className="mt-4 pt-4 border-t border-gray-200">
                                    <div className="flex items-center gap-2 text-sm text-gray-600">
                                        <Database className="w-4 h-4" />
                                        <span>
                                            Basado en {response.sources.length} documento(s) con
                                            similitud promedio:{' '}
                                            <span className="font-semibold text-blue-600">
                                                {(averageSimilarity * 100).toFixed(1)}%
                                            </span>
                                        </span>
                                    </div>
                                </div>
                            )}
                        </div>

                        {/* Sources Table */}
                        {response.sources.length > 0 && (
                            <div className="bg-white rounded-xl border border-gray-200 p-6">
                                <h3 className="text-lg font-bold text-gray-900 mb-4">
                                    Fuentes Consultadas ({response.sources.length})
                                </h3>

                                <div className="overflow-x-auto">
                                    <table className="w-full text-sm">
                                        <thead>
                                            <tr className="border-b border-gray-200">
                                                <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                    #
                                                </th>
                                                <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                    Documento
                                                </th>
                                                <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                    Proyecto ID
                                                </th>
                                                <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                    Similitud
                                                </th>
                                                <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                                    Contenido
                                                </th>
                                            </tr>
                                        </thead>
                                        <tbody>
                                            {response.sources.map((source, idx) => {
                                                const similarity =
                                                    (source.metadata.similarity || 0) * 100
                                                return (
                                                    <tr
                                                        key={idx}
                                                        className="border-b border-gray-100 hover:bg-gray-50"
                                                    >
                                                        <td className="py-3 px-4 text-gray-600">
                                                            {idx + 1}
                                                        </td>
                                                        <td className="py-3 px-4">
                                                            <span className="text-gray-900 font-medium">
                                                                {source.metadata.filename ||
                                                                    'Sin nombre'}
                                                            </span>
                                                        </td>
                                                        <td className="py-3 px-4 text-gray-600">
                                                            {source.metadata.project_id || '-'}
                                                        </td>
                                                        <td className="py-3 px-4">
                                                            <div className="flex items-center gap-2">
                                                                <div className="flex-1 bg-gray-200 rounded-full h-2 max-w-[100px]">
                                                                    <div
                                                                        className={`h-2 rounded-full ${
                                                                            similarity >= 80
                                                                                ? 'bg-green-500'
                                                                                : similarity >= 60
                                                                                ? 'bg-yellow-500'
                                                                                : 'bg-red-500'
                                                                        }`}
                                                                        style={{
                                                                            width: `${similarity}%`,
                                                                        }}
                                                                    />
                                                                </div>
                                                                <span
                                                                    className={`text-xs font-semibold ${
                                                                        similarity >= 80
                                                                            ? 'text-green-600'
                                                                            : similarity >= 60
                                                                            ? 'text-yellow-600'
                                                                            : 'text-red-600'
                                                                    }`}
                                                                >
                                                                    {similarity.toFixed(1)}%
                                                                </span>
                                                            </div>
                                                        </td>
                                                        <td className="py-3 px-4 max-w-md">
                                                            <p className="text-gray-700 text-xs line-clamp-2">
                                                                {source.content}
                                                            </p>
                                                        </td>
                                                    </tr>
                                                )
                                            })}
                                        </tbody>
                                    </table>
                                </div>
                            </div>
                        )}
                    </div>
                )}

                {/* Empty State */}
                {!response && !error && !isLoading && (
                    <div className="bg-white rounded-xl border border-gray-200 p-12 text-center">
                        <Search className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                        <h3 className="text-lg font-semibold text-gray-900 mb-2">
                            Sin resultados aún
                        </h3>
                        <p className="text-sm text-gray-600 max-w-md mx-auto">
                            Ingresa una pregunta y haz clic en "Consultar" para obtener respuestas
                            basadas en los documentos de tus proyectos.
                        </p>
                    </div>
                )}
            </div>
        </div>
    )
}
