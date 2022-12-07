import { defineStore } from 'pinia'
import { store } from './index';
import { login, logout, getInfo } from '@/api/user'
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
        paths: ['token', 'name', 'avatar', 'roles']
      }
    ]
  },
  getters: {
    getToken: (state) => state.token,
    getAvatar: (state) => state.avatar,
    getIntroduction: (state) => state.introduction,
    getName: (state) => state.name,
    getRoles: (state) => state.roles,
    getPermission_routes: () => {
      const permission = permissionStore()
      return permission.routes
    }
  },
  actions: {
    async userlogin(userInfo) {
      let resdata
      const { username, password } = userInfo
      await login({ username: username.trim(), password: password }).then(response => {
        console.log(response)
        const { Data } = response
        this.token = Data.token
        resdata = Data
      })
      return resdata
    },
    // 获取用户信息
    async getInfo() {
      let resdate
      await getInfo(this.token).then(response => {
        console.log(response)
        if (response) {
          const { data } = response
          if (!data) {
            // reject('验证失败，请重新登录')
          }
          const { roles, name, avatar, introduction } = data
          // roles必须存在
          if (!roles || roles.length <= 0) {
            // reject('getInfo: roles不能为空!')
          }

          this.roles = roles
          this.name = name
          this.introduction = introduction
          this.avatar = avatar
          console.log(data)
          resdate = data
        }
      })
      return resdate
    },
    // 用户退出
    async logout() {

      await logout(this.token).then(() => {
        this.token = ''
        this.roles = []
        resetRouter()
      }).catch((error) => { reject(error) })
    },
    // 移除token
    async resetToken() {
      this.token = ''
      this.roles = []
    },
    // 动态修改权限
    async changeRoles(role) {
      const token = role + '-token'
      this.token = token
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
