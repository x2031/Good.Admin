import {permissionStore} from '@/store/permission'

/**
 * @param {Array} value
 * @returns {Boolean}
 * @example see @/views/permission/directive.vue
 */

export default function checkPermission(value) {
  const store=permissionStore()
	if (value && value instanceof Array && value.length > 0) {
		const roles =  store.roles
		const permissionRoles = value

		const hasPermission = roles.some((role) => {
			return permissionRoles.includes(role)
		})
		return hasPermission
	} else {
		return false
	}
}
