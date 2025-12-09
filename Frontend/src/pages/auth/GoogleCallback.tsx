import { useEffect, useRef } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'

export default function GoogleCallback() {
    const location = useLocation()
    const navigate = useNavigate()
    const { handleGoogleCallback } = useAuth()
    const hasRun = useRef(false)

    useEffect(() => {
        if (hasRun.current) return // evita doble ejecución
        hasRun.current = true

        const params = new URLSearchParams(location.search)
        const code = params.get('code')
        const errorParam = params.get('error')

        if (errorParam) {
            console.error('Google auth error:', errorParam)
            navigate('/login')
            return
        }

        if (!code) {
            console.error('No auth code provided in Google callback')
            navigate('/login')
            return
        }

        handleGoogleCallback(code).catch((err) => {
            console.error('Error handling Google callback:', err)
            navigate('/login')
        })
    }, [location.search, handleGoogleCallback, navigate])

    // <-- AÑADIR RETURN CON JSX
    return (
        <div className="flex h-screen items-center justify-center">
            <p className="text-sm text-gray-600">Procesando autenticación con Google...</p>
        </div>
    )
}
