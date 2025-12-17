import { Outlet } from 'react-router-dom'
import Sidebar from './Sidebar'
import Header from './Header'

export default function MainLayout() {
    return (
        <div className="min-h-screen bg-gray-50 flex">
            {/* Sidebar - Fixed a la izquierda */}
            <Sidebar />

            {/* Contenedor principal - empuja contenido a la derecha del sidebar */}
            <div className="flex-1 ml-64 flex flex-col">
                {/* Header - Fixed en la parte superior del área de contenido */}
                <Header />

                {/* Main Content - con padding-top para compensar el header fixed */}
                <main className="flex-1 p-6 mt-20">
                    <Outlet />
                </main>
            </div>
        </div>
    )
}
