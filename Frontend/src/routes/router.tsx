import { createBrowserRouter, Navigate } from 'react-router-dom'
import ProtectedRoute from '../components/ProtectedRoute'
import MainLayout from '../components/layout/MainLayout'
import LoginPage from '../pages/auth/LoginPage'
import DashboardPage from '../pages/Dashboard/DashboardPage'
import ProjectsPage from '../pages/Projects/ProjectsPage'
import ProjectDetailPage from '../pages/Projects/ProjectDetailPage'
import GoogleCallback from '../pages/auth/GoogleCallback'
import CreateProjectPage from '../pages/Projects/CreateProjectPage'
import ActivitiesListPage from '@/pages/activities/ActivitiesListPage'
import ActivityDetailPage from '@/pages/activities/ActivityDetailPage'
import QueryPage from '../pages/RAG/QueryPage'
import DocumentsPage from '../pages/RAG/DocumentsPage'
import BudgetGenerationPage from '../pages/RAG/BudgetGenerationPage'
import RegisterPage from '../pages/auth/RegisterPage'

export const router = createBrowserRouter([
    {
        path: '/login',
        element: <LoginPage />,
    },
    {
        path: '/register',
        element: <RegisterPage />,
    },
    {
        path: '/auth/google/callback',
        element: <GoogleCallback />,
    },
    {
        path: '/',
        element: (
            <ProtectedRoute>
                <MainLayout />
            </ProtectedRoute>
        ),
        children: [
            {
                index: true,
                element: <Navigate to="/dashboard" replace />,
            },
            {
                path: 'dashboard',
                element: <DashboardPage />,
            },
            {
                path: 'projects',
                element: <ProjectsPage />,
            },
            {
                path: 'projects/new',
                element: <CreateProjectPage />,
            },
            {
                path: 'projects/:id',
                element: <ProjectDetailPage />,
            },
            {
                path: 'activities',
                element: <ActivitiesListPage />,
            },
            {
                path: 'activities/:id',
                element: <ActivityDetailPage />,
            },
            {
                path: 'rag/query',
                element: <QueryPage />,
            },
            {
                path: 'rag/documents',
                element: <DocumentsPage />,
            },
            {
                path: 'rag/budget-generation',
                element: <BudgetGenerationPage />,
            },
        ],
    },
    {
        path: '*',
        element: <Navigate to="/" replace />,
    },
])
