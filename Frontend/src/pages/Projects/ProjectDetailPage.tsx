import { useState, useEffect } from 'react'
import { useParams, useNavigate } from 'react-router-dom'
import { ArrowLeft } from 'lucide-react'

import { apiService } from '../../services/api.service'
import ProjectHeader from './components/ProjectHeader'
import ProjectTabs from './components/ProjectTabs'
import OverviewTab from './components/tabs/OverviewTab'
import ActivitiesTab from './components/tabs/ActivitiesTab'
import type { Project } from '../../types'
import { useProjectsStore } from '../../store/projectStore'

type TabType = 'overview' | 'objectives' | 'team' | 'budget' | 'documents' | 'activities'

export default function ProjectDetailPage() {
    const { id } = useParams<{ id: string }>()
    const navigate = useNavigate()

    const [project, setProject] = useState<Project | null>(null)
    const [isLoading, setIsLoading] = useState(true)
    const [activeTab, setActiveTab] = useState<TabType>('overview')

    const enrichedProjects = useProjectsStore((state) => state.projects)

    useEffect(() => {
        if (!id) return

        const loadProject = async () => {
            try {
                setIsLoading(true)
                const data = await apiService.getProjectById(Number(id))

                const enriched = enrichedProjects.find((p) => (p.id === data.id ? p : ''))
                setProject(enriched ?? data)
            } catch (error) {
                console.error('Error loading project:', error)
                setProject(null)
            } finally {
                setIsLoading(false)
            }
        }

        loadProject()
    }, [id, enrichedProjects])

    if (isLoading) {
        return (
            <div className="min-h-[60vh] flex items-center justify-center">
                <p className="text-gray-600 text-lg">Cargando proyecto...</p>
            </div>
        )
    }

    if (!project) {
        return (
            <div className="min-h-[60vh] flex flex-col items-center justify-center gap-3 text-center">
                <p className="text-xl font-semibold text-gray-800">Proyecto no encontrado</p>
                <p className="text-sm text-gray-600">
                    El proyecto que buscas no existe o no tienes acceso a él.
                </p>
                <button
                    type="button"
                    onClick={() => navigate('/projects')}
                    className="inline-flex items-center gap-2 px-4 py-2 bg-blue-600 text-white rounded-lg hover:bg-blue-700 transition"
                >
                    <ArrowLeft className="w-4 h-4" />
                    Volver a Proyectos
                </button>
            </div>
        )
    }

    return (
        <div className="space-y-6">
            <button
                type="button"
                onClick={() => navigate('/projects')}
                className="inline-flex items-center gap-2 text-gray-600 hover:text-gray-900 transition"
            >
                <ArrowLeft className="w-4 h-4" />
                Volver a Proyectos
            </button>

            <ProjectHeader project={project} />

            <ProjectTabs activeTab={activeTab} onTabChange={setActiveTab} />

            <div className="mt-4">
                {activeTab === 'overview' && <OverviewTab project={project} />}
                {activeTab === 'objectives' && <div>Tab de Objetivos - Próximamente</div>}
                {activeTab === 'team' && <div>Tab de Equipo - Próximamente</div>}
                {activeTab === 'budget' && <div>Tab de Presupuesto - Próximamente</div>}
                {activeTab === 'documents' && <div>Tab de Documentos - Próximamente</div>}
                {activeTab === 'activities' && <ActivitiesTab projectId={project.id} />}
            </div>
        </div>
    )
}
