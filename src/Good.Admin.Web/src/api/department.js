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

/**
 * @Descriptions 获取部门列表
 * @param {*} data 筛选数据及分页数据 
 */
export function getDepartments(data) {
  return request({
    url: '/api/Department/GetList',
    method: 'post',
    data
  })
}

/**
 * @Descriptions 添加部门
 * @param {*} data 
 */
export function addDepartment(data) {
  return request({
    url: '/api/Department/Add',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 更新部门
 * @param {*} data 
 */
export function UpdateDepartment(data) {
  return request({
    url: '/api/Department/Update',
    method: 'post',
    data
  })
}
/**
 * @Descriptions 删除部门
 * @param {Array<string>} ids 角色id  
 */
export function deleteDepartment(ids) {
  console.log(ids)
  return request({
    url: '/api/Department/Delete',
    method: 'delete',
    data: ids
  })
}