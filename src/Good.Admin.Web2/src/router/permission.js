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
  NProgress.start()
  const userStore = useUserStore()
  const permissioStore = permissionStore()
  //设置页面title
  document.title = getPageTitle(to.meta.title)
  console.log(document.title)
  // 判断用户是否登录
  // const hasToken = getToken()
  console.log(userStore.token)
  if (userStore.token) {
    console.log('获取到token')
    if (to.path === '/login') {
      console.log('/login')
      //如果已登录，重定向主页     
      next({ path: '/' })
      NProgress.done()
    } else {
      console.log('开始获取路由')
      // 判断用户获取到权限
      const hasRoles = userStore.roles && userStore.roles.length > 0
      console.log(hasRoles)
      if (hasRoles) {
        console.log('已获取到权限,正常访问页面')
        // await userStore.getInfo().then(res => {
        //   console.log(res)
        //   const { roles } = res
        //   console.log(roles)
        //   let accessRoutes = permissioStore.generateRoutes(roles)
        //   accessRoutes.forEach((route) => {
        //     console.log(route)
        //     !router.hasRoute(route) || router.addRoute(route) // 动态添加路由          
        //   })
        //   next({ ...to, replace: true })
        // })


        next()
      } else {
        try {
          // 获取用户信息
          console.log("获取用户信息")
          let accessRoutes
          console.log("1")
          await userStore.getInfo().then(res => {
            console.log(res)
            const { roles } = res
            console.log(roles)
            accessRoutes = permissioStore.generateRoutes(roles)
          })
          // 获取权限路由
          console.log("2")
          console.log(accessRoutes)
          // 动态添加权限路由 vue-router4 api 修改为单个添加
          accessRoutes.forEach((route) => {
            router.addRoute(route)
          })
          console.log("3")
          next({ ...to, replace: true })// 导航不会留下历史记录，而是直接替换掉当前的 history 记录。这在你不想留下 history 记录的时候非常有用，比如在登录后跳转到首页。          
          console.log("4")
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
