import { defineStore } from 'pinia'
import { login, logout, getInfo } from '@/api/user'
import { getToken, setToken, removeToken } from '@/utils/auth'
import router, { resetRouter } from '@/router'


export const useUserStore = defineStore("user", {
  state: () => ({
    token: getToken(),
    introduction: '',
    name: '',
    avatar: '',
    roles: []
  }),
  getters: {
    token: (state) => state.user.token,
    avatar: (state) => state.user.avatar,
    introduction: (state) => state.user.introduction,
    name: (state) => state.user.name,
    roles: (state) => state.user.roles,
    permission_routes: (state) => state.permission.routes
  },
  action: {
    login(userInfo) {
      const { username, password } = userInfo
      return new Promise((resolve, reject) => {
        login({ username: username.trim(), password: password })
          .then((response) => {
            const { data } = response
            this.token = data.token
            //setToken(data.token)
            resolve()
          })
          .catch((error) => {
            reject(error)
          })
      })
    },
    // 获取用户信息
    getInfo({ commit, state }) {
      return new Promise((resolve, reject) => {
        getInfo(state.token)
          .then((response) => {
            const { data } = response

            if (!data) {
              reject('验证失败，请重新登录')
            }
            const { roles, name, avatar, introduction } = data
            // roles必须存在
            if (!roles || roles.length <= 0) {
              reject('getInfo: roles不能为空!')
            }
            this.roles = roles
            this.name = name
            this.avatar = avatar
            this.introduction = introduction
            resolve(data)
          })
          .catch((error) => {
            reject(error)
          })
      })
    },
    // 用户退出
    logout({ commit, state, dispatch }) {
      return new Promise((resolve, reject) => {
        logout(state.token)
          .then(() => {
            this.token = ''            
            this.roles=[]
            removeToken()
            resetRouter()
            setTimeout(() => {
              resolve()
            }, 1000)
          })
          .catch((error) => {
            reject(error)
          })
      })
    },
    // 移除token
    resetToken({ commit }) {
      return new Promise((resolve) => {
        this.token = ''
        this.roles=[]
        removeToken()
        resolve()
      })
    },
    // 动态修改权限
    async changeRoles({ commit, dispatch }, role) {
      const token = role + '-token'
      this.token = token
      setToken(token)

      const { roles } = await dispatch('getInfo')

      resetRouter()
      // 获取当前用户权限路由
      const accessRoutes = await dispatch('permission/generateRoutes', roles, { root: true })
      // 动态添加路由
      accessRoutes.forEach((route) => {
        router.addRoute(route)
      })
    }
  },
})

// 在组件setup函数外使用
// export function useUserStoreWithOut() {
//   return useUserStore(store);
// }
