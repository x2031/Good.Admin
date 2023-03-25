<template>
	<div>
		<a-modal
			v-model:visible="editvisible"
			:title="title"
			cancelText="取消"
			okText="确定"
			:closable="false"
			@ok="handleOk"
			@cancel="handleCancel"
		>
			<a-form
				autocomplete="off"
				layout="horizontal"
				:label-col="labelCol"
				:wrapper-col="wrapperCol"
				@finish="onEditFinish"
				@finishFailed="onEditFinishFailed"
			>
				<a-form-item label="部门名称" v-bind="validateInfos.Name">
					<a-input placeholder="请输入部门名称" v-model:value="editmodelRef.Name" />
				</a-form-item>
				<a-form-item label="父级部门">
					<a-select
						ref="select"
						placeholder="选择父级部门"
						:allowClear="true"
						v-model:value="editmodelRef.ParentId"
					>
						<template v-for="item in deparmenSelect.value" :key="item.Id">
							<a-select-option :value="item.Id">{{ item.Name }}</a-select-option>
						</template>
					</a-select>
				</a-form-item>
			</a-form>
		</a-modal>
	</div>
</template>

<script setup>
import { reactive, ref, toRaw, onMounted } from 'vue'
import { Form } from 'ant-design-vue'
import { GetDepartmentInfo } from '@/api/department'

const useForm = Form.useForm
const deparmenSelect = reactive([])
const editvisible = ref(false)
const props = defineProps({
	title: String
})

const labelCol = { span: 4, offset: 4 }
const wrapperCol = { span: 12 }

let editmodelRef = reactive({
	Name: '',
	ParentId: undefined,
	Id: '',
	CreateTime: ''
})
const rulesRef = reactive({
	Name: [
		{ required: true, message: '请输入部门名称' },
		{ min: 2, max: 10, message: '长度在 2 到 10 个字符' }
	]
})
const { resetFields, validate, validateInfos } = useForm(editmodelRef, rulesRef)
const onEditFinish = (values) => {
	console.log('Success:', values, editmodelRef)
}
const onEditFinishFailed = (errorInfo) => {
	console.log('Failed:', errorInfo)
}
const handleCancel = () => {
	resetFields()
	editvisible.value = false
}
const handleOk = () => {
	validate().then((res) => {
		console.log(res)
		if (res) {
			console.log('Success:', editmodelRef)
			emit('submit', editmodelRef)
		}
	})
}
const show = (editrow) => {
	resetFields()
	editvisible.value = true
	editmodelRef = Object.assign(editmodelRef, editrow)
	console.log('show:', editmodelRef)
}
const close = () => {
	editvisible.value = false
}
const getDepartment = () => {
	// 获取部门数据
	GetDepartmentInfo({ Id: '' }).then((res) => {
		if (res.code === 200 && res.success) {
			deparmenSelect.value = res.data
		} else {
			message.error(res.msg)
		}
	})
}
//暴露数据和方法
defineExpose({
	editvisible,
	editmodelRef,
	show,
	close
})
const emit = defineEmits(['submit'])

onMounted(() => {
	getDepartment()
})
</script>

<style lang="less" scoped></style>
