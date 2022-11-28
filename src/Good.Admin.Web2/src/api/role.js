import request from '@/utils/request'
import { Descriptions } from 'ant-design-vue'

export function getRoutes() {
  return request({
    url: '/jc-admin/routes',
    method: 'get'
  })
}

export function getRoles() {
  return request({
    url: '/jc-admin/roles',
    method: 'get'
  })
}

export function addRole(data) {
  return request({
    url: '/jc-admin/role',
    method: 'post',
    data
  })
}

/**
 * @Descriptions 更新角色
 * @param {String} id 角色id
 * @param {*} data 角色数据 
 */
export function updateRole(id, data) {
  return request({
    url: `/jc-admin/role/${id}`,
    method: 'put',
    data
  })
}
/**
 * @description: 删除角色
 * @param {String} id 角色id  
 */
export function deleteRole(id) {
  return request({
    url: `/jc-admin/role/${id}`,
    method: 'delete'
  })
}
