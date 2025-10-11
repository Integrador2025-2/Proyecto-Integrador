// src/services/StorageService.ts

/**
 * Servicio para manejar el almacenamiento local (localStorage)
 * Proporciona métodos seguros con manejo de errores
 */

class StorageService {
    /**
     * Guarda un valor en localStorage
     * @param key - Clave bajo la cual se guardará el valor
     * @param value - Valor a guardar (será serializado a JSON)
     */
    set<T>(key: string, value: T): void {
        try {
            const serializedValue = JSON.stringify(value)
            localStorage.setItem(key, serializedValue)
        } catch (error) {
            console.error(`Error al guardar en localStorage (${key}):`, error)
        }
    }

    /**
     * Obtiene un valor de localStorage
     * @param key - Clave del valor a obtener
     * @returns El valor deserializado o null si no existe o hay error
     */
    get<T>(key: string): T | null {
        try {
            const item = localStorage.getItem(key)
            if (!item) return null
            return JSON.parse(item) as T
        } catch (error) {
            console.error(`Error al leer de localStorage (${key}):`, error)
            return null
        }
    }

    /**
     * Elimina un valor de localStorage
     * @param key - Clave del valor a eliminar
     */
    remove(key: string): void {
        try {
            localStorage.removeItem(key)
        } catch (error) {
            console.error(`Error al eliminar de localStorage (${key}):`, error)
        }
    }

    /**
     * Limpia todo el localStorage
     */
    clear(): void {
        try {
            localStorage.clear()
        } catch (error) {
            console.error('Error al limpiar localStorage:', error)
        }
    }

    /**
     * Verifica si existe una clave en localStorage
     * @param key - Clave a verificar
     * @returns true si existe, false si no
     */
    has(key: string): boolean {
        return localStorage.getItem(key) !== null
    }
}

export default new StorageService()
