import axios from 'axios'
import { message } from 'ant-design-vue'
import { useUserStoreWithOut } from '@/store/user'
import 'ant-design-vue/es/message/style/css';
import { v4 as uuid } from 'uuid'
import md5 from 'crypto-js/md5'

const service = axios.create({
  baseURL: import.meta.env.VITE_APP_BASE_API, // url = base url + request url  
  timeout: import.meta.env.VITE_APP_TIMEOUT // 请求超时
})

// 在发送请求之前做某件事
service.interceptors.request.use((config) => {
  //CheckSign签名检验
  // let appId = defaultSettings.appId
  // let appSecret = defaultSettings.appSecret
  // let guid = uuid()
  // let time = moment().format("YYYY-MM-DD HH:mm:ss")
  // let body = ''
  // if (config.data) {
  //   body = JSON.stringify(config.data)
  // }
  // let sign = md5(appId + time + guid + body + appSecret)
  //  config.headers.appId = appId;
  //  config.headers.time = time;
  //  config.headers.guid = guid;
  //  config.headers.sign = sign;  
  let reg = /api/
  if (!reg.test(config.url)) {
    config.baseURL = 'http://127.0.0.1:5173'
  }
  else {
    config.baseURL = import.meta.env.VITE_APP_BASE_API
  }
  if (useUserStoreWithOut().token) {
    const token_type = "Bearer "
    config.headers.Authorization = token_type + useUserStoreWithOut().token
  }
  return config
}, (error) => {
  // 请求错误
  return Promise.reject(error)
}
)
//返回状态判断(添加响应拦截器)
service.interceptors.response.use((response) => {
  const res = response.data
  if (res.code !== 200) {
    //  message.error(res.msg || 'Error')
    // 508: 非法token; 512: 其他用户已登录; 514: Token过期;
    if (res.code === 401) {
      // 重新登录
      Modal.confirm({
        title: '确认注销',
        content: '您已经注销，您可以取消以留在此页面，也可以重新登录',
        okText: '重新登录',
        cancelText: '取消',
        onOk() {
          useUserStoreWithOut.resetToken()
            .then(() => {
              location.reload()
            })
        },
        onCancel() { }
      })
    }
    else if (res.code === 500) {
      //提示
      //服务器内部错误
      //message.error(res.msg || 'Error', 3)
      return Promise.reject(new Error(res.msg || 'Error'))
    }
    else if (res.code === 503) {
      //提示
      //服务挂掉了
      message.error('服务器不可用' || 'Error', 3)
    }
    else {
      return Promise.reject(new Error(res.msg || 'Error'))
    }
  }
  else {
    return res
  }
},
  (error, res) => {
    message.error(error.message || 'Error', 3)
    return Promise.reject(error)
  }
)

export default service
