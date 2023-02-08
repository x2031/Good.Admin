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
					<a-col :span="6">
						<a-form-item name="dbtype" label="数据库类型">
							<a-select v-model:value="modelRef.dbtype" placeholder="Select a person">
								<a-select-option value="mysql">mysql</a-select-option>
								<a-select-option value="sqlserver">sqlserver</a-select-option>
								<a-select-option value="oracle">oracle</a-select-option>
							</a-select>
						</a-form-item>
					</a-col>
					<!-- v-show="expand || i <= 6"  -->
					<a-col :span="6">
						<a-form-item label="IP" name="IP">
							<a-input placeholder="输入IP" v-model:value="modelRef.ip" />
						</a-form-item>
					</a-col>
					<a-col :span="6">
						<a-form-item label="备注" name="remark">
							<a-input placeholder="输入备注" v-model:value="modelRef.remark" />
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
						<vxe-column field="dbtype" title="数据库类型"></vxe-column>
						<vxe-column field="ip" title="ip"></vxe-column>
						<vxe-column field="remark" title="备注"></vxe-column>
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
	</div>
</template>

<script>
import Chart from '@/components/Charts/index.vue'
import { UserOutlined } from '@ant-design/icons-vue'
import { reactive, defineComponent, ref, onMounted } from 'vue'
import { Form } from 'ant-design-vue'
const useForm = Form.useForm

export default {
	name: 'Dbmanage',
	components: { Chart, UserOutlined },
	setup() {
		const loading = ref(true)
		const allAlign = ref(null)
		const modelRef = reactive({
			remark: '',
			ip: '',
			dbtype: ''
		})
		const rulesRef = reactive({})
		const { resetFields, validate, validateInfos, mergeValidateInfo } = useForm(modelRef, rulesRef)
		const tableData1 = ref([
			{ id: 10001, dbtype: 'mysql', ip: '192.168.1.1', remark: '' },
			{ id: 10002, dbtype: 'mysql', ip: '192.168.1.1', remark: '' },
			{ id: 10003, dbtype: 'mysql', ip: '192.168.1.1', remark: '' },
			{ id: 10004, dbtype: 'mysql', ip: '192.168.1.1', remark: '' }
		])

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
			loading,
			allAlign,
			tableData1,
			page5,
			validateInfos,
			modelRef,
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
