import request from '@/utils/request'
import { Descriptions } from 'ant-design-vue'

/**
 * @Descriptions 获取角色列表
 * @param {*} data 筛选数据及分页数据 
 */
export function getRoles(data) {
  return request({
    url: '/api/Role/getList',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 根据id获取单个角色信息
 * @param {string} id 角色id 
*/
export function getRole(id) {
  return request({
    url: `/api/Role/GetTheData?id=${id}`,
    method: 'get',
  })
}

/**
 * @Descriptions 添加角色
 * @param {*} data 
 */
export function addRole(data) {
  return request({
    url: '/api/Role/Save',
    method: 'post',
    data
  })
}

/**
 * @Descriptions 删除角色
 * @param {Array<string>} ids 角色id  
 */
export function deleteRole(ids) {
  console.log(ids)
  return request({
    url: '/api/Role/Delete',
    method: 'delete',
    data: ids
  })
}
