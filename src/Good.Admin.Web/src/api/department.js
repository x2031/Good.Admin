import request from '@/utils/request'


/**
 * @Descriptions 获取部门信息
 * @param {*} data 父级id
 */
export function GetDepartmentInfo(data) {
  return request({
    url: '/api/Department/GetListByParentId',
    method: 'post',
    data
  })
}

