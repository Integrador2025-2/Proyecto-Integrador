import { createBrowserRouter, Navigate } from 'react-router-dom'
import { PrivateRoute } from '../components/auth/PrivateRoute'
import { LoginPage } from '../pages/auth/LoginPage'
import { RegisterPage } from '../pages/auth/RegisterPage'
import { GoogleCallback } from '../pages/auth/GoogleCallback'
import { UnauthorizedPage } from '../pages/auth/UnauthorizedPage'
import { Layout } from '../components/layout'
import { Home } from '../pages/Home'

// Importar páginas futuras (por ahora comentadas hasta que se creen)
// import { ProjectsPage } from '../pages/projects/ProjectsPage';
// import { ProjectDetailPage } from '../pages/projects/ProjectDetailPage';
// import { CreateProjectPage } from '../pages/projects/CreateProjectPage';

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
        path: '/unauthorized',
        element: <UnauthorizedPage />,
    },
    {
        path: '/',
        element: <PrivateRoute />,
        children: [
            {
                path: '/',
                element: <Layout />,
                children: [
                    {
                        index: true,
                        element: <Home />,
                    },
                    // Rutas futuras de proyectos
                    // {
                    //   path: 'projects',
                    //   element: <ProjectsPage />,
                    // },
                    // {
                    //   path: 'projects/:id',
                    //   element: <ProjectDetailPage />,
                    // },
                    // {
                    //   path: 'projects/new',
                    //   element: <CreateProjectPage />,
                    // },

                    // Rutas solo para administradores
                    // {
                    //   path: 'admin',
                    //   element: <PrivateRoute requiredRoles={['Administrador']} />,
                    //   children: [
                    //     {
                    //       path: 'users',
                    //       element: <UsersManagementPage />,
                    //     },
                    //   ],
                    // },
                ],
            },
        ],
    },
    {
        path: '*',
        element: <Navigate to="/" replace />,
    },
])
