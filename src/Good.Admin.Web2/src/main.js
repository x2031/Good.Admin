import 'core-js/stable';
import { createApp } from 'vue'
import App from '@/App.vue'
import {setupStore} from '@/store'
import router from '@/router'
import '@/permission' //路由权限
import permission from '@/directive/permission' // 权限按钮
import _ from 'lodash'

import {setupAntd} from '@/plugins/antd'
import {setupDay} from '@/plugins/day'
//全局样式
import 'normalize.css/normalize.css'
// import '@/styles/index.less'


const app = createApp(App)
app.config.productionTip = false
app.directive('permission', permission)
//注册antd
setupDay(app)
setupAntd(app)
app.use(router)
setupStore(app)

app.mount('#app')
