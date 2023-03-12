import Layout from '@/layout/index.vue'

export const constantRoutes = [
  {
    path: '/login',
    name: 'Login',
    component: () => import('@/views/login/index.vue'),
    hidden: true,
    meta: { title: '登录', icon: 'login', isAsync: false }
  },
  {
    path: '/403',
    name: '403',
    meta: { title: '403', isAsync: false },
    component: () => import('@/views/error-page/403.vue'),
    hidden: true
  },
  {
    path: '/404',
    name: '404',
    meta: { title: '404', isAsync: false },
    component: () => import('@/views/error-page/404.vue'),
    hidden: true
  },
  {
    path: '/500',
    name: '500',
    meta: { title: '500', isAsync: false },
    component: () => import('@/views/error-page/500.vue'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    name: 'Dashboard',
    redirect: '/dashboard',
    children: [
      {
        path: '/dashboard',
        component: () => import('@/views/dashboard/index.vue'),
        name: 'Dashboard',
        meta: { title: '控制台', icon: 'DashboardOutlined', isAsync: false }
      }
    ]
  }
]

export const asyncRoutes = [
  {
    path: '/system',
    component: Layout,
    redirect: '/system/roleList',
    alwaysShow: true,
    name: 'System',
    meta: {
      title: '系统管理',
      icon: 'SettingOutlined',
      roles: ['admin']
    },
    children: [
      {
        path: '/system/role/roleList',
        component: () => import('@/views/system/role/index.vue'),
        name: 'RoleList',
        meta: {
          title: '角色管理',
          roles: ['admin'],
          isAsync: true
        }
      },
      {
        path: '/system/approvalFlowConfig',
        component: () => import('@/views/system/approvalFlowConfig.vue'),
        name: 'ApprovalFlowConfig',
        meta: {
          title: '审批流配置',
          roles: ['admin'],
          isAsync: true
        }
      }
    ]
  },
  {
    path: '/permission',
    component: Layout,
    redirect: '/system/approvalFlowConfig',
    alwaysShow: true,
    name: 'approvalFlowConfig',
    meta: {
      title: '权限管理',
      icon: 'KeyOutlined',
      roles: ['admin'],
      isAsync: true
    },
    children: [
      {
        path: '/system/approvalFlowConfig',
        component: () => import('@/views/system/approvalFlowConfig.vue'),
        name: 'approvalFlowConfig',
        meta: {
          title: '角色权限',
          roles: ['admin'],
          isAsync: true
        }
      }
    ]
  },
  {
    path: '/userCenter',
    component: Layout,
    redirect: '/userCenter/userSet',
    alwaysShow: true,
    name: 'UserCenter',
    meta: {
      title: '个人中心',
      icon: 'UserOutlined',
      roles: ['admin', 'editor'],
      isAsync: true
    },
    children: [
      {
        path: '/userCenter/userSet',
        component: () => import('@/views/userCenter/userSet.vue'),
        name: 'UserSet',
        meta: {
          title: '个人设置',
          roles: ['admin', 'editor'],
          isAsync: true
        }
      }
    ]
  },
  { path: '/:pathMatch(.*)*', name: 'NoFound', redirect: '/404', hidden: true }
]
//开发模式额外路由
export const localdevRoutes = [
  {
    path: '/developer',
    component: Layout,
    redirect: '/developer/index',
    alwaysShow: true,
    name: 'Developer',
    meta: {
      title: '开发',
      icon: 'SettingOutlined',
      roles: ['admin']
    },
    children: [
      {
        path: '/developer/index',
        component: () => import('@/views/developer/index.vue'),
        name: 'index',
        meta: {
          title: '代码生成',
          icon: 'SettingOutlined',
          roles: ['admin'],
        }
      },
      {
        path: '/developer/dbmanage',
        component: () => import('@/views/developer/dbmanage.vue'),
        name: 'dbmanage',
        meta: {
          title: '数据库管理',
          roles: ['admin']
        }
      }
    ]
  }
]

export default constantRoutes