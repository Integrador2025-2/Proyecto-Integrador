import type { Project, ProjectParticipant, Objective, Document } from '../types'

export const mockParticipants: ProjectParticipant[] = [
    {
        id: 1,
        userId: 1,
        nombre: 'Juan Pérez',
        rol: 'Investigador Principal',
        email: 'juan.perez@email.com',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=12',
    },
    {
        id: 2,
        userId: 2,
        nombre: 'María González',
        rol: 'Co-investigadora',
        email: 'maria.gonzalez@email.com',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=45',
    },
    {
        id: 3,
        userId: 3,
        nombre: 'Carlos López',
        rol: 'Asistente de Investigación',
        email: 'carlos.lopez@email.com',
        profilePictureUrl: 'https://i.pravatar.cc/150?img=33',
    },
]

export const mockObjectives: Objective[] = [
    {
        id: 1,
        projectId: 1,
        nombre: 'Objetivo General',
        descripcion:
            'Desarrollar una metodología innovadora para la gestión de proyectos académicos',
        tipo: 'General',
        estado: 'En curso',
        progreso: 65,
    },
    {
        id: 2,
        projectId: 1,
        nombre: 'Análisis de Requisitos',
        descripcion:
            'Identificar y documentar todos los requisitos funcionales y no funcionales del sistema',
        tipo: 'Específico',
        estado: 'Completado',
        progreso: 100,
    },
    {
        id: 3,
        projectId: 1,
        nombre: 'Diseño de Arquitectura',
        descripcion: 'Diseñar la arquitectura técnica del sistema de gestión',
        tipo: 'Específico',
        estado: 'En curso',
        progreso: 75,
    },
    {
        id: 4,
        projectId: 1,
        nombre: 'Implementación del Sistema',
        descripcion: 'Desarrollar e implementar los módulos principales del sistema',
        tipo: 'Específico',
        estado: 'En curso',
        progreso: 45,
    },
]

export const mockDocuments: Document[] = [
    {
        id: 1,
        projectId: 1,
        nombre: 'Propuesta de Investigación.pdf',
        tipo: 'PDF',
        url: '#',
        fechaSubida: '2024-01-20T10:30:00Z',
        uploadedBy: 'Juan Pérez',
        tamaño: '2.5 MB',
    },
    {
        id: 2,
        projectId: 1,
        nombre: 'Cronograma de Actividades.xlsx',
        tipo: 'Excel',
        url: '#',
        fechaSubida: '2024-02-15T14:20:00Z',
        uploadedBy: 'María González',
        tamaño: '1.8 MB',
    },
    {
        id: 3,
        projectId: 1,
        nombre: 'Presupuesto Detallado.pdf',
        tipo: 'PDF',
        url: '#',
        fechaSubida: '2024-03-10T09:45:00Z',
        uploadedBy: 'Juan Pérez',
        tamaño: '3.2 MB',
    },
]

export const mockProjects: Project[] = [
    {
        id: 1,
        codigo: 'PROJ-2024-001',
        nombre: 'Sistema de Gestión de Proyectos de Investigación',
        descripcion:
            'Desarrollo de una plataforma web integral para la gestión de proyectos de investigación universitarios con financiamiento SGR',
        fechaInicio: '2024-01-15',
        fechaFin: '2024-12-31',
        estado: 'En ejecución',
        presupuestoTotal: 150000000,
        presupuestoEjecutado: 85000000,
        investigadorPrincipal: 'Juan Pérez',
        participantes: mockParticipants,
        objetivos: mockObjectives.filter((o) => o.projectId === 1),
        documentos: mockDocuments.filter((d) => d.projectId === 1),
        progreso: 65,
    },
    {
        id: 2,
        codigo: 'PROJ-2024-002',
        nombre: 'Análisis de Impacto Ambiental en Caldas',
        descripcion:
            'Estudio del impacto ambiental de actividades industriales en el departamento de Caldas',
        fechaInicio: '2024-03-01',
        fechaFin: '2025-02-28',
        estado: 'En revisión',
        presupuestoTotal: 95000000,
        presupuestoEjecutado: 25000000,
        investigadorPrincipal: 'María González',
        participantes: [mockParticipants[1]],
        objetivos: [],
        documentos: [],
        progreso: 30,
    },
    {
        id: 3,
        codigo: 'PROJ-2023-015',
        nombre: 'Innovación en Procesos Educativos Digitales',
        descripcion:
            'Implementación de metodologías educativas innovadoras mediante plataformas digitales',
        fechaInicio: '2023-08-01',
        fechaFin: '2024-07-31',
        estado: 'Finalizado',
        presupuestoTotal: 75000000,
        presupuestoEjecutado: 75000000,
        investigadorPrincipal: 'Carlos López',
        participantes: [mockParticipants[2]],
        objetivos: [],
        documentos: [],
        progreso: 100,
    },
]

export const getProjectById = (id: number): Project | undefined => {
    return mockProjects.find((p) => p.id === id)
}

export const getProjectsByUserId = (userId: number): Project[] => {
    return mockProjects.filter((p) =>
        p.participantes.some((participant) => participant.userId === userId),
    )
}
