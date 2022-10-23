import router from '.'
import { permissionStore } from '@/store/permission'
import { useUserStore } from '@/store/user'
import { message } from 'ant-design-vue'
import NProgress from 'nprogress'
// import { getToken } from '@/utils/auth'
import getPageTitle from '@/utils/get-page-title'

import 'nprogress/nprogress.css'
NProgress.configure({ showSpinner: false })

const whiteList = ['/login', '/redirect'] // 没有重定向白名单


router.beforeEach(async (to, from, next) => {
  console.log("正在获取路由")
  const permissioStore = permissionStore()
  const userStore = useUserStore()
  NProgress.start()
  //设置页面title
  document.title = getPageTitle(to.meta.title)
  console.log(document.title)
  // 判断用户是否登录
  // const hasToken = getToken()
  if (userStore.token) {
    console.log('重定向到主页')
    if (to.path === '/login') {
      // 如果已登录，重定向主页
      next({ path: '/' })
      NProgress.done()
    } else {
      console.log('开始获取路由')
      // 判断用户获取到权限
      const hasRoles = userStore.roles && userStore.roles.length > 0
      console.log(hasRoles)
      if (hasRoles) {
        next()
      } else {
        try {
          // 获取用户信息
          console.log("获取用户信息")
          const { roles } = await userStore.getInfo()
          // 获取权限路由
          console.log("1")
          const accessRoutes = await permissioStore.generateRoutes(roles)
          console.log("2")
          console.log(accessRoutes)
          // 动态添加权限路由 vue-router4 api 修改为单个添加
          accessRoutes.forEach((route) => {
            router.addRoute(route)
          })
          console.log("3")
          // 导航不会留下历史记录
          next({ ...to, replace: true })
          console.log("4")
        } catch (error) {
          // 移除token 返回登录
          await userStore.resetToken()
          console.log(error.message)
          message.error(error.message || '有错误')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  }
  //未登录
  else {
    console.log(whiteList)
    if (whiteList.indexOf(to.path) !== -1) {
      next()
    } else {
      // 其他没有访问权限的页面被重定向到登录页面
      next(`/login?redirect=${to.path}`)
      NProgress.done()
    }
  }
})

router.afterEach(() => {
  NProgress.done()
})
