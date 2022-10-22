import { defineStore } from 'pinia'
import { asyncRoutes, constantRoutes } from '@/router'
// TODO 路由生成重写
// TODO 页面权限重写
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
export const useUserStore = defineStore("permission", {
  state: () => ({
    token: getToken(),
    introduction: '',
    name: '',
    avatar: '',
    roles: [],
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
  action: {
    //权限判断
    generateRoutes({ commit }, roles) {
      return new Promise((resolve) => {
        let accessedRoutes
        if (roles.includes('admin')) {
          accessedRoutes = asyncRoutes || []
        } else {
          accessedRoutes = filterAsyncRoutes(asyncRoutes, roles)
        }
        commit('SET_ROUTES', accessedRoutes)
        this.addRoutes = accessedRoutes;
        this.routes = constantRoutes.concat(routes);
        resolve(accessedRoutes)
      })
    }
  },
})

