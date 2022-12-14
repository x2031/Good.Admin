import { defineConfig, loadEnv } from 'vite'
import vue from '@vitejs/plugin-vue'
import path from 'path'
import vueJsx from '@vitejs/plugin-vue-jsx'
import legacy from '@vitejs/plugin-legacy'
import mockPlugin from 'vite-plugin-mockit'
import { qrcode } from 'vite-plugin-qrcode'
import Components from 'unplugin-vue-components/vite'
import { AntDesignVueResolver } from 'unplugin-vue-components/resolvers'
import { createStyleImportPlugin, VxeTableResolve } from 'vite-plugin-style-import'

// https://vitejs.dev/config/

export default defineConfig(({ command, mode }) => {
  const env = loadEnv(mode.mode, process.cwd())
  return {
    baseUrl: './',
    resolve: {
      alias: [
        { find: '@', replacement: path.resolve(__dirname, './src') }
      ],
      extensions: ['.js', '.ts', '.mjs', '.jsx', '.tsx', '.less', '.json']
    },
    clearScreen: false, // vite清屏不清除控制台打印的信息   
    server: {
      open: true, //启动时是否自动打开      
    },
    define: {
      'process.env': {}
    },
    logLevel: 'info',
    plugins: [
      Components({
        resolvers: [AntDesignVueResolver()],
      }),
      createStyleImportPlugin({
        resolves: [
          VxeTableResolve()
        ],
      }),
      vue(),
      vueJsx(),
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
  }
})

// export default defineConfig({
//   baseUrl: './',
//   resolve: {
//     alias: [
//       { find: '@', replacement: path.resolve(__dirname, './src') }
//     ],
//     extensions: ['.js', '.ts', '.mjs', '.jsx', '.tsx', '.less', '.json']
//   },
//   clearScreen: false, // vite清屏不清除控制台打印的信息   
//   server: {
//     //本地服务
//     //port: 3001, //端口号
//     open: true, //启动时是否自动打开
//     proxy: {
//       '/api': {
//         target: env.VITE_APP_BASE_API,
//         changeOrigin: true,
//         rewrite: (path) => path.replace(/^\/api/, "")
//       }
//     },
//   },
//   define: {
//     'process.env': {}
//   },
//   logLevel: 'info',
//   plugins: [
//     Components({
//       resolvers: [AntDesignVueResolver()],
//     }),
//     vue(),
//     vueJsx(),

//     mockPlugin({
//       entry: './mock/index.js',
//       watchFiles: [], // watch file or dir change refresh mock
//       watchOptions: {}, //extension option from chokidar option
//       disable: false // default false
//     }),
//     //旧版浏览器支持
//     legacy({
//       targets: ['defaults', 'not IE 10'],
//       ignoreBrowserslistConfig: true,
//     }),
//     qrcode()
//   ],
//   css: {
//     preprocessorOptions: {
//       less: {
//         javascriptEnabled: true
//       }
//     }
//   }
// })
