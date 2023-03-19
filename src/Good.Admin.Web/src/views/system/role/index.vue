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
						<a-form-item label="角色名" name="roleName">
							<a-input placeholder="输入角色名" v-model:value="modelRef.roleName" />
						</a-form-item>
					</a-col>
				</a-row>
				<a-row>
					<a-col :span="24" style="text-align: right">
						<a-button type="primary" html-type="submit">搜索</a-button>
						<a-button style="margin: 0 8px" @click="resetFields">重置</a-button>
						<!-- <a style="font-size: 12px" @click="expand = !expand">
							<template v-if="expand">
								<UpOutlined />
							</template>
							<template v-else>
								<DownOutlined />
							</template>
							Collapse
						</a> -->
					</a-col>
				</a-row>
			</a-form>
		</div>
		<div class="vxetablebox">
			<a-row :gutter="24" class="home-main">
				<a-col :span="24">
					<vxe-toolbar>
						<template #buttons>
							<a-button @click="addHandle()" type="primary">
								<template #icon><plus-outlined /></template>新增
							</a-button>
							<a-button type="primary" danger :disabled="deleteVisible" @click="deleteHandle()">
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
						ref="xTable"
						round
						:data="tableDate.listData"
						:row-config="{ keyField: 'Id', isHover: true }"
						:loading="tableDate.loading"
						@checkbox-change="selectChangeEvent"
						@checkbox-all="selectAllChangeEvent"
					>
						<vxe-column type="checkbox" width="60"></vxe-column>
						<!-- <vxe-column field="Id" width="60" title="ID"></vxe-column> -->
						<vxe-column type="seq" width="60"></vxe-column>
						<vxe-column field="RoleName" title="角色名"></vxe-column>
						<vxe-column field="CreateTime" title="创建时间"></vxe-column>
						<vxe-column title="操作" width="200" show-overflow>
							<template #default="{ row }">
								<!-- <vxe-button type="text" icon="vxe-icon-edit" @click="editEvent(row)"></vxe-button> -->
								<!-- <a-button size="middle" @click="edithandle(row)">
									<template #icon></template>
								</a-button> -->
								<div @click="editHandle(row)">
									<edit-outlined style="font-size: medium" />
								</div>
							</template>
						</vxe-column>
					</vxe-table>

					<vxe-pager
						background
						v-model:current-page="tableDate.pagination.currentPage"
						v-model:page-size="tableDate.pagination.pageSize"
						:total="tableDate.pagination.totalResult"
						@page-change="handlePageChange"
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
		<roleListEdit :title="modeltitle" ref="editRef" @submit="saveHandle($event)" />
	</div>
</template>

<script setup>
import roleListEdit from './components/roleListEdit.vue'
import { reactive, createVNode, toRaw, ref, onMounted } from 'vue'
import { Form, message, Modal } from 'ant-design-vue'
import { ExclamationCircleOutlined } from '@ant-design/icons-vue'
import { getRoles, addRole, deleteRole } from '@/api/role'

const useForm = Form.useForm
const xTable = ref()
const editRef = ref(roleListEdit)
const modeltitle = ref('新增角色')
const deleteVisible = ref(true)
const selectId = reactive([])
const modelRef = reactive({
	roleName: '',
	createTime: '',
	remark: ''
})
const tableDate = reactive({
	loading: false,
	listData: [],
	pagination: {
		currentPage: 1,
		pageSize: 10,
		totalResult: 0
	}
})
const searchdata = reactive({
	PageIndex: 1,
	PageSize: 10,
	SortField: '',
	SortType: '',
	Search: {
		roleId: '',
		roleName: modelRef.roleName
	}
})

const { resetFields } = useForm(modelRef)
const refresh = () => {
	getlistData()
}
const addHandle = () => {
	editRef.value.show()
}
const deleteHandle = () => {
	Modal.confirm({
		title: '提醒',
		icon: createVNode(ExclamationCircleOutlined),
		content: '确定要删除所选数据么？',
		okText: '确认删除',
		okType: 'danger',
		cancelText: '取消',
		onOk() {
			deleteRole(selectId.value).then((res) => {
				if (res.code === 200 && res.success) {
					message.success('删除成功')
					deleteVisible.value = true
					getlistData()
				} else {
					message.error(res.msg)
				}
			})
		},
		// eslint-disable-next-line @typescript-eslint/no-empty-function
		onCancel() {}
	})
}
const editHandle = (row) => {
	var editrow = toRaw(row)
	modeltitle.value = '编辑角色'
	editRef.value.show(editrow)
}
const saveHandle = (value) => {
	addRole(value).then((res) => {
		if (res.code === 200 && res.success) {
			editRef.value.close()
			message.success('保存成功')
			getlistData()
		} else {
			message.error(res.msg)
		}
	})
}
const onFinish = (values) => {
	searchdata.Search.roleName = values.roleName
	console.log(searchdata)
	getlistData()
}
const onFinishFailed = (errorInfo) => {
	console.log('Failed:', errorInfo)
}
const getlistData = () => {
	// 获取权限数据
	tableDate.loading = true
	getRoles(searchdata).then((res) => {
		if (res.code === 200) {
			tableDate.listData = res.data
			tableDate.pagination.totalResult = res.total
			tableDate.loading = false
		} else {
			message.error(res.msg)
		}
	})
}
const handlePageChange = ({ currentPage, pageSize }) => {
	tableDate.pagination.currentPage = currentPage
	tableDate.pagination.pageSize = pageSize
	searchdata.PageIndex = currentPage
	searchdata.PageSize = pageSize
	console.log(pageSize)
	console.log(searchdata)
	console.log(tableDate)
	getlistData()
}

const selectChangeEvent = ({ checked }) => {
	deleteVisible.value = false
	const $table = xTable.value
	const records = $table.getCheckboxRecords()
	if (records.length === 0) {
		deleteVisible.value = true
	}
	console.log(checked ? '勾选事件' : '取消事件', records)
	selectId.value = records.map((item) => item.Id)
}
const selectAllChangeEvent = () => {
	deleteVisible.value = false
	const $table = xTable.value
	const records = $table.getCheckboxRecords()
	if (records.length === 0) {
		deleteVisible.value = true
	}
	//获取所有选中的id
	selectId.value = records.map((item) => item.Id)
}

// mounted
onMounted(() => {
	getlistData()
})
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
