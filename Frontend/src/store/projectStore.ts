// Nuevo archivo completo (nota los tipos explícitos en state)
import { create } from 'zustand'
import type { Project } from '../types'

interface ProjectsState {
    projects: Project[]
    addProject: (project: Project) => void
}

export const useProjectsStore = create<ProjectsState>((set) => ({
    projects: [],
    addProject: (project) =>
        set((state: ProjectsState) => ({
            projects: [...state.projects, project],
        })),
}))
