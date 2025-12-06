import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft } from 'lucide-react'
import { useProjects } from '../../hooks/useProjects'
import ProjectHeader from '../components/ProjectHeader'
import ProjectTabs from './components/ProjectTabs'
import OverviewTab from './tabs/OverviewTab'
import ObjectivesTab from './tabs/ObjectivesTab'
import TeamTab from './tabs/TeamTab'
import type { Project } from '../../types'

type TabType = 'overview' | 'objectives' | 'team' | 'budget' | 'documents' | 'activities'

export default function ProjectDetailPage() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()
    const { getProjectById } = useProjects()

    const [project, setProject] = useState<Project | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [activeTab, setActiveTab] = useState<TabType>('overview')

    useEffect(() => {
        loadProject()
    }, [id])

    const loadProject = async () => {
        if (!id) return

        try {
            setIsLoading(true)
            const data = await getProjectById(parseInt(id))
            setProject(data)
        } catch (error) {
            console.error('Error loading project:', error)
        } finally {
            setIsLoading(false)
        }
    }

    if (isLoading) {
        return (
            <div className="flex items-center justify-center h-96">
                <div className="text-center">
                    <div className="animate-spin rounded-full h-12 w-12 border-b-2 border-blue-600 mx-auto"></div>
                    <p className="mt-4 text-gray-600">Cargando proyecto...</p>
                </div>
            </div>
        )
    }

    if (!project) {
        return (
            <div className="flex items-center justify-center h-96">
                <div className="text-center">
                    <div className="w-16 h-16 bg-red-100 rounded-full flex items-center justify-center mx-auto mb-4">
                        <span className="text-2xl">❌</span>
                    </div>
                    <h3 className="text-lg font-semibold text-gray-900 mb-2">
                        Proyecto no encontrado
                    </h3>
                    <p className="text-gray-600 mb-6">
                        El proyecto que buscas no existe o no tienes acceso a él.
                    </p>
                    <button
                        onClick={() => navigate('/projects')}
                        className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
                    >
                        <ArrowLeft className="w-4 h-4" />
                        Volver a Proyectos
                    </button>
                </div>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            {/* Back Button */}
            <button
                onClick={() => navigate('/projects')}
                className="inline-flex items-center gap-2 text-gray-600 hover:text-gray-900 transition"
            >
                <ArrowLeft className="w-4 h-4" />
                <span className="text-sm font-medium">Volver a Proyectos</span>
            </button>

            {/* Project Header */}
            <ProjectHeader project={project} />

            {/* Tabs Navigation */}
            <ProjectTabs activeTab={activeTab} onTabChange={setActiveTab} />

            {/* Tab Content */}
            <div className="bg-white rounded-xl shadow-sm border border-gray-200">
                {activeTab === 'overview' && <OverviewTab project={project} />}
                {activeTab === 'objectives' && <ObjectivesTab project={project} />}
                {activeTab === 'team' && <TeamTab project={project} />}
                {activeTab === 'budget' && (
                    <div className="p-12 text-center text-gray-500">
                        <p>Tab de Presupuesto - Próximamente</p>
                    </div>
                )}
                {activeTab === 'documents' && (
                    <div className="p-12 text-center text-gray-500">
                        <p>Tab de Documentos - Próximamente</p>
                    </div>
                )}
                {activeTab === 'activities' && (
                    <div className="p-12 text-center text-gray-500">
                        <p>Tab de Actividades - Próximamente</p>
                    </div>
                )}
            </div>
        </div>
    )
}
