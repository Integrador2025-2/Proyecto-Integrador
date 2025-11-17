import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react-swc'
import tailwindcss from '@tailwindcss/vite'
import tsconfigPaths from 'vite-tsconfig-paths'

// https://vite.dev/config/
export default defineConfig({
    plugins: [react(), tailwindcss(), tsconfigPaths()],
    server: {
        open: true, // abre el navegador al iniciar
        port: 3000, // puerto del servidor
    },
})
