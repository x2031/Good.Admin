import { createRouter, createWebHistory } from 'vue-router'
import routes from './router.config';

const router = createRouter({
  routes: routes,
  history: createWebHistory(),
  scrollBehavior: () => ({ left: 0, top: 0 })
})

export function resetRouter() {
  const newRouter = createRouter({
    history: createWebHistory('/'),
    routes: routes,
    scrollBehavior: () => ({ left: 0, top: 0 })
  })

  router.matcher = newRouter.matcher // reset router  踩坑处！！！
}

export default router
