import 'core-js/stable';
import { createApp } from 'vue'
import App from '@/App.vue'
import { store } from '@/store'
import router from '@/router'
import _ from 'lodash'
import { setupAntd } from '@/plugins/antd'
import { setupDay } from '@/plugins/day'
import '@/router/permission' //路由权限
import permission from '@/directive/permission' // 权限按钮

//全局样式
import 'normalize.css/normalize.css'
import './styles/index.less'





const app = createApp(App)
app.use(store)
app.use(router)
app.config.productionTip = false
setupAntd(app)
setupDay(app)
app.directive('permission', permission)

app.mount('#app')


