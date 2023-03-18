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

export function logout() {
  return request({
    url: '/jc-admin/user/logout',
    method: 'post'
  })
}
