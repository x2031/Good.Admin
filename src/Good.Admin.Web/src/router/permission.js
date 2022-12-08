import router from '.'
import { permissionStore } from '@/store/permission'
import { useUserStore } from '@/store/user'
import NProgress from 'nprogress'
import getPageTitle from '@/utils/get-page-title'
import 'nprogress/nprogress.css'// 进度条样式

NProgress.configure({ showSpinner: false })
let accessRoutes
const whiteList = ['/login', '/redirect'] //重定向白名单

router.beforeEach(async (to, from, next) => {
  console.log("正在获取路由")
  NProgress.start()
  const userStore = useUserStore()
  const permissioStore = permissionStore()
  //设置页面title
  document.title = getPageTitle(to.meta.title)
  console.log(document.title)
  // 获取用户token
  if (userStore.token) {
    console.log('获取到token')
    if (to.path === '/login') {
      next({ path: '/' })
      NProgress.done()
    } else {
      console.log('开始获取路由')
      // 判断用户获取到权限
      const hasRoles = userStore.roles && userStore.roles.length > 0
      const hasRoute = router.hasRoute(to.name);
      console.info("to.name:", to)
      console.log("hasRoute:", hasRoute)
      console.log("from:", from)
      if (hasRoles) {
        if (!hasRoute) {
          // 获取权限路由
          await userStore.getInfo().then(res => {
            const { roles } = res
            accessRoutes = permissioStore.generateRoutes(roles)
            accessRoutes.forEach((route) => {
              router.addRoute(route)
            })
            next({ ...to, replace: true })// 导航不会留下历史记录
          })
        }
        else {
          next()
        }
      } else {
        try {
          // 获取用户信息
          console.log("获取用户信息")
          // 获取权限路由
          await userStore.getInfo().then(res => {
            const { roles } = res
            accessRoutes = permissioStore.generateRoutes(roles)
            accessRoutes.forEach((route) => {
              router.addRoute(route)
            })
            next({ ...to, replace: true })// 导航不会留下历史记录
          })
        } catch (error) {
          // 移除token 返回登录
          await userStore.resetToken()
          console.log(error.message)
          //message.error(error.message || '有错误')
          next(`/login?redirect=${to.path}`)
          NProgress.done()
        }
      }
    }
  }
  //未登录
  else {
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
