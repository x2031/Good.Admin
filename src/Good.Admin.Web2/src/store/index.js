import { createPinia } from 'pinia';
// import { createStore } from 'vuex'
import getters from './getters'

// const modulesFiles = import.meta.globEager('./modules/*.js')

// const modules = {}

// Object.keys(modulesFiles).forEach((key) => {
// 	const moduleName = key.replace(/^\.\/modules\/(.*)\.\w+$/, '$1')
// 	modules[moduleName] = modulesFiles[key].default || {}
// })

// const store = createStore({
// 	modules,
// 	getters
// })


const store = createPinia();

export function setupStore(app) {
  app.use(store);
}

export { store };