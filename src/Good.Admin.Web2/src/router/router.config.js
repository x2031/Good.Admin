import Layout from '@/layout/index.vue'

export const constantRoutes = [
	{
		path: '/login',
		name: 'Login',  
		component: () => import('@/views/login/index.vue'),
		hidden: true,
		meta: { title: '登录', icon: 'login' }
	},
	{
		path: '/403',
		name: '403',
		component: () => import('@/views/error-page/403.vue'),
		hidden: true
	},
	{
		path: '/404',
		name: '404',
		component: () => import('@/views/error-page/404.vue'),
		hidden: true
	},
	{
		path: '/500',
		name: '500',
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
				meta: { title: '控制台', icon: 'DashboardOutlined' }
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
        path: '/system/roleList',
        component: () => import('@/views/system/roleList.vue'),
        name: 'RoleList',
        meta: {
          title: '角色管理',
          roles: ['admin']
        }
      },
      {
        path: '/system/approvalFlowConfig',
        component: () => import('@/views/system/approvalFlowConfig.vue'),
        name: 'ApprovalFlowConfig',
        meta: {
          title: '审批流配置',
          roles: ['admin']
        }
      }
    ]
  },
	{
    path: '/permission',
    component: Layout,
    redirect: '/permission/rolePermissionList',
    alwaysShow: true,
    name: 'Permission',
    meta: {
      title: '权限管理',
      icon: 'KeyOutlined',
      roles: ['admin']
    },
    children: [
      {
        path: '/permission/rolePermissionList',
        component: () => import('@/views/permission/rolePermissionList.vue'),
        name: 'RolePermissionList',
        meta: {
          title: '角色权限',
          roles: ['admin']
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
      roles: ['admin', 'editor']
    },
    children: [
      {
        path: '/userCenter/userSet',
        component: () => import('@/views/userCenter/userSet.vue'),
        name: 'UserSet',
        meta: {
          title: '个人设置',
          roles: ['admin', 'editor']
        }
      }
    ]
  },
	{ path: '/:pathMatch(.*)*', name: 'NoFound', redirect: '/404', hidden: true }
]

export default constantRoutes