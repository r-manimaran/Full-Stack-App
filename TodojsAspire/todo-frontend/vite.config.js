import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig(() => {
  const apiUrl = process.env.services__apiservice__https__0 || process.env.services__apiservice__http__0;

  
  return {
    plugins: [react()],
    define: {
      __API_URL__: JSON.stringify(apiUrl)
    },
    server: {
      port: parseInt(process.env.VITE_PORT) || 3000,
      proxy: {
        '/api': {
          target: apiUrl,
          changeOrigin: true,
          secure: false,

          rewrite: (path) => path.replace(/^\/api/, '')
        }
      }
    },
    build: {
      outDir: 'dist',
      rollupOptions: {
        input: './index.html'
      }
    }
  }
})