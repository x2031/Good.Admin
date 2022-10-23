import {permissionStore} from '@/store/permission'

function checkPermission(el, binding) {
	const { value } = binding
  const store = permissionStore()
	const roles = store.roles

	if (value && value instanceof Array) {
		if (value.length > 0) {
			const permissionRoles = value

			const hasPermission = roles.some((role) => {
				return permissionRoles.includes(role)
			})

			if (!hasPermission) {
				el.parentNode && el.parentNode.removeChild(el)
			}
		}
	} else {
		throw new Error(`需要roles!！例如v-permission="['admin','editor']"`)
	}
}

export default {
	mounted(el, binding) {
		checkPermission(el, binding)
	},
	updated(el, binding) {
		checkPermission(el, binding)
	}
}
