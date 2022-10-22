import { createPinia } from 'pinia';
import piniaPluginPersist from 'pinia-plugin-persist'

const pinia = createPinia()
pinia.use(piniaPluginPersist)
export function setupStore(app) {
  app.use(pinia);
}

export default setupStore;