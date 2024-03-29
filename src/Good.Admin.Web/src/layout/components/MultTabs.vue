<template>
	<div class="tabs-view">
		<Tabs
			:active-key="activeKey"
			hide-add
			type="editable-card"
			class="tabs"
			@change="changePage"
			@edit="editTabItem"
		>
			<Tabs.TabPane
				v-for="pageItem in tabsList"
				:key="pageItem.fullPath"
				:closable="tabsList.length != 1"
			>
				<template #tab>
					<Dropdown :trigger="['contextmenu']">
						<div style="display: inline-block"> {{ pageItem.meta?.title }} </div>
						<template #overlay>
							<Menu style="user-select: none">
								<Menu.Item key="1" :disabled="activeKey !== pageItem.fullPath" @click="reloadPage">
									<reload-outlined />
									重新加载
								</Menu.Item>
								<Menu.Item key="2" :disabled="tabsList.length == 1" @click="removeTab(pageItem)">
									<close-outlined />
									关闭
								</Menu.Item>
								<Menu.Divider />
								<Menu.Item key="3" :disabled="tabsList.length == 1" @click="closeLeft(pageItem)">
									<vertical-right-outlined />
									关闭左侧
								</Menu.Item>
								<Menu.Item key="4" :disabled="tabsList.length == 1" @click="closeRight(pageItem)">
									<vertical-left-outlined />
									关闭右侧
								</Menu.Item>
								<Menu.Divider />
								<Menu.Item key="5" :disabled="tabsList.length == 1" @click="closeOther(pageItem)">
									<column-width-outlined />
									关闭其他
								</Menu.Item>
								<Menu.Item key="6" :disabled="tabsList.length == 1" @click="closeAll">
									<minus-outlined />
									关闭所有
								</Menu.Item>
							</Menu>
						</template>
					</Dropdown>
				</template>
			</Tabs.TabPane>

			<template #rightExtra>
				<Dropdown :trigger="['click']" placement="bottomRight">
					<a class="ant-dropdown-link" @click.prevent>
						<down-outlined :style="{ fontSize: '20px' }" />
					</a>
					<template #overlay>
						<Menu style="user-select: none">
							<Menu.Item key="1" :disabled="activeKey !== route.fullPath" @click="reloadPage">
								<reload-outlined />
								重新加载
							</Menu.Item>
							<Menu.Item key="2" :disabled="tabsList.length == 1" @click="removeTab(route)">
								<close-outlined />
								关闭当前
							</Menu.Item>
							<Menu.Divider />
							<Menu.Item key="5" :disabled="tabsList.length == 1" @click="closeOther(route)">
								<column-width-outlined />
								关闭其它
							</Menu.Item>
							<Menu.Item key="6" :disabled="tabsList.length == 1" @click="closeAll">
								<minus-outlined />
								关闭所有
							</Menu.Item>
						</Menu>
					</template>
				</Dropdown>
			</template>
		</Tabs>
		<div class="tabs-view-content">
			<router-view v-slot="{ Component }">
				<!-- :name="Object.is(route.meta?.transitionName, false) ? '' : 'fade-transform'" -->
				<template v-if="Component">
					<transition mode="out-in" appear>
						<keep-alive :include="keepAliveComponents">
							<component :is="Component" :key="route.fullPath" />
						</keep-alive>
					</transition>
				</template>
			</router-view>
		</div>
	</div>
</template>

<script setup>
import { computed, unref, watch } from 'vue'
import { Dropdown, Tabs, message, Menu } from 'ant-design-vue'
import { useRoute, useRouter } from 'vue-router'
import { storage } from '@/utils/storage'
import { useTabsViewStore, blackList } from '@/store/tabsView'
import { useKeepAliveStore } from '@/store/keepAlive'
// import { REDIRECT_NAME } from '@/router/constant';

const route = useRoute()
const router = useRouter()
const tabsViewStore = useTabsViewStore()
const keepAliveStore = useKeepAliveStore()
const activeKey = computed(() => tabsViewStore.getCurrentTab?.fullPath)
// 标签页列表
const tabsList = computed(() => tabsViewStore.getTabsList)
// 缓存的路由组件列表
const keepAliveComponents = computed(() => keepAliveStore.list)
// 获取简易的路由对象
const getSimpleRoute = (route) => {
	const { fullPath, hash, meta, name, params, path, query } = route
	return { fullPath, hash, meta, name, params, path, query }
}

let routes = []

try {
	const routesStr = storage.get('TABS_ROUTES')
	routes = routesStr ? JSON.parse(routesStr) : [getSimpleRoute(route)]
} catch (e) {
	routes = [getSimpleRoute(route)]
}

// 初始化标签页
tabsViewStore.initTabs(routes)
watch(
	() => route.fullPath,
	() => {
		if (blackList.some((n) => n === route.name)) return
		// tabsViewMutations.addTabs(getSimpleRoute(route))
		tabsViewStore.addTabs(getSimpleRoute(route))
	},
	{ immediate: true }
)

// 在页面关闭或刷新之前，保存数据
window.addEventListener('beforeunload', () => {
	storage.set('TABS_ROUTES', JSON.stringify(tabsList.value))
})

// 目标路由是否等于当前路由
const isCurrentRoute = (route) => {
	return router.currentRoute.value.matched.some((item) => item.name === route.name)
}

// 关闭当前页面
const removeTab = (route) => {
	if (tabsList.value.length === 1) {
		return message.warning('这已经是最后一页，不能再关闭了！')
	}
	// tabsViewMutations.closeCurrentTabs(route)
	tabsViewStore.closeCurrentTab(route)
}
// tabs 编辑（remove || add）
const editTabItem = (targetKey, action) => {
	if (action == 'remove') {
		removeTab(tabsList.value.find((item) => item.fullPath == targetKey))
	}
}
// 切换页面
const changePage = (key) => {
	Object.is(route.fullPath, key) || router.push(key)
}

// 刷新页面
const reloadPage = () => {
	// router.replace({
	// 	name: 'Redirect',
	// 	params: {
	// 		path: unref(route).fullPath
	// 	}
	// })
	router.replace(unref(route).fullPath)
}

// 关闭左侧
const closeLeft = (route) => {
	// tabsViewMutations.closeLeftTabs(route)
	tabsViewStore.closeLeftTabs(route)
	!isCurrentRoute(route) && router.replace(route.fullPath)
}

// 关闭右侧
const closeRight = (route) => {
	// tabsViewMutations.closeRightTabs(route)
	tabsViewStore.closeRightTabs(route)
	!isCurrentRoute(route) && router.replace(route.fullPath)
}

// 关闭其他
const closeOther = (route) => {
	// tabsViewMutations.closeOtherTabs(route)
	tabsViewStore.closeOtherTabs(route)
	!isCurrentRoute(route) && router.replace(route.fullPath)
}

// 关闭全部
const closeAll = () => {
	localStorage.removeItem('routes')
	// tabsViewMutations.closeAllTabs()
	tabsViewStore.closeAllTabs()
	router.replace('/')
}
</script>

<style lang="less" scoped>
.dark .tabs-view {
	border-top: 1px solid black;
}

.tabs-view {
	border-top: 1px solid #eee;

	:deep(.tabs) {
		.ant-tabs-nav {
			@apply bg-white dark:bg-black;
			padding: 0px 20px 0 10px;
			background-color: white;
			margin: 0;
			user-select: none;
		}

		.ant-tabs-tabpane {
			display: none;
		}

		.ant-tabs-tab-remove {
			display: flex;
			padding: 0;
			margin: 0;

			.anticon-close {
				padding-left: 6px;
			}
		}

		.ant-tabs-tab:not(.ant-tabs-tab-active) {
			.ant-tabs-tab-remove {
				width: 0;
			}

			.anticon-close {
				width: 0;
				visibility: hidden;
				transition: width 0.3s;
			}

			&:hover {
				.anticon-close {
					width: 16px;
					visibility: visible;
					padding-left: 6px;
				}

				.ant-tabs-tab-remove {
					width: unset;
				}
			}
		}
	}
	.tabs-view-content {
		/* height: calc(100vh - #{$header-height}); */
		height: calc(100vh - 110px);
		padding: 10px 14px 0;
		overflow: auto;
	}
}
</style>
