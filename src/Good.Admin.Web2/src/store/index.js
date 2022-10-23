import { createPinia } from 'pinia';
import piniaPluginPersist from 'pinia-plugin-persist'

const store = createPinia()

store.use(piniaPluginPersist)

export { store }

// export function setupStore(app) {
//   app.use(pinia);
// }
//export default setupStore;