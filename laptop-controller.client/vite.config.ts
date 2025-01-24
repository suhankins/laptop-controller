import { fileURLToPath, URL } from 'node:url';

import { defineConfig } from 'vite';
import plugin from '@vitejs/plugin-react';
import { env } from 'process';

const target = env.ASPNETCORE_HTTP_PORT ? `http://localhost:${env.ASPNETCORE_HTTP_PORT}` :
    env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:7071';

// https://vitejs.dev/config/
export default defineConfig({
    plugins: [plugin()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        host: true,
        proxy: {
            '^/screenbrightness': {
                target,
                secure: false
            },
            '^/keyboardbrightness': {
                target,
                secure: false
            },
            '^/volume': {
                target,
                secure: false
            },
        },
        port: 56057
    }
})
