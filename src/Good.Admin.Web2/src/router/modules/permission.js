import Layout from '@/layout/index.vue'

const permissionRouter = {
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
}

export default permissionRouter
