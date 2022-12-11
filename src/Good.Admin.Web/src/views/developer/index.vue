<template>
	<div class="dev-container"> </div>
</template>

<script>
import { timeFix } from '@/utils/tool'
import Chart from '@/components/Charts/index.vue'
import { UserOutlined } from '@ant-design/icons-vue'
import { reactive, ref, onMounted } from 'vue'
import userUrl from '@/assets/images/user.png'

export default {
	name: 'Dashboard',
	components: { Chart, UserOutlined },
	setup() {
		// data
		const timeFormat = timeFix()
		const timeToFix = ref(timeFormat)
		const avatarUrl = ref(userUrl)
		const loading = ref(true)

		// mounted
		onMounted(() => {
			setTimeout(() => {
				loading.value = false
			}, 1000)
		})

		return {
			timeToFix,
			avatarUrl,
			loading
		}
	}
}
</script>

<style lang="less" scoped>
.dev-container {
	overflow-x: hidden;
}
</style>
