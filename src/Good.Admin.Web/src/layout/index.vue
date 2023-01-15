<template>
	<a-layout class="layout">
		<!-- :style="{ overflow: 'auto', height: '100vh', position: 'fixed', left: 0 }" -->
		<a-layout-sider v-model:collapsed="isCollapsed" :trigger="null" collapsible>
			<sidebar :isCollapsed="isCollapsed" />
		</a-layout-sider>
		<a-layout>
			<PageHeader
				:class="[isCollapsed ? 'lf-80' : 'lf-200']"
				:isCollapsed="isCollapsed"
				@icon-click="handleIconClick"
			/>
			<!-- <app-main :class="[isCollapsed ? 'ml-80' : 'ml-200']"></app-main> -->
			<a-layout-content class="layout-content">
				<MultTabs />
			</a-layout-content>
			<PageFooter />
		</a-layout>
	</a-layout>
</template>

<script>
import { Sidebar, MultTabs, PageHeader, PageFooter } from '@/layout/components'
import { ref } from 'vue'
export default {
	name: 'Layout',
	components: { Sidebar, MultTabs, PageHeader, PageFooter },
	setup() {
		// data
		const isCollapsed = ref(false)

		// methods
		const handleIconClick = () => {
			isCollapsed.value = !isCollapsed.value
		}

		return {
			isCollapsed,
			handleIconClick
		}
	}
}
</script>

<style lang="less" scoped>
.ml-200 {
	margin-left: 200px;
}
.ml-80 {
	margin-left: 80px;
}
.lf-200 {
	left: 200px;
}
.lf-80 {
	left: 80px;
}
.layout-content {
	flex: none;
}
.layout {
	display: flex;
	height: 100vh;
	overflow: hidden;

	.ant-layout {
		overflow: hidden;
	}
}
</style>
