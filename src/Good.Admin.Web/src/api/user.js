import request from '@/utils/request'

export function login(data) {
  return request({
    url: '/api/Home/Login',
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
