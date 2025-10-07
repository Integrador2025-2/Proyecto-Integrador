// src/components/pages/Home/Layout.tsx

import { useState } from 'react'
import {
    LayoutDashboard,
    FolderKanban,
    Target,
    Zap,
    CheckSquare,
    AlertTriangle,
    FileText,
    Menu,
    LogOut,
    User,
} from 'lucide-react'
import { useUserStore } from '@/globalStates/useUserStore'

interface LayoutProps {
    children: React.ReactNode
}

interface NavItemProps {
    icon: React.ReactNode
    label: string
    active?: boolean
    onClick?: () => void
}

const NavItem = ({ icon, label, active = false, onClick }: NavItemProps) => {
    return (
        <button
            onClick={onClick}
            className={`
        w-full flex items-center gap-3 px-4 py-3 rounded-lg
        transition-colors duration-200
        ${
            active
                ? 'bg-cyan-600/20 text-cyan-400'
                : 'text-gray-300 hover:bg-gray-800/50 hover:text-white'
        }
      `}
        >
            <span className="text-xl">{icon}</span>
            <span className="font-medium">{label}</span>
        </button>
    )
}

export const Layout = ({ children }: LayoutProps) => {
    const [activeSection, setActiveSection] = useState('dashboard')
    const [sidebarOpen, setSidebarOpen] = useState(true)
    const { user, logout } = useUserStore()

    const navItems = [
        { id: 'dashboard', icon: <LayoutDashboard />, label: 'Dashboard' },
        { id: 'proyectos', icon: <FolderKanban />, label: 'Proyectos' },
        { id: 'objetivos', icon: <Target />, label: 'Objetivos' },
        { id: 'actividades', icon: <Zap />, label: 'Actividades' },
        { id: 'tareas', icon: <CheckSquare />, label: 'Tareas' },
        { id: 'problemas', icon: <AlertTriangle />, label: 'Análisis Problemas' },
        { id: 'reportes', icon: <FileText />, label: 'Reportes' },
    ]

    return (
        <div className="flex h-screen bg-gray-950">
            {/* Sidebar */}
            <aside
                className={`
          ${sidebarOpen ? 'w-64' : 'w-20'} 
          bg-gray-900 border-r border-gray-800
          transition-all duration-300 ease-in-out
          flex flex-col
        `}
            >
                {/* Header del Sidebar */}
                <div className="p-6 border-b border-gray-800">
                    <div className="flex items-center gap-3">
                        <div className="w-8 h-8 bg-gradient-to-br from-cyan-500 to-blue-600 rounded-lg flex items-center justify-center">
                            <span className="text-white font-bold">📊</span>
                        </div>
                        {sidebarOpen && (
                            <div>
                                <h1 className="text-lg font-bold text-cyan-400">Gestión</h1>
                                <p className="text-xs text-gray-400">Proyectos</p>
                            </div>
                        )}
                    </div>
                </div>

                {/* Navegación */}
                <nav className="flex-1 p-4 space-y-2 overflow-y-auto">
                    {navItems.map((item) => (
                        <NavItem
                            key={item.id}
                            icon={item.icon}
                            label={sidebarOpen ? item.label : ''}
                            active={activeSection === item.id}
                            onClick={() => setActiveSection(item.id)}
                        />
                    ))}
                </nav>

                {/* Usuario y Logout */}
                <div className="p-4 border-t border-gray-800">
                    {sidebarOpen ? (
                        <div className="space-y-2">
                            <div className="flex items-center gap-3 px-4 py-3 bg-gray-800/50 rounded-lg">
                                <div className="w-8 h-8 bg-cyan-600 rounded-full flex items-center justify-center">
                                    <User className="w-4 h-4 text-white" />
                                </div>
                                <div className="flex-1 min-w-0">
                                    <p className="text-sm font-medium text-white truncate">
                                        {user?.name || 'Usuario'}
                                    </p>
                                    <p className="text-xs text-gray-400 truncate">
                                        {user?.email || 'email@example.com'}
                                    </p>
                                </div>
                            </div>
                            <button
                                onClick={logout}
                                className="w-full flex items-center gap-3 px-4 py-2 text-red-400 hover:bg-red-500/10 rounded-lg transition-colors"
                            >
                                <LogOut className="w-5 h-5" />
                                <span>Cerrar sesión</span>
                            </button>
                        </div>
                    ) : (
                        <button
                            onClick={logout}
                            className="w-full p-3 text-red-400 hover:bg-red-500/10 rounded-lg transition-colors"
                        >
                            <LogOut className="w-5 h-5 mx-auto" />
                        </button>
                    )}
                </div>
            </aside>

            {/* Contenido Principal */}
            <div className="flex-1 flex flex-col overflow-hidden">
                {/* Header */}
                <header className="h-16 bg-gray-900/50 backdrop-blur-sm border-b border-gray-800 flex items-center justify-between px-6">
                    <div className="flex items-center gap-4">
                        <button
                            onClick={() => setSidebarOpen(!sidebarOpen)}
                            className="p-2 hover:bg-gray-800 rounded-lg transition-colors"
                        >
                            <Menu className="w-5 h-5 text-gray-400" />
                        </button>
                        <h2 className="text-2xl font-bold text-white capitalize">
                            {activeSection}
                        </h2>
                    </div>

                    <div className="flex items-center gap-3">
                        {/* Aquí puedes agregar botones de acción según la sección */}
                    </div>
                </header>

                {/* Área de Contenido */}
                <main className="flex-1 overflow-y-auto bg-gray-950 p-6">{children}</main>
            </div>
        </div>
    )
}
