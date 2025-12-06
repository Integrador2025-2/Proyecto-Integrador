import { LayoutList, Target, Users, DollarSign, FileText, CheckSquare } from 'lucide-react'

type TabType = 'overview' | 'objectives' | 'team' | 'budget' | 'documents' | 'activities'

interface ProjectTabsProps {
    activeTab: TabType
    onTabChange: (tab: TabType) => void
}

export default function ProjectTabs({ activeTab, onTabChange }: ProjectTabsProps) {
    const tabs = [
        { id: 'overview' as TabType, label: 'Información General', icon: LayoutList },
        { id: 'objectives' as TabType, label: 'Objetivos', icon: Target },
        { id: 'team' as TabType, label: 'Equipo', icon: Users },
        { id: 'budget' as TabType, label: 'Presupuesto', icon: DollarSign },
        { id: 'documents' as TabType, label: 'Documentos', icon: FileText },
        { id: 'activities' as TabType, label: 'Actividades', icon: CheckSquare },
    ]

    return (
        <div className="bg-white rounded-xl shadow-sm border border-gray-200">
            <div className="border-b border-gray-200">
                <nav className="flex overflow-x-auto">
                    {tabs.map((tab) => {
                        const Icon = tab.icon
                        const isActive = activeTab === tab.id

                        return (
                            <button
                                key={tab.id}
                                onClick={() => onTabChange(tab.id)}
                                className={`flex items-center gap-2 px-6 py-4 border-b-2 font-medium text-sm whitespace-nowrap transition ${
                                    isActive
                                        ? 'border-blue-600 text-blue-600'
                                        : 'border-transparent text-gray-600 hover:text-gray-900 hover:border-gray-300'
                                }`}
                            >
                                <Icon className="w-5 h-5" />
                                {tab.label}
                            </button>
                        )
                    })}
                </nav>
            </div>
        </div>
    )
}
