import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/Home/Login',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 获取用户列表
 * @param {*} data 筛选数据及分页数据 
 */
export function getList(data) {
  return request({
    url: '/api/User/getList',
    method: 'post',
    data
  })
}
export function getInfo(token) {
  return request({
    url: '/jc-admin/user/info',
    method: 'get',
    params: { token }
  })
}
/**
 * @Descriptions 添加用户
 * @param {*} data 
 */
export function addUser(data) {
  return request({
    url: '/api/User/Add',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 更新用户
 * @param {*} data 
 */
export function UpdateUser(data) {
  return request({
    url: '/api/User/Update',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 删除用户
 * @param {Array<string>} ids 用户id  
 */
export function deleteUser(ids) {
  console.log(ids)
  return request({
    url: '/api/User/Delete',
    method: 'delete',
    data: ids
  })
}
export function logout() {
  return request({
    url: '/jc-admin/user/logout',
    method: 'post'
  })
}
