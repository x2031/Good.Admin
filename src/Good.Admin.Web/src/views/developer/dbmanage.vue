<template>
	<div class="dashboard-container">
		<div class="searchbox">
			<a-form
				:model="formState"
				name="basic"
				autocomplete="off"
				@finish="onFinish"
				@finishFailed="onFinishFailed"
			>
				<a-row :gutter="24">
					<a-col :span="6">
						<a-form-item name="dbtype" label="数据库类型">
							<a-select v-model:value="formState.dbtype" placeholder="数据库类型">
								<a-select-option value="mysql">mysql</a-select-option>
								<a-select-option value="sqlserver">sqlserver</a-select-option>
								<a-select-option value="oracle">oracle</a-select-option>
							</a-select>
						</a-form-item>
					</a-col>
					<!-- v-show="expand || i <= 6"  -->
					<a-col :span="6">
						<a-form-item label="IP" name="IP" :rules="[{ required: true, message: '输入IP' }]">
							<a-input placeholder="输入IP" v-model:value="formState.username" />
						</a-form-item>
					</a-col>
				</a-row>
				<a-row>
					<a-col :span="24" style="text-align: right">
						<a-button type="primary" html-type="submit">搜索</a-button>
						<a-button style="margin: 0 8px" @click="() => formRef.resetFields()">重置</a-button>
						<a style="font-size: 12px" @click="expand = !expand">
							<template v-if="expand">
								<UpOutlined />
							</template>
							<template v-else>
								<DownOutlined />
							</template>
							Collapse
						</a>
					</a-col>
				</a-row>
			</a-form>
		</div>
		<div class="vxetablebox">
			<a-row :gutter="24" class="home-main">
				<a-col :span="24">
					<vxe-toolbar>
						<template #buttons>
							<a-button @click="allAlign = 'right'" type="primary">
								<template #icon><plus-outlined /></template>新增
							</a-button>

							<a-button type="primary" danger>
								<template #icon><delete-outlined /></template>删除
							</a-button>
							<a-button>
								<template #icon><reload-outlined /></template>刷新
							</a-button>
							<a-button>
								<template #icon><file-excel-outlined /></template>导出
							</a-button>

							<!-- <a-button @click="allAlign = 'left'" type="primary">居左</a-button>
							<a-button @click="allAlign = 'center'" type="primary">居中</a-button>
							<a-button @click="allAlign = 'right'" type="primary">居右</a-button> -->
						</template>
					</vxe-toolbar>

					<vxe-table round :align="allAlign" :data="tableData1" :row-config="{ isHover: true }">
						<vxe-column type="seq" width="60"></vxe-column>
						<vxe-column field="name" title="Name"></vxe-column>
						<vxe-column field="sex" title="Sex"></vxe-column>
						<vxe-column field="age" title="Age"></vxe-column>
					</vxe-table>

					<vxe-pager
						background
						v-model:current-page="page5.currentPage"
						v-model:page-size="page5.pageSize"
						:total="page5.totalResult"
						:layouts="[
							'PrevJump',
							'PrevPage',
							'JumpNumber',
							'NextPage',
							'NextJump',
							'Sizes',
							'FullJump',
							'Total'
						]"
					>
					</vxe-pager>
				</a-col>
			</a-row>
		</div>
		<a-row :gutter="20" class="home-main">
			<a-col :span="16" class="home-left">
				<a-card class="project-box enter-y" title="进行中的项目" :loading="loading">
					<template #extra>
						<a href="#">更多</a>
					</template>
					<a-card-grid class="project-item" v-for="(item, index) in projectData" :key="index">
						<a-card :bordered="false">
							<a-card-meta :title="item.title">
								<template #description>{{ item.content }}</template>
							</a-card-meta>
							<div class="card-footer">
								<div>{{ item.userName }}</div>
								<div>{{ item.date }}</div>
							</div>
						</a-card>
					</a-card-grid>
				</a-card>
				<a-card class="pending-box enter-y" title="待办任务" :loading="loading">
					<template #extra>
						<a href="#">更多</a>
					</template>
					<a-list item-layout="horizontal" :data-source="pendingData">
						<template #renderItem="{ item }">
							<a-list-item>
								<a-list-item-meta :description="item.content">
									<template #title>
										<a href="#">{{ item.title }}</a>
									</template>
									<template #avatar>
										<a-avatar
											src="https://gw.alipayobjects.com/zos/rmsportal/BiazfanxmamNRoxxVxka.png"
										/>
									</template>
								</a-list-item-meta>
							</a-list-item>
						</template>
					</a-list>
				</a-card>
			</a-col>
			<a-col :span="8" class="home-right">
				<a-card class="chart-box enter-y" title="活动指数" :loading="loading">
					<div>
						<Chart />
					</div>
				</a-card>
				<a-card class="black-box enter-y" title="黑名单" :loading="loading">
					<div>
						<ul>
							<li>上海虎扑体育有限公司</li>
							<li>北京百度科技有限公司</li>
							<li>深圳腾讯科技有限公司</li>
						</ul>
					</div>
				</a-card>
			</a-col>
		</a-row>
	</div>
</template>

<script>
import { timeFix } from '@/utils/tool'
import Chart from '@/components/Charts/index.vue'
import { UserOutlined } from '@ant-design/icons-vue'
import { reactive, defineComponent, ref, onMounted } from 'vue'
import userUrl from '@/assets/images/user.png'

export default {
	name: 'Dbmanage',
	components: { Chart, UserOutlined },
	setup() {
		const timeFormat = timeFix()
		const timeToFix = ref(timeFormat)
		const avatarUrl = ref(userUrl)
		const loading = ref(true)
		const projectData = reactive([
			{ title: '阿里', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' },
			{ title: '百度', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' },
			{ title: '腾讯', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' },
			{ title: '携程', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' },
			{ title: '美团', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' },
			{ title: '饿了么', content: 'this is a test', userName: 'Jason Chen', date: '2021-07-07' }
		])
		const pendingData = reactive([
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' },
			{ title: '项目申请', content: 'this is a test' }
		])
		const allAlign = ref(null)
		const tableData1 = ref([
			{ id: 10001, name: 'Test1', role: 'Develop', sex: 'Man', age: 28, address: 'test abc' },
			{ id: 10002, name: 'Test2', role: 'Test', sex: 'Women', age: 22, address: 'Guangzhou' },
			{ id: 10003, name: 'Test3', role: 'PM', sex: 'Man', age: 32, address: 'Shanghai' },
			{ id: 10004, name: 'Test4', role: 'Designer', sex: 'Women', age: 24, address: 'Shanghai' }
		])
		const formState = reactive({
			username: '',
			password: '',
			remember: true,
			dbtype: ''
		})
		const page5 = reactive({
			currentPage: 1,
			pageSize: 10,
			totalResult: 300
		})
		const onFinish = (values) => {
			console.log('Success:', values)
		}

		const onFinishFailed = (errorInfo) => {
			console.log('Failed:', errorInfo)
		}
		// mounted
		onMounted(() => {
			setTimeout(() => {
				loading.value = false
			}, 1000)
		})

		return {
			timeToFix,
			avatarUrl,
			loading,
			projectData,
			pendingData,
			allAlign,
			tableData1,
			page5,
			formState,
			onFinish,
			onFinishFailed
		}
	}
}
</script>

<style lang="less" scoped>
.dashboard-container {
	overflow-x: hidden;
	.home-main {
		display: flex;

		.home-left {
			.project-box {
				.project-item {
					cursor: pointer;

					.card-footer {
						display: flex;
						justify-content: space-between;
						color: rgba(0, 0, 0, 0.45);
						margin-top: 8px;
					}
				}
			}

			.pending-box {
				margin-top: 14px;
			}
		}

		.home-right {
			.chart-box {
				margin-bottom: 20px;
			}

			.black-box {
				ul {
					padding: 0 14px;

					li {
						color: rgba(0, 0, 0, 0.65);
						font-size: 14px;
						padding: 12px 0;
					}
				}
			}
		}
	}
}
</style>

<style lang="less">
.loopX(@n, @i: 1) when (@i =< @n ) {
	.enter-x:nth-child(@{i}) {
		opacity: 0;
		z-index: @i;
		animation: enter-x-animation 0.4s ease-in-out 0.3s;
		animation-fill-mode: forwards;
		animation-delay: @i * 0.1s;
	}

	.loopX(@n, (@i + 1));
}

.loopY(@n, @i: 1) when (@i =< @n ) {
	.enter-y:nth-child(@{i}) {
		opacity: 0;
		z-index: @i;
		animation: enter-y-animation 0.4s ease-in-out 0.3s;
		animation-fill-mode: forwards;
		animation-delay: @i * 0.1s;
	}

	.loopY(@n, (@i + 1));
}

.loopX(1);
.loopY(4);

@keyframes enter-x-animation {
	from {
		transform: translateX(0);
	}

	to {
		opacity: 1;
		transform: translateX(0);
	}
}

@keyframes enter-y-animation {
	from {
		transform: translateY(50px);
	}

	to {
		opacity: 1;
		transform: translateY(0);
	}
}

.vxetablebox {
	overflow-x: hidden;
	overflow-y: hidden;
	margin-bottom: 20px;
	padding: 24px 24px 16px;
	background: #fff;
}

.searchbox {
	margin-bottom: 20px;
	padding: 24px 24px 16px;
	background: #fff;
}
</style>
