import { useState, useEffect } from 'react'
import { Bell, Search } from 'lucide-react'
import { apiServiceMock } from '../../mocks/api.service.mock'
import { useAuthStore } from '../../store/authStore'
import type { Notification } from '../../types'

export default function Header() {
    const user = useAuthStore((state) => state.user)

    const [notifications, setNotifications] = useState<Notification[]>([])
    const [showNotifications, setShowNotifications] = useState(false)
    const [unreadCount, setUnreadCount] = useState(0)

    useEffect(() => {
        loadNotifications()
    }, [user])

    const loadNotifications = async () => {
        if (!user) return

        try {
            const data = await apiServiceMock.getUnreadNotifications(user.id)
            setNotifications(data)
            setUnreadCount(data.length)
        } catch (error) {
            console.error('Error loading notifications:', error)
        }
    }

    const handleMarkAsRead = async (notificationId: number) => {
        try {
            await apiServiceMock.markNotificationAsRead(notificationId)
            loadNotifications()
        } catch (error) {
            console.error('Error marking notification as read:', error)
        }
    }

    const getNotificationIcon = (tipo: Notification['tipo']) => {
        switch (tipo) {
            case 'success':
                return '✅'
            case 'warning':
                return '⚠️'
            case 'error':
                return '❌'
            default:
                return 'ℹ️'
        }
    }

    return (
        <header className="bg-white border-b border-gray-200 px-6 py-4 fixed top-0 right-0 left-64 z-10 h-20">
            <div className="flex items-center justify-between">
                {/* Search Bar */}
                <div className="flex-1 max-w-2xl">
                    <div className="relative">
                        <Search className="absolute left-3 top-1/2 transform -translate-y-1/2 text-gray-400 w-5 h-5" />
                        <input
                            type="text"
                            placeholder="Buscar proyectos, actividades, documentos..."
                            className="w-full pl-10 pr-4 py-2 border border-gray-300 rounded-lg focus:ring-2 focus:ring-blue-500 focus:border-transparent"
                        />
                    </div>
                </div>

                {/* Notifications */}
                <div className="relative ml-6">
                    <button
                        onClick={() => setShowNotifications(!showNotifications)}
                        className="relative p-2 text-gray-600 hover:bg-gray-100 rounded-lg transition"
                    >
                        <Bell className="w-6 h-6" />
                        {unreadCount > 0 && (
                            <span className="absolute top-0 right-0 w-5 h-5 bg-red-500 text-white text-xs rounded-full flex items-center justify-center">
                                {unreadCount}
                            </span>
                        )}
                    </button>

                    {/* Notifications Dropdown */}
                    {showNotifications && (
                        <div className="absolute right-0 mt-2 w-80 bg-white rounded-lg shadow-xl border border-gray-200 max-h-96 overflow-y-auto">
                            <div className="p-4 border-b border-gray-200">
                                <h3 className="font-semibold text-gray-900">Notificaciones</h3>
                                <p className="text-xs text-gray-500 mt-1">{unreadCount} sin leer</p>
                            </div>

                            {notifications.length === 0 ? (
                                <div className="p-8 text-center text-gray-500">
                                    <Bell className="w-12 h-12 mx-auto mb-3 text-gray-300" />
                                    <p className="text-sm">No tienes notificaciones</p>
                                </div>
                            ) : (
                                <ul className="divide-y divide-gray-100">
                                    {notifications.map((notification) => (
                                        <li
                                            key={notification.id}
                                            className="p-4 hover:bg-gray-50 cursor-pointer transition"
                                            onClick={() => handleMarkAsRead(notification.id)}
                                        >
                                            <div className="flex gap-3">
                                                <span className="text-xl flex-shrink-0">
                                                    {getNotificationIcon(notification.tipo)}
                                                </span>
                                                <div className="flex-1 min-w-0">
                                                    <p className="text-sm font-medium text-gray-900">
                                                        {notification.titulo}
                                                    </p>
                                                    <p className="text-xs text-gray-600 mt-1">
                                                        {notification.mensaje}
                                                    </p>
                                                    <p className="text-xs text-gray-400 mt-1">
                                                        {new Date(
                                                            notification.fecha,
                                                        ).toLocaleDateString('es-ES', {
                                                            day: '2-digit',
                                                            month: 'short',
                                                            hour: '2-digit',
                                                            minute: '2-digit',
                                                        })}
                                                    </p>
                                                </div>
                                            </div>
                                        </li>
                                    ))}
                                </ul>
                            )}

                            {notifications.length > 0 && (
                                <div className="p-3 border-t border-gray-200">
                                    <button
                                        onClick={async () => {
                                            if (user) {
                                                await apiServiceMock.markAllNotificationsAsRead(
                                                    user.id,
                                                )
                                                loadNotifications()
                                            }
                                        }}
                                        className="text-sm text-blue-600 hover:text-blue-700 font-medium w-full text-center"
                                    >
                                        Marcar todas como leídas
                                    </button>
                                </div>
                            )}
                        </div>
                    )}
                </div>
            </div>
        </header>
    )
}
