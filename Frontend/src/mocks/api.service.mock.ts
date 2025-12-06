import type { Project, Activity, Notification, DashboardStats } from '../types'
import { mockProjects, getProjectById, getProjectsByUserId } from '../mocks/projects.mock'
import {
    mockActivities,
    getActivitiesByProjectId,
    getActivitiesByObjectiveId,
    getActivityById,
    getActivitiesByStatus,
} from '../mocks/activities.mock'
import {
    getNotificationsByUserId,
    getUnreadNotifications,
    markNotificationAsRead,
    markAllAsRead,
} from '../mocks/notifications.mock'
import { getDashboardStats } from '../mocks/dashboard.mock'

class ApiServiceMock {
    // === Proyectos ===

    async getProjects(): Promise<Project[]> {
        await this.simulateNetworkDelay()
        return mockProjects
    }

    async getProjectById(id: number): Promise<Project> {
        await this.simulateNetworkDelay()

        const project = getProjectById(id)

        if (!project) {
            throw new Error(`Proyecto con ID ${id} no encontrado`)
        }

        return project
    }

    async getProjectsByUserId(userId: number): Promise<Project[]> {
        await this.simulateNetworkDelay()
        return getProjectsByUserId(userId)
    }

    async createProject(projectData: Omit<Project, 'id'>): Promise<Project> {
        await this.simulateNetworkDelay()

        const newProject: Project = {
            ...projectData,
            id: mockProjects.length + 1,
        }

        mockProjects.push(newProject)

        return newProject
    }

    async updateProject(id: number, updates: Partial<Project>): Promise<Project> {
        await this.simulateNetworkDelay()

        const projectIndex = mockProjects.findIndex((p) => p.id === id)

        if (projectIndex === -1) {
            throw new Error(`Proyecto con ID ${id} no encontrado`)
        }

        mockProjects[projectIndex] = {
            ...mockProjects[projectIndex],
            ...updates,
        }

        return mockProjects[projectIndex]
    }

    async deleteProject(id: number): Promise<void> {
        await this.simulateNetworkDelay()

        const projectIndex = mockProjects.findIndex((p) => p.id === id)

        if (projectIndex === -1) {
            throw new Error(`Proyecto con ID ${id} no encontrado`)
        }

        mockProjects.splice(projectIndex, 1)
    }

    // === Actividades ===

    async getActivities(): Promise<Activity[]> {
        await this.simulateNetworkDelay()
        return mockActivities
    }

    async getActivitiesByProjectId(projectId: number): Promise<Activity[]> {
        await this.simulateNetworkDelay()
        return getActivitiesByProjectId(projectId)
    }

    async getActivitiesByObjectiveId(objectiveId: number): Promise<Activity[]> {
        await this.simulateNetworkDelay()
        return getActivitiesByObjectiveId(objectiveId)
    }

    async getActivityById(id: number): Promise<Activity> {
        await this.simulateNetworkDelay()

        const activity = getActivityById(id)

        if (!activity) {
            throw new Error(`Actividad con ID ${id} no encontrada`)
        }

        return activity
    }

    async getActivitiesByStatus(status: Activity['estado']): Promise<Activity[]> {
        await this.simulateNetworkDelay()
        return getActivitiesByStatus(status)
    }

    async createActivity(activityData: Omit<Activity, 'id'>): Promise<Activity> {
        await this.simulateNetworkDelay()

        const newActivity: Activity = {
            ...activityData,
            id: mockActivities.length + 1,
        }

        mockActivities.push(newActivity)

        return newActivity
    }

    async updateActivity(id: number, updates: Partial<Activity>): Promise<Activity> {
        await this.simulateNetworkDelay()

        const activityIndex = mockActivities.findIndex((a) => a.id === id)

        if (activityIndex === -1) {
            throw new Error(`Actividad con ID ${id} no encontrada`)
        }

        mockActivities[activityIndex] = {
            ...mockActivities[activityIndex],
            ...updates,
        }

        return mockActivities[activityIndex]
    }

    async deleteActivity(id: number): Promise<void> {
        await this.simulateNetworkDelay()

        const activityIndex = mockActivities.findIndex((a) => a.id === id)

        if (activityIndex === -1) {
            throw new Error(`Actividad con ID ${id} no encontrada`)
        }

        mockActivities.splice(activityIndex, 1)
    }

    // === Notificaciones ===

    async getNotifications(userId: number): Promise<Notification[]> {
        await this.simulateNetworkDelay()
        return getNotificationsByUserId(userId)
    }

    async getUnreadNotifications(userId: number): Promise<Notification[]> {
        await this.simulateNetworkDelay()
        return getUnreadNotifications(userId)
    }

    async markNotificationAsRead(notificationId: number): Promise<void> {
        await this.simulateNetworkDelay()
        markNotificationAsRead(notificationId)
    }

    async markAllNotificationsAsRead(userId: number): Promise<void> {
        await this.simulateNetworkDelay()
        markAllAsRead(userId)
    }

    // === Dashboard ===

    async getDashboardStats(userId: number): Promise<DashboardStats> {
        await this.simulateNetworkDelay()
        return getDashboardStats(userId)
    }

    // === Utilidades ===

    private async simulateNetworkDelay(minMs: number = 200, maxMs: number = 600): Promise<void> {
        const delay = Math.random() * (maxMs - minMs) + minMs
        return new Promise((resolve) => setTimeout(resolve, delay))
    }
}

export const apiServiceMock = new ApiServiceMock()
