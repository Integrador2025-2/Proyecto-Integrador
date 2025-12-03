import axios, { AxiosError, InternalAxiosRequestConfig } from 'axios'

const API_BASE_URL = import.meta.env.VITE_API_BASE_URL || 'https://localhost:7000/api'

const api = axios.create({
    baseURL: API_BASE_URL,
    headers: {
        'Content-Type': 'application/json',
    },
})

let isRefreshing = false
let failedQueue: Array<{
    resolve: (value?: unknown) => void
    reject: (reason?: any) => void
}> = []

const processQueue = (error: Error | null = null) => {
    failedQueue.forEach((prom) => {
        if (error) {
            prom.reject(error)
        } else {
            prom.resolve()
        }
    })

    failedQueue = []
}

// Request interceptor - Agregar token a todas las peticiones
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = localStorage.getItem('access_token')

        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`
        }

        return config
    },
    (error: AxiosError) => {
        return Promise.reject(error)
    },
)

// Response interceptor - Manejar refresh token automático
api.interceptors.response.use(
    (response) => response,
    async (error: AxiosError) => {
        const originalRequest = error.config as InternalAxiosRequestConfig & { _retry?: boolean }

        // Si el error no es 401 o ya se intentó renovar, rechazar
        if (error.response?.status !== 401 || originalRequest._retry) {
            return Promise.reject(error)
        }

        // Si ya se está renovando el token, agregar a la cola
        if (isRefreshing) {
            return new Promise((resolve, reject) => {
                failedQueue.push({ resolve, reject })
            })
                .then(() => {
                    return api(originalRequest)
                })
                .catch((err) => {
                    return Promise.reject(err)
                })
        }

        originalRequest._retry = true
        isRefreshing = true

        const refreshToken = localStorage.getItem('refresh_token')

        if (!refreshToken) {
            // No hay refresh token, limpiar y redirigir a login
            localStorage.removeItem('access_token')
            localStorage.removeItem('refresh_token')
            localStorage.removeItem('user')
            window.location.href = '/login'
            return Promise.reject(error)
        }

        try {
            // Intentar renovar el token
            const response = await axios.post(`${API_BASE_URL}/auth/refresh-token`, {
                refreshToken,
            })

            const { token, refreshToken: newRefreshToken } = response.data

            // Guardar nuevos tokens
            localStorage.setItem('access_token', token)
            localStorage.setItem('refresh_token', newRefreshToken)

            // Actualizar el token en el header de la petición original
            if (originalRequest.headers) {
                originalRequest.headers.Authorization = `Bearer ${token}`
            }

            // Procesar cola de peticiones pendientes
            processQueue()

            // Reintentar la petición original
            return api(originalRequest)
        } catch (refreshError) {
            // Si falla el refresh, limpiar y redirigir a login
            processQueue(refreshError as Error)
            localStorage.removeItem('access_token')
            localStorage.removeItem('refresh_token')
            localStorage.removeItem('user')
            window.location.href = '/login'
            return Promise.reject(refreshError)
        } finally {
            isRefreshing = false
        }
    },
)

export default api
