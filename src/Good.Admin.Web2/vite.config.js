import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'
import { resolve } from 'path'
import vueJsx from '@vitejs/plugin-vue-jsx'
import legacy from '@vitejs/plugin-legacy'
import mockPlugin from 'vite-plugin-mockit'
import { qrcode } from 'vite-plugin-qrcode'

// https://vitejs.dev/config/
export default defineConfig({
  resolve: {
    alias: {
      '@': resolve(__dirname, 'src')
    },
    extensions: ['.mjs', '.js', '.ts', '.jsx', '.tsx', '.json']
  },
  clearScreen: false, // vite清屏不清除控制台打印的信息   
  server: {
    //本地服务
    //port: 3001, //端口号
    open: true, //启动时是否自动打开
    proxy: {
      // '/vue-manage': {
      //   target: env.VITE_BASE_URL,
      //   changeOrigin: true,
      // }     
    },
  },
  define: {
    'process.env': {}
  },
  logLevel: 'info',
  plugins: [
    vue(),
    vueJsx(),
    //mock 
    mockPlugin({
      entry: './mock/index.js',
      watchFiles: [], // watch file or dir change refresh mock
      watchOptions: {}, //extension option from chokidar option
      disable: false // default false
    }),
    //旧版浏览器支持
    legacy({
      targets: ['defaults', 'not IE 10'],
      ignoreBrowserslistConfig: true,
    }),
    qrcode()

  ],
  css: {
    preprocessorOptions: {
      less: {
        javascriptEnabled: true
      }
    }
  }
})
