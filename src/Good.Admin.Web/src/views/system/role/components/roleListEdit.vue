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
				<a-form-item label="角色名" v-bind="validateInfos.RoleName">
					<a-input placeholder="请输入角色名" v-model:value="editmodelRef.RoleName" />
				</a-form-item>
				<a-form-item label="角色类型" v-bind="validateInfos.RoleType">
					<a-select v-model:value="editmodelRef.RoleType" placeholder="请选择角色类型">
						<a-select-option value="1">超级管理员</a-select-option>
						<a-select-option value="2">部门管理员</a-select-option>
						<a-select-option value="3">普通用户</a-select-option>
					</a-select>
				</a-form-item>
			</a-form>
		</a-modal>
	</div>
</template>

<script setup>
import { reactive, defineExpose, ref, toRaw, onMounted } from 'vue'
import { Form } from 'ant-design-vue'

const useForm = Form.useForm
const editvisible = ref(false)
const props = defineProps({
	title: String
})

const labelCol = { span: 4, offset: 4 }
const wrapperCol = { span: 12 }

const editmodelRef = reactive({
	RoleName: '',
	RoleType: '',
	Actions: [],
	Id: ''
})
const rulesRef = reactive({
	RoleName: [
		{ required: true, message: '请输入角色名' },
		{ min: 2, max: 10, message: '长度在 2 到 10 个字符' }
	],
	RoleType: [{ required: true, message: '请选择角色类型' }]
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
const show = () => {
	resetFields()
	editvisible.value = true
}
const close = () => {
	editvisible.value = false
}

//暴露数据和方法
defineExpose({
	editvisible,
	editmodelRef,
	show,
	close
})
const emit = defineEmits(['submit'])
</script>

<style lang="less" scoped></style>
