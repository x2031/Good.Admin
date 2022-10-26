import axios from 'axios'
import { message, Modal } from 'ant-design-vue'
import { useUserStoreWithOut } from '@/store/user'
import { getToken } from '@/utils/auth'
import { v4 as uuid } from 'uuid'
import md5 from 'crypto-js/md5'

const service = axios.create({
  //baseURL: process.env.APP_BASE_API, // url = base url + request url
  //withCredentials: true, // send cookies when cross-domain requests
  timeout: process.env.APP_TIMEOUT // 请求超时
})

// TODO 异常拦截处理器

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


  if (useUserStoreWithOut.token) {
    // const token = storage.get('X-Token')
    // if (token) {
    //   console.debug(token)
    //   //config.headers['Access-Token'] = token
    //   config.headers['X-Token'] = getToken()
    // }
    config.headers['X-Token'] = getToken()
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
    message.error(res.message || 'Error')
    // 508: 非法token; 512: 其他用户已登录; 514: Token国企;
    if (res.code === 508 || res.code === 512 || res.code === 514) {
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
    return Promise.reject(new Error(res.message || 'Error'))
  }
  else {
    return res
  }
},
  (error, res) => {
    message.error(error.message || 'Error', 5000)
    return Promise.reject(error)
  }
)

export default service
