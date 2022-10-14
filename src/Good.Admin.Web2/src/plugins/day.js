import dayjs from 'dayjs'
import 'dayjs/locale/zh-cn'
import relativeTime from 'dayjs/plugin/relativeTime'
import isToday from 'dayjs/plugin/isToday'
import isBetween from 'dayjs/plugin/isBetween'
import dayOfYear from 'dayjs/plugin/dayOfYear'
import customParseFormat from 'dayjs/plugin/customParseFormat'
import advancedFormat from 'dayjs/plugin/advancedFormat'
import weekOfYear from 'dayjs/plugin/weekOfYear'
import weekday from 'dayjs/plugin/weekday'
import objectSupport from 'dayjs/plugin/objectSupport'

dayjs.locale('zh-cn')
dayjs.extend(relativeTime)// 配置使用处理相对时间的插件
dayjs.extend(isToday)
dayjs.extend(isBetween)
dayjs.extend(dayOfYear)
dayjs.extend(customParseFormat)
dayjs.extend(advancedFormat)
dayjs.extend(weekOfYear)
dayjs.extend(objectSupport)
dayjs.extend(weekday)

export function setupDay(app) {
  app.config.globalProperties.dayjs = dayjs//全局挂载
}