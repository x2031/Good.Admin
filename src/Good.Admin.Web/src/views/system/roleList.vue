<template>
	<div>
		<div class="searchbox">
			<a-form
				:model="modelRef"
				autocomplete="off"
				@finish="onFinish"
				@finishFailed="onFinishFailed"
			>
				<a-row :gutter="24">
					<!-- v-show="expand || i <= 6"  -->
					<a-col :span="6">
						<a-form-item label="用户名" name="roleName">
							<a-input placeholder="输入用户名" v-model:value="modelRef.roleName" />
						</a-form-item>
					</a-col>
				</a-row>
				<a-row>
					<a-col :span="24" style="text-align: right">
						<a-button type="primary" html-type="submit">搜索</a-button>
						<a-button style="margin: 0 8px" @click="resetFields">重置</a-button>
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
							<a-button @click="refresh()">
								<template #icon><reload-outlined /></template>刷新
							</a-button>
							<a-button>
								<template #icon><file-excel-outlined /></template>导出
							</a-button>
						</template>
					</vxe-toolbar>

					<vxe-table
						round
						:align="allAlign"
						:data="tableDate.roleData"
						:row-config="{ isHover: true }"
						:loading="tableDate.loading"
					>
						<vxe-column type="seq" width="60"></vxe-column>
						<vxe-column field="RoleName" title="数据库类型"></vxe-column>
						<vxe-column field="CreateTime" title="创建时间"></vxe-column>
					</vxe-table>

					<vxe-pager
						background
						v-model:current-page="tableDate.pagination.currentPage"
						v-model:page-size="tableDate.pagination.pageSize"
						:total="tableDate.pagination.totalResult"
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
	</div>
</template>

<script>
import Chart from '@/components/Charts/index.vue'
import { UserOutlined } from '@ant-design/icons-vue'
import { reactive, defineComponent, ref, onMounted } from 'vue'
import { Form } from 'ant-design-vue'
import { getRoles } from '@/api/role'
const useForm = Form.useForm

export default {
	name: 'Dbmanage',

	components: { Chart, UserOutlined },
	setup() {
		const loading = ref(true)
		const allAlign = ref(null)
		const modelRef = reactive({
			roleName: '',
			createTime: '',
			remark: ''
		})
		const tableDate = reactive({
			loading: false,
			roleData: [],
			pagination: {
				currentPage: 1,
				pageSize: 20,
				totalResult: 0
			}
		})
		const rulesRef = reactive({})
		const { resetFields, validate, validateInfos, mergeValidateInfo } = useForm(modelRef, rulesRef)
		const searchdata = reactive({
			PageIndex: tableDate.pagination.currentPage,
			PageSize: tableDate.pagination.pageSize,
			SortField: '',
			SortType: '',
			Search: {
				roleId: '',
				roleName: modelRef.roleName
			}
		})

		const getRoleData = (searchdata) => {
			// 获取权限数据
			tableDate.loading = true
			getRoles(searchdata).then((res) => {
				if (res.code === 200) {
					tableDate.roleData = res.data
					tableDate.pagination.totalResult = res.total
					tableDate.loading = false
				}
			})
		}
		const refresh = () => {
			getRoleData(searchdata)
		}
		const onFinish = (values) => {
			console.log(modelRef)
			console.log('Success:', values)
			searchdata.Search.roleName = values.roleName
			console.log(searchdata)
			getRoleData(searchdata)
		}
		const onFinishFailed = (errorInfo) => {
			console.log('Failed:', errorInfo)
		}
		// mounted
		onMounted(() => {
			getRoleData(searchdata)
		})

		return {
			tableDate,
			loading,
			allAlign,
			validateInfos,
			modelRef,
			searchdata,
			refresh,
			getRoleData,
			resetFields,
			onFinish,
			onFinishFailed
		}
	}
}
</script>

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
	padding: 10px 24px 16px;
	background: #fff;
}

.searchbox {
	margin-bottom: 20px;
	padding: 24px 24px 2px;
	background: #fff;
}
</style>
