// src/services/api.ts

import axios from 'axios'
import type { AxiosInstance, AxiosError, InternalAxiosRequestConfig } from 'axios'
import StorageService from './StorageService'

/**
 * Instancia de Axios configurada con interceptores
 */

// URL base de la API (configurable desde variables de entorno)
const API_BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:8000/api'

// Crear instancia de Axios
const api: AxiosInstance = axios.create({
    baseURL: API_BASE_URL,
    timeout: 10000, // 10 segundos
    headers: {
        'Content-Type': 'application/json',
    },
})

/**
 * Interceptor de REQUEST
 * Agrega el token de autenticación a todas las peticiones si existe
 */
api.interceptors.request.use(
    (config: InternalAxiosRequestConfig) => {
        const token = StorageService.get<string>('token')

        if (token && config.headers) {
            config.headers.Authorization = `Bearer ${token}`
        }

        return config
    },
    (error: AxiosError) => {
        return Promise.reject(error)
    },
)

/**
 * Interceptor de RESPONSE
 * Maneja errores globales de respuesta
 */
api.interceptors.response.use(
    (response) => {
        return response
    },
    (error: AxiosError) => {
        // Manejar errores de autenticación (401)
        if (error.response?.status === 401) {
            // Token expirado o inválido
            StorageService.remove('token')
            StorageService.remove('user')

            // Redirigir al login si no estamos ya ahí
            if (window.location.pathname !== '/login') {
                window.location.href = '/login'
            }
        }

        // Manejar errores de servidor (500+)
        if (error.response && error.response.status >= 500) {
            console.error('Error del servidor:', error.response.data)
        }

        return Promise.reject(error)
    },
)

export default api
