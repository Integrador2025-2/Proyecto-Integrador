import type { Activity, Task } from '../types'

export const mockTasks: Task[] = [
    // Tareas para Actividad 1
    {
        id: 1,
        activityId: 1,
        nombre: 'Revisión bibliográfica',
        descripcion: 'Realizar revisión de literatura sobre gestión de proyectos',
        completada: true,
        fechaVencimiento: '2024-02-15',
        responsable: 'María González',
    },
    {
        id: 2,
        activityId: 1,
        nombre: 'Entrevistas con stakeholders',
        descripcion: 'Conducir entrevistas con investigadores y administradores',
        completada: true,
        fechaVencimiento: '2024-02-28',
        responsable: 'Juan Pérez',
    },
    {
        id: 3,
        activityId: 1,
        nombre: 'Documentar hallazgos',
        descripcion: 'Consolidar y documentar los resultados del análisis',
        completada: false,
        fechaVencimiento: '2024-03-10',
        responsable: 'Carlos López',
    },
    // Tareas para Actividad 2
    {
        id: 4,
        activityId: 2,
        nombre: 'Diseño de base de datos',
        descripcion: 'Crear el modelo entidad-relación y esquema de BD',
        completada: true,
        fechaVencimiento: '2024-04-15',
        responsable: 'Juan Pérez',
    },
    {
        id: 5,
        activityId: 2,
        nombre: 'Selección de tecnologías',
        descripcion: 'Evaluar y seleccionar el stack tecnológico',
        completada: true,
        fechaVencimiento: '2024-04-20',
        responsable: 'María González',
    },
    {
        id: 6,
        activityId: 2,
        nombre: 'Documentación técnica',
        descripcion: 'Elaborar documentación de la arquitectura',
        completada: false,
        fechaVencimiento: '2024-05-05',
        responsable: 'Carlos López',
    },
    // Tareas para Actividad 3
    {
        id: 7,
        activityId: 3,
        nombre: 'Desarrollo del backend',
        descripcion: 'Implementar API REST y lógica de negocio',
        completada: true,
        fechaVencimiento: '2024-07-15',
        responsable: 'Juan Pérez',
    },
    {
        id: 8,
        activityId: 3,
        nombre: 'Desarrollo del frontend',
        descripcion: 'Implementar interfaz de usuario con React',
        completada: false,
        fechaVencimiento: '2024-08-30',
        responsable: 'María González',
    },
    {
        id: 9,
        activityId: 3,
        nombre: 'Pruebas de integración',
        descripcion: 'Ejecutar suite de pruebas end-to-end',
        completada: false,
        fechaVencimiento: '2024-09-15',
        responsable: 'Carlos López',
    },
    // Tareas para Actividad 4
    {
        id: 10,
        activityId: 4,
        nombre: 'Pruebas unitarias',
        descripcion: 'Escribir y ejecutar pruebas unitarias',
        completada: false,
        fechaVencimiento: '2024-10-10',
        responsable: 'María González',
    },
    {
        id: 11,
        activityId: 4,
        nombre: 'Pruebas de usuario',
        descripcion: 'Realizar sesiones de UAT con usuarios finales',
        completada: false,
        fechaVencimiento: '2024-10-25',
        responsable: 'Juan Pérez',
    },
    {
        id: 12,
        activityId: 4,
        nombre: 'Corrección de bugs',
        descripcion: 'Resolver issues identificados en testing',
        completada: false,
        fechaVencimiento: '2024-11-05',
        responsable: 'Carlos López',
    },
]

export const mockActivities: Activity[] = [
    {
        id: 1,
        projectId: 1,
        objectiveId: 2,
        nombre: 'Análisis de Requisitos del Sistema',
        descripcion:
            'Realizar un análisis exhaustivo de los requisitos funcionales y no funcionales del sistema de gestión de proyectos',
        fechaInicio: '2024-02-01',
        fechaFin: '2024-03-15',
        estado: 'Completada',
        responsable: 'Juan Pérez',
        responsableId: 1,
        progreso: 100,
        tareas: mockTasks.filter((t) => t.activityId === 1),
        presupuesto: 15000000,
    },
    {
        id: 2,
        projectId: 1,
        objectiveId: 3,
        nombre: 'Diseño de Arquitectura del Sistema',
        descripcion:
            'Definir la arquitectura técnica, patrones de diseño y estructura de la base de datos',
        fechaInicio: '2024-03-16',
        fechaFin: '2024-05-10',
        estado: 'Completada',
        responsable: 'María González',
        responsableId: 2,
        progreso: 100,
        tareas: mockTasks.filter((t) => t.activityId === 2),
        presupuesto: 20000000,
    },
    {
        id: 3,
        projectId: 1,
        objectiveId: 4,
        nombre: 'Desarrollo e Implementación',
        descripcion: 'Implementar los módulos principales del sistema según las especificaciones',
        fechaInicio: '2024-05-11',
        fechaFin: '2024-09-20',
        estado: 'En curso',
        responsable: 'Juan Pérez',
        responsableId: 1,
        progreso: 65,
        tareas: mockTasks.filter((t) => t.activityId === 3),
        presupuesto: 45000000,
    },
    {
        id: 4,
        projectId: 1,
        objectiveId: 4,
        nombre: 'Pruebas y Control de Calidad',
        descripcion: 'Ejecutar plan de pruebas completo y validar funcionalidades',
        fechaInicio: '2024-09-21',
        fechaFin: '2024-11-15',
        estado: 'En curso',
        responsable: 'Carlos López',
        responsableId: 3,
        progreso: 30,
        tareas: mockTasks.filter((t) => t.activityId === 4),
        presupuesto: 12000000,
    },
    {
        id: 5,
        projectId: 1,
        objectiveId: 4,
        nombre: 'Capacitación de Usuarios',
        descripcion: 'Preparar material de capacitación y realizar sesiones con usuarios finales',
        fechaInicio: '2024-11-16',
        fechaFin: '2024-12-15',
        estado: 'Pendiente',
        responsable: 'María González',
        responsableId: 2,
        progreso: 0,
        tareas: [],
        presupuesto: 8000000,
    },
    {
        id: 6,
        projectId: 1,
        objectiveId: 4,
        nombre: 'Despliegue y Puesta en Producción',
        descripcion: 'Realizar despliegue final en ambiente de producción',
        fechaInicio: '2024-12-16',
        fechaFin: '2024-12-31',
        estado: 'Pendiente',
        responsable: 'Juan Pérez',
        responsableId: 1,
        progreso: 0,
        tareas: [],
        presupuesto: 10000000,
    },
]

export const getActivitiesByProjectId = (projectId: number): Activity[] => {
    return mockActivities.filter((a) => a.projectId === projectId)
}

export const getActivitiesByObjectiveId = (objectiveId: number): Activity[] => {
    return mockActivities.filter((a) => a.objectiveId === objectiveId)
}

export const getActivityById = (id: number): Activity | undefined => {
    return mockActivities.find((a) => a.id === id)
}

export const getActivitiesByStatus = (status: Activity['estado']): Activity[] => {
    return mockActivities.filter((a) => a.estado === status)
}

export const getUpcomingActivities = (daysAhead: number = 7): Activity[] => {
    const today = new Date()
    const futureDate = new Date()
    futureDate.setDate(today.getDate() + daysAhead)

    return mockActivities.filter((a) => {
        const endDate = new Date(a.fechaFin)
        return endDate >= today && endDate <= futureDate && a.estado !== 'Completada'
    })
}
