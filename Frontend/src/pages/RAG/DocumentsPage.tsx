import { useState, useEffect } from 'react'
import { Upload, FileText, Trash2, Loader2, AlertCircle, CheckCircle2, Download } from 'lucide-react'
import { apiService } from '../../services/api.service'

interface Document {
    id: number
    filename: string
    documentType: string
    uploadDate: string
    projectId?: number
    fileSize?: number
}

export default function DocumentsPage() {
    const [documents, setDocuments] = useState<Document[]>([])
    const [isLoading, setIsLoading] = useState(false)
    const [isUploading, setIsUploading] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState<string | null>(null)
    const [projectIdFilter, setProjectIdFilter] = useState<number | undefined>(undefined)

    // Upload form state
    const [selectedFile, setSelectedFile] = useState<File | null>(null)
    const [uploadProjectId, setUploadProjectId] = useState<number | undefined>(undefined)
    const [documentType, setDocumentType] = useState('project_document')

    useEffect(() => {
        if (projectIdFilter) {
            loadDocuments()
        }
    }, [projectIdFilter])

    const loadDocuments = async () => {
        if (!projectIdFilter) return

        setIsLoading(true)
        setError(null)

        try {
            const docs = await apiService.getProjectDocuments(projectIdFilter)
            setDocuments(docs)
        } catch (err) {
            console.error('Error loading documents:', err)
            setError(err instanceof Error ? err.message : 'Error al cargar documentos')
        } finally {
            setIsLoading(false)
        }
    }

    const handleFileSelect = (e: React.ChangeEvent<HTMLInputElement>) => {
        const file = e.target.files?.[0]
        if (file) {
            setSelectedFile(file)
            setError(null)
        }
    }

    const handleUpload = async (e: React.FormEvent) => {
        e.preventDefault()

        if (!selectedFile) {
            setError('Por favor selecciona un archivo')
            return
        }

        setIsUploading(true)
        setError(null)
        setSuccess(null)

        try {
            await apiService.uploadDocument(
                selectedFile,
                uploadProjectId,
                documentType
            )

            setSuccess(`Documento "${selectedFile.name}" subido exitosamente`)
            setSelectedFile(null)
            setUploadProjectId(undefined)
            setDocumentType('project_document')

            // Reset file input
            const fileInput = document.getElementById('file-upload') as HTMLInputElement
            if (fileInput) fileInput.value = ''

            // Reload documents if we're viewing the same project
            if (projectIdFilter === uploadProjectId) {
                await loadDocuments()
            }
        } catch (err) {
            console.error('Error uploading document:', err)
            setError(err instanceof Error ? err.message : 'Error al subir el documento')
        } finally {
            setIsUploading(false)
        }
    }

    const handleDelete = async (documentId: number) => {
        if (!confirm('¿Estás seguro de eliminar este documento?')) {
            return
        }

        try {
            await apiService.deleteDocument(documentId)
            setSuccess('Documento eliminado exitosamente')
            await loadDocuments()
        } catch (err) {
            console.error('Error deleting document:', err)
            setError(err instanceof Error ? err.message : 'Error al eliminar el documento')
        }
    }

    const formatFileSize = (bytes?: number) => {
        if (!bytes) return '-'
        const kb = bytes / 1024
        if (kb < 1024) return `${kb.toFixed(1)} KB`
        return `${(kb / 1024).toFixed(1)} MB`
    }

    const formatDate = (dateString: string) => {
        return new Date(dateString).toLocaleDateString('es-CO', {
            year: 'numeric',
            month: 'short',
            day: 'numeric',
            hour: '2-digit',
            minute: '2-digit',
        })
    }

    return (
        <div className="min-h-screen bg-gray-50 p-6">
            <div className="max-w-6xl mx-auto space-y-6">
                {/* Header */}
                <div className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="flex items-center gap-3 mb-2">
                        <div className="w-12 h-12 bg-purple-100 rounded-lg flex items-center justify-center">
                            <FileText className="w-6 h-6 text-purple-600" />
                        </div>
                        <div>
                            <h1 className="text-2xl font-bold text-gray-900">
                                Gestión de Documentos
                            </h1>
                            <p className="text-sm text-gray-600">
                                Sube y gestiona documentos para consultas RAG
                            </p>
                        </div>
                    </div>
                </div>

                {/* Success/Error Messages */}
                {success && (
                    <div className="bg-green-50 border border-green-200 rounded-xl p-4">
                        <div className="flex items-start gap-3">
                            <CheckCircle2 className="w-5 h-5 text-green-600 mt-0.5 flex-shrink-0" />
                            <div>
                                <h4 className="text-sm font-semibold text-green-900 mb-1">Éxito</h4>
                                <p className="text-sm text-green-700">{success}</p>
                            </div>
                        </div>
                    </div>
                )}

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

                {/* Upload Form */}
                <form onSubmit={handleUpload} className="bg-white rounded-xl border border-gray-200 p-6">
                    <h3 className="text-lg font-bold text-gray-900 mb-4">Subir Nuevo Documento</h3>

                    <div className="space-y-4">
                        <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                            <div>
                                <label
                                    htmlFor="upload-project-id"
                                    className="block text-sm font-medium text-gray-700 mb-2"
                                >
                                    ID del Proyecto (Opcional)
                                </label>
                                <input
                                    id="upload-project-id"
                                    type="number"
                                    value={uploadProjectId || ''}
                                    onChange={(e) =>
                                        setUploadProjectId(
                                            e.target.value ? parseInt(e.target.value) : undefined
                                        )
                                    }
                                    placeholder="Ej: 1"
                                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                    disabled={isUploading}
                                />
                            </div>

                            <div>
                                <label
                                    htmlFor="document-type"
                                    className="block text-sm font-medium text-gray-700 mb-2"
                                >
                                    Tipo de Documento
                                </label>
                                <select
                                    id="document-type"
                                    value={documentType}
                                    onChange={(e) => setDocumentType(e.target.value)}
                                    className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                    disabled={isUploading}
                                >
                                    <option value="project_document">Documento del Proyecto</option>
                                    <option value="technical_document">Documento Técnico</option>
                                    <option value="budget_document">Documento de Presupuesto</option>
                                    <option value="report">Informe</option>
                                    <option value="other">Otro</option>
                                </select>
                            </div>
                        </div>

                        <div>
                            <label
                                htmlFor="file-upload"
                                className="block text-sm font-medium text-gray-700 mb-2"
                            >
                                Archivo
                            </label>
                            <input
                                id="file-upload"
                                type="file"
                                onChange={handleFileSelect}
                                accept=".pdf,.docx,.txt,.xlsx,.xls"
                                className="w-full px-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                                disabled={isUploading}
                            />
                            <p className="text-xs text-gray-500 mt-1">
                                Formatos soportados: PDF, DOCX, TXT, XLSX
                            </p>
                        </div>

                        {selectedFile && (
                            <div className="bg-gray-50 rounded-lg p-3 flex items-center gap-3">
                                <FileText className="w-5 h-5 text-gray-600" />
                                <div className="flex-1">
                                    <p className="text-sm font-medium text-gray-900">
                                        {selectedFile.name}
                                    </p>
                                    <p className="text-xs text-gray-600">
                                        {formatFileSize(selectedFile.size)}
                                    </p>
                                </div>
                            </div>
                        )}

                        <button
                            type="submit"
                            disabled={isUploading || !selectedFile}
                            className="w-full bg-purple-600 text-white px-6 py-3 rounded-lg font-medium hover:bg-purple-700 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors flex items-center justify-center gap-2"
                        >
                            {isUploading ? (
                                <>
                                    <Loader2 className="w-5 h-5 animate-spin" />
                                    Subiendo...
                                </>
                            ) : (
                                <>
                                    <Upload className="w-5 h-5" />
                                    Subir Documento
                                </>
                            )}
                        </button>
                    </div>
                </form>

                {/* Documents List */}
                <div className="bg-white rounded-xl border border-gray-200 p-6">
                    <div className="flex items-center justify-between mb-4">
                        <h3 className="text-lg font-bold text-gray-900">Documentos Subidos</h3>

                        <div className="flex items-center gap-2">
                            <label htmlFor="filter-project" className="text-sm text-gray-600">
                                Filtrar por proyecto:
                            </label>
                            <input
                                id="filter-project"
                                type="number"
                                value={projectIdFilter || ''}
                                onChange={(e) =>
                                    setProjectIdFilter(
                                        e.target.value ? parseInt(e.target.value) : undefined
                                    )
                                }
                                placeholder="ID"
                                className="w-24 px-3 py-1 text-sm border border-gray-300 rounded-lg focus:ring-2 focus:ring-purple-500 focus:border-transparent"
                            />
                            <button
                                onClick={loadDocuments}
                                disabled={!projectIdFilter || isLoading}
                                className="px-3 py-1 text-sm bg-gray-100 text-gray-700 rounded-lg hover:bg-gray-200 disabled:opacity-50 disabled:cursor-not-allowed"
                            >
                                {isLoading ? <Loader2 className="w-4 h-4 animate-spin" /> : 'Buscar'}
                            </button>
                        </div>
                    </div>

                    {isLoading ? (
                        <div className="py-12 text-center">
                            <Loader2 className="w-8 h-8 text-gray-400 animate-spin mx-auto mb-2" />
                            <p className="text-sm text-gray-600">Cargando documentos...</p>
                        </div>
                    ) : documents.length === 0 ? (
                        <div className="py-12 text-center">
                            <FileText className="w-16 h-16 text-gray-300 mx-auto mb-4" />
                            <h3 className="text-lg font-semibold text-gray-900 mb-2">
                                {projectIdFilter ? 'No hay documentos' : 'Selecciona un proyecto'}
                            </h3>
                            <p className="text-sm text-gray-600 max-w-md mx-auto">
                                {projectIdFilter
                                    ? 'Este proyecto aún no tiene documentos subidos.'
                                    : 'Ingresa un ID de proyecto para ver sus documentos.'}
                            </p>
                        </div>
                    ) : (
                        <div className="overflow-x-auto">
                            <table className="w-full text-sm">
                                <thead>
                                    <tr className="border-b border-gray-200">
                                        <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                            Nombre
                                        </th>
                                        <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                            Tipo
                                        </th>
                                        <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                            Proyecto
                                        </th>
                                        <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                            Tamaño
                                        </th>
                                        <th className="text-left py-3 px-4 font-semibold text-gray-700">
                                            Fecha
                                        </th>
                                        <th className="text-right py-3 px-4 font-semibold text-gray-700">
                                            Acciones
                                        </th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {documents.map((doc) => (
                                        <tr
                                            key={doc.id}
                                            className="border-b border-gray-100 hover:bg-gray-50"
                                        >
                                            <td className="py-3 px-4">
                                                <div className="flex items-center gap-2">
                                                    <FileText className="w-4 h-4 text-gray-400" />
                                                    <span className="text-gray-900 font-medium">
                                                        {doc.filename}
                                                    </span>
                                                </div>
                                            </td>
                                            <td className="py-3 px-4 text-gray-600">
                                                {doc.documentType}
                                            </td>
                                            <td className="py-3 px-4 text-gray-600">
                                                {doc.projectId || '-'}
                                            </td>
                                            <td className="py-3 px-4 text-gray-600">
                                                {formatFileSize(doc.fileSize)}
                                            </td>
                                            <td className="py-3 px-4 text-gray-600">
                                                {formatDate(doc.uploadDate)}
                                            </td>
                                            <td className="py-3 px-4">
                                                <div className="flex items-center justify-end gap-2">
                                                    <button
                                                        onClick={() => handleDelete(doc.id)}
                                                        className="p-2 text-red-600 hover:bg-red-50 rounded-lg transition-colors"
                                                        title="Eliminar"
                                                    >
                                                        <Trash2 className="w-4 h-4" />
                                                    </button>
                                                </div>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>
                    )}
                </div>
            </div>
        </div>
    )
}
