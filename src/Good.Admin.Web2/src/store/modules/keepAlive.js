import { defineStore } from 'pinia';
import { store } from '@/store';

export const useKeepAliveStore = defineStore({
  id: 'keep-alive',
  state: () => ({
    list: [],
  }),
  actions: {
    add(name) {
      if (typeof name === 'string') {
        !this.list.includes(name) && this.list.push(name);
      } else {
        name.map((v) => {
          v && !this.list.includes(v) && this.list.push(v);
        });
      }
    },
    remove(name) {
      if (typeof name === 'string') {
        this.list = this.list.filter((v) => {
          return v !== name;
        });
      } else {
        this.list = this.list.filter((v) => {
          return !name.includes(v);
        });
      }
    },
    clear() {
      this.list = [];
    },
  },
});

// 在组件setup函数外使用
export function useKeepAliveStoreWithOut() {
  return useKeepAliveStore(store);
}
