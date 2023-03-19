<template>
	<a-config-provider :locale="zhCN">
		<router-view></router-view>
	</a-config-provider>
</template>

<script>
import enUS from 'ant-design-vue/es/locale/en_US'
import zhCN from 'ant-design-vue/es/locale/zh_CN'
export default {
	name: 'App',
	setup() {
		return { enUS, zhCN }
	}
}
</script>

<style></style>
