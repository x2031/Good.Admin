import { defineStore } from 'pinia'
import { asyncRoutes, constantRoutes } from '@/router/router.config'
/**
 * 通过meta.role来判断当前用户是否有权限
 * @param roles
 * @param route
 */
function hasPermission(roles, route) {
  if (route.meta && route.meta.roles) {
    return roles.some((role) => route.meta.roles.includes(role))
  } else {
    return true
  }
}

/**
 * 通过递归过滤异步路由
 * @param routes asyncRoutes
 * @param roles
 */
export function filterAsyncRoutes(routes, roles) {
  const res = []
  routes.forEach((route) => {
    const tmp = { ...route }
    if (hasPermission(roles, tmp)) {
      if (tmp.children) {
        tmp.children = filterAsyncRoutes(tmp.children, roles)
      }
      res.push(tmp)
    }
  })
  return res
}
export const permissionStore = defineStore({
  id: 'app-permission',
  state: () => ({
    routes: [],
    addRoutes: []
  }),
  persist: {
    enabled: true,
    strategies: [
      {
        key: 'permission',
        storage: localStorage
      }
    ]
  },
  actions: {
    //权限判断
    generateRoutes(roles) {
      console.log('获取权限路由')
      console.log(roles)
      let accessedRoutes
      if (roles.includes('admin')) {
        console.log('admin')
        console.log(asyncRoutes)
        accessedRoutes = asyncRoutes || []
      } else {
        accessedRoutes = filterAsyncRoutes(asyncRoutes, roles)
      }
      this.addRoutes = accessedRoutes;
      accessedRoutes = constantRoutes.concat(this.addRoutes);
      this.routes = accessedRoutes
      console.log(this.routes)
      return accessedRoutes
    },
    async getRouteIsAsync(path) {
      let isAsync = false
      if (this.routes.find(x => x.path == path)) {
        isAsync = this.routes.find(x => x.path == path).meta.isAsync
      }
      else {
        this.routes.forEach(element => {
          if (element.children) {
            if (element.children.find(x => x.path == path)) {
              isAsync = element.children.find(x => x.path == path).meta.isAsync
            }
          }
        })
      }
      return isAsync
    }
  },
})

