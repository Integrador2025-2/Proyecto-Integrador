import { Link, useLocation } from 'react-router-dom'
import {
    LayoutDashboard,
    FolderKanban,
    CheckSquare,
    Users,
    FileText,
    Settings,
    LogOut,
} from 'lucide-react'
import { useAuth } from '../../hooks/useAuth'
import { useAuthStore } from '../../store/authStore'

export default function Sidebar() {
    const location = useLocation()
    const { logout } = useAuth()
    const user = useAuthStore((state) => state.user)

    const menuItems = [
        { icon: LayoutDashboard, label: 'Dashboard', path: '/dashboard' },
        { icon: FolderKanban, label: 'Proyectos', path: '/projects' },
        { icon: CheckSquare, label: 'Actividades', path: '/activities' },
        { icon: Users, label: 'Equipo', path: '/team' },
        { icon: FileText, label: 'Documentos', path: '/documents' },
        { icon: Settings, label: 'Configuración', path: '/settings' },
    ]

    const isActive = (path: string) => location.pathname === path

    return (
        <aside className="w-64 bg-white border-r border-gray-200 flex flex-col h-screen fixed left-0 top-0">
            {/* Logo */}
            <div className="p-6 border-b border-gray-200">
                <h1 className="text-2xl font-bold text-blue-600">SIGPI</h1>
                <p className="text-xs text-gray-500 mt-1">Sistema de Gestión de Proyectos</p>
            </div>

            {/* User Info */}
            <div className="p-4 border-b border-gray-200">
                <div className="flex items-center gap-3">
                    <img
                        src={user?.profilePictureUrl || 'https://i.pravatar.cc/150?img=1'}
                        alt={user?.firstName}
                        className="w-10 h-10 rounded-full"
                    />
                    <div className="flex-1 min-w-0">
                        <p className="text-sm font-semibold text-gray-900 truncate">
                            {user?.firstName} {user?.lastName}
                        </p>
                        <p className="text-xs text-gray-500 truncate">{user?.roleName}</p>
                    </div>
                </div>
            </div>

            {/* Navigation */}
            <nav className="flex-1 p-4 overflow-y-auto">
                <ul className="space-y-1">
                    {menuItems.map((item) => {
                        const Icon = item.icon
                        return (
                            <li key={item.path}>
                                <Link
                                    to={item.path}
                                    className={`flex items-center gap-3 px-3 py-2.5 rounded-lg transition-colors ${
                                        isActive(item.path)
                                            ? 'bg-blue-50 text-blue-600 font-medium'
                                            : 'text-gray-700 hover:bg-gray-50'
                                    }`}
                                >
                                    <Icon className="w-5 h-5" />
                                    <span className="text-sm">{item.label}</span>
                                </Link>
                            </li>
                        )
                    })}
                </ul>
            </nav>

            {/* Logout Button */}
            <div className="p-4 border-t border-gray-200">
                <button
                    onClick={logout}
                    className="flex items-center gap-3 px-3 py-2.5 rounded-lg text-gray-700 hover:bg-red-50 hover:text-red-600 transition-colors w-full"
                >
                    <LogOut className="w-5 h-5" />
                    <span className="text-sm font-medium">Cerrar Sesión</span>
                </button>
            </div>
        </aside>
    )
}
