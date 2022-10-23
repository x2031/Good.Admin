import { defineStore } from 'pinia';

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
})