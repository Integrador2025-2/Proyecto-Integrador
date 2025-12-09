import { create } from 'zustand'
import type { Project, Objective, ProjectParticipant, ProjectDocument } from '../types'

interface ProjectState {
    projects: Project[] // Proyectos enriquecidos
    isInitialized: boolean
    addProject: (project: Project) => void
    updateProject: (projectId: number, updates: Partial<Project>) => void
    removeProject: (projectId: number) => void

    // === OBJETIVOS ===
    addObjective: (projectId: number, objective: Objective) => void
    updateObjective: (projectId: number, objectiveId: number, updates: Partial<Objective>) => void
    removeObjective: (projectId: number, objectiveId: number) => void

    // === PARTICIPANTES ===
    addParticipant: (projectId: number, participant: ProjectParticipant) => void
    removeParticipant: (projectId: number, participantId: number) => void

    // === DOCUMENTOS ===
    addDocument: (projectId: number, document: ProjectDocument) => void
    removeDocument: (projectId: number, documentId: number) => void
}

export const useProjectStore = create<ProjectState>((set) => ({
    projects: [],
    isInitialized: false,

    addProject: (project) =>
        set((state) => ({
            projects: [...state.projects, project],
        })),

    updateProject: (projectId, updates) =>
        set((state) => ({
            projects: state.projects.map((p) => (p.id === projectId ? { ...p, ...updates } : p)),
        })),

    removeProject: (projectId) =>
        set((state) => ({
            projects: state.projects.filter((p) => p.id !== projectId),
        })),

    // === OBJETIVOS ===
    addObjective: (projectId, objective) =>
        set((state) => {
            const newId = objective.id ?? Date.now()
            const newObjective: Objective = { ...objective, id: newId }

            return {
                projects: state.projects.map((p) =>
                    p.id === projectId
                        ? { ...p, objetivos: [...(p.objetivos ?? []), newObjective] }
                        : p,
                ),
            }
        }),

    updateObjective: (projectId, objectiveId, updates) =>
        set((state) => ({
            projects: state.projects.map((p) =>
                p.id === projectId
                    ? {
                          ...p,
                          objetivos: (p.objetivos ?? []).map((o) =>
                              o.id === objectiveId ? { ...o, ...updates } : o,
                          ),
                      }
                    : p,
            ),
        })),

    removeObjective: (projectId, objectiveId) =>
        set((state) => ({
            projects: state.projects.map((p) =>
                p.id === projectId
                    ? {
                          ...p,
                          objetivos: (p.objetivos ?? []).filter((o) => o.id !== objectiveId),
                      }
                    : p,
            ),
        })),

    // === PARTICIPANTES ===
    addParticipant: (projectId, participant) =>
        set((state) => {
            const newId = participant.id ?? Date.now()
            const newParticipant: ProjectParticipant = { ...participant, id: newId }

            return {
                projects: state.projects.map((p) =>
                    p.id === projectId
                        ? {
                              ...p,
                              participantes: [...(p.participantes ?? []), newParticipant],
                          }
                        : p,
                ),
            }
        }),

    removeParticipant: (projectId, participantId) =>
        set((state) => ({
            projects: state.projects.map((p) =>
                p.id === projectId
                    ? {
                          ...p,
                          participantes: (p.participantes ?? []).filter(
                              (m) => m.id !== participantId,
                          ),
                      }
                    : p,
            ),
        })),

    // === DOCUMENTOS ===
    addDocument: (projectId, document) =>
        set((state) => {
            const newId = document.id ?? Date.now()
            const newDocument: ProjectDocument = { ...document, id: newId }

            return {
                projects: state.projects.map((p) =>
                    p.id === projectId
                        ? {
                              ...p,
                              documentos: [...(p.documentos ?? []), newDocument],
                          }
                        : p,
                ),
            }
        }),

    removeDocument: (projectId, documentId) =>
        set((state) => ({
            projects: state.projects.map((p) =>
                p.id === projectId
                    ? {
                          ...p,
                          documentos: (p.documentos ?? []).filter((d) => d.id !== documentId),
                      }
                    : p,
            ),
        })),
}))
