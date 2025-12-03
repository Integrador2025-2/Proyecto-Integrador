// src/components/pages/Home/Home.tsx

import { Layout } from '@/components/layout'

export const Home = () => {
    return (
        <Layout>
            <div className="space-y-6">
                {/* Grid de estadísticas */}
                <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
                    {/* Card: Proyectos Activos */}
                    <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-xl p-6">
                        <p className="text-gray-400 text-sm mb-2">Proyectos Activos</p>
                        <h3 className="text-4xl font-bold text-white mb-2">12</h3>
                        <p className="text-green-400 text-sm">↑ 3 este mes</p>
                    </div>

                    {/* Card: Objetivos Cumplidos */}
                    <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-xl p-6">
                        <p className="text-gray-400 text-sm mb-2">Objetivos Cumplidos</p>
                        <h3 className="text-4xl font-bold text-white mb-2">85%</h3>
                        <p className="text-green-400 text-sm">↑ 5% vs mes anterior</p>
                    </div>

                    {/* Card: Tareas Pendientes */}
                    <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-xl p-6">
                        <p className="text-gray-400 text-sm mb-2">Tareas Pendientes</p>
                        <h3 className="text-4xl font-bold text-white mb-2">47</h3>
                        <p className="text-orange-400 text-sm">↑ 12 esta semana</p>
                    </div>

                    {/* Card: Eficiencia General */}
                    <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-xl p-6">
                        <p className="text-gray-400 text-sm mb-2">Eficiencia General</p>
                        <h3 className="text-4xl font-bold text-white mb-2">92%</h3>
                        <p className="text-green-400 text-sm">↑ 2% este trimestre</p>
                    </div>
                </div>

                {/* Sección de Proyectos Recientes */}
                <div className="bg-gray-900/50 backdrop-blur-sm border border-gray-800 rounded-xl p-6">
                    <div className="flex items-center justify-between mb-6">
                        <h2 className="text-xl font-bold text-white">Proyectos Recientes</h2>
                        <div className="flex gap-2">
                            <button className="px-3 py-1 bg-cyan-600/20 text-cyan-400 rounded-lg text-sm hover:bg-cyan-600/30 transition-colors">
                                Tabla
                            </button>
                            <button className="px-3 py-1 text-gray-400 hover:bg-gray-800 rounded-lg text-sm transition-colors">
                                Tarjetas
                            </button>
                            <button className="px-3 py-1 text-gray-400 hover:bg-gray-800 rounded-lg text-sm transition-colors">
                                Kanban
                            </button>
                        </div>
                    </div>

                    {/* Tabla simple de proyectos */}
                    <div className="overflow-x-auto">
                        <table className="w-full">
                            <thead>
                                <tr className="border-b border-gray-800">
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Proyecto
                                    </th>
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Objetivo General
                                    </th>
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Estado
                                    </th>
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Progreso
                                    </th>
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Responsable
                                    </th>
                                    <th className="text-left py-3 px-4 text-sm font-medium text-gray-400">
                                        Fecha Límite
                                    </th>
                                </tr>
                            </thead>
                            <tbody>
                                <tr className="border-b border-gray-800/50 hover:bg-gray-800/30 transition-colors">
                                    <td className="py-4 px-4 text-white font-medium">
                                        Desarrollo App Móvil
                                    </td>
                                    <td className="py-4 px-4 text-gray-300">
                                        Crear aplicación de gestión financiera
                                    </td>
                                    <td className="py-4 px-4">
                                        <span className="px-3 py-1 bg-green-500/20 text-green-400 rounded-full text-sm">
                                            En Progreso
                                        </span>
                                    </td>
                                    <td className="py-4 px-4">
                                        <div className="flex items-center gap-2">
                                            <div className="flex-1 h-2 bg-gray-800 rounded-full overflow-hidden">
                                                <div
                                                    className="h-full bg-cyan-500 rounded-full"
                                                    style={{ width: '65%' }}
                                                ></div>
                                            </div>
                                            <span className="text-sm text-gray-400">65%</span>
                                        </div>
                                    </td>
                                    <td className="py-4 px-4 text-gray-300">María González</td>
                                    <td className="py-4 px-4 text-gray-300">15 Nov 2025</td>
                                </tr>
                                <tr className="border-b border-gray-800/50 hover:bg-gray-800/30 transition-colors">
                                    <td className="py-4 px-4 text-white font-medium">
                                        Sistema ERP
                                    </td>
                                    <td className="py-4 px-4 text-gray-300">
                                        Implementar sistema de gestión empresarial
                                    </td>
                                    <td className="py-4 px-4">
                                        <span className="px-3 py-1 bg-yellow-500/20 text-yellow-400 rounded-full text-sm">
                                            Pendiente
                                        </span>
                                    </td>
                                    <td className="py-4 px-4">
                                        <div className="flex items-center gap-2">
                                            <div className="flex-1 h-2 bg-gray-800 rounded-full overflow-hidden">
                                                <div
                                                    className="h-full bg-cyan-500 rounded-full"
                                                    style={{ width: '25%' }}
                                                ></div>
                                            </div>
                                            <span className="text-sm text-gray-400">25%</span>
                                        </div>
                                    </td>
                                    <td className="py-4 px-4 text-gray-300">Carlos Ruiz</td>
                                    <td className="py-4 px-4 text-gray-300">30 Dic 2025</td>
                                </tr>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </Layout>
    )
}
