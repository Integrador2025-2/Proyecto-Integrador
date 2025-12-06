import { useEffect } from 'react'
import { RouterProvider } from 'react-router-dom'
import { router } from './routes/router'
import { useAuthStore } from './store/authStore'

function App() {
    const initializeAuth = useAuthStore((state) => state.initializeAuth)

    useEffect(() => {
        // Inicializar el estado de autenticación desde localStorage al montar la app
        initializeAuth()
    }, []) // Ejecutar solo una vez al montar el componente

    return <RouterProvider router={router} />
}

export default App
