import { defineStore } from 'pinia'
import { store } from './index';
import { login, logout, getInfo } from '@/api/user'
import { getToken, setToken, removeToken } from '@/utils/auth'
import router, { resetRouter } from '@/router'
import { permissionStore } from '@/store/permission'


export const useUserStore = defineStore({
  id: 'app-user',
  state: () => ({
    token: '',
    introduction: '',
    name: '',
    avatar: '',
    roles: []
  }),
  persist: {
    enabled: true,
    strategies: [
      {
        key: 'app-user',
        storage: localStorage,
        paths: ['token','name','avatar','roles']
      }
    ]
  },
  getters: {
    getToken: () => getToken(),
    getAvatar: (state) => state.avatar,
    getIntroduction: (state) => state.introduction,
    getName: (state) => state.name,
    getRoles: (state) => state.roles,
    getPermission_routes: ()=>{
      const permission= permissionStore()
      return permission.routes
    }
  },
  actions: {  
    async userlogin(userInfo) {
      console.log(userInfo)
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
    async getInfo() {
      return new Promise((resolve, reject) => {
        getInfo(this.token)
          .then((response) => {
            const { data } = response

            if (!data) {
              reject('验证失败，请重新登录')
            }
            const { roles, name, avatar, introduction ,token} = data
            // roles必须存在
            if (!roles || roles.length <= 0) {
              reject('getInfo: roles不能为空!')
            }
            this.token=token;
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
    async logout() {
      return new Promise((resolve, reject) => {
        logout(this.token)
          .then(() => {
            this.token = ''
            this.roles = []
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
    async resetToken() {
      return new Promise((resolve) => {
        this.token = ''
        this.roles = []      
        resolve()
      })
    },
    // 动态修改权限
    async changeRoles(role) {
      const token = role + '-token'
      this.token = token
      setToken(token)

      const { roles } = await getInfo()

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

export function useUserStoreWithOut() {
  return useUserStore(store);
}
