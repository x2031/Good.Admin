<template>
	<div>
		<a-modal
			v-model:visible="editvisible"
			:title="title"
			cancelText="取消"
			okText="保存"
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
				<a-form-item label="用户名" v-bind="validateInfos.UserName">
					<a-input placeholder="请输入用户名" v-model:value="editmodelRef.UserName" />
				</a-form-item>
				<a-form-item label="姓名" v-bind="validateInfos.RealName">
					<a-input placeholder="请输入姓名" v-model:value="editmodelRef.RealName" />
				</a-form-item>
				<a-form-item label="密码" v-bind="validateInfos.newPwd">
					<a-input-password placeholder="请输入密码" v-model:value="editmodelRef.newPwd" />
				</a-form-item>
				<a-form-item label="性别">
					<a-radio-group v-model:value="editmodelRef.Sex">
						<a-radio :value="0">女</a-radio>
						<a-radio :value="1">男</a-radio>
					</a-radio-group>
				</a-form-item>
				<a-form-item label="生日" name="Birthday">
					<a-date-picker v-model:value="editmodelRef.Birthday" value-format="YYYY-MM-DD" />
				</a-form-item>
				<a-form-item label="部门" name="DepartmentId" v-bind="validateInfos.DepartmentId">
					<a-select
						ref="select"
						placeholder="选择部门"
						:allowClear="true"
						v-model:value="editmodelRef.DepartmentId"
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
const editvisible = ref(false)
const props = defineProps({
	title: String
})

const deparmenSelect = reactive({})
const labelCol = { span: 4, offset: 4 }
const wrapperCol = { span: 12 }

let editmodelRef = reactive({
	newPwd: '',
	RoleIdList: [],
	Id: '',
	UserName: '',
	RealName: '',
	Sex: 0,
	CreateTime: '',
	Birthday: '',
	DepartmentId: undefined
})
const rulesRef = reactive({
	UserName: [
		{ required: true, message: '请输入用户名' },
		{ min: 2, max: 10, message: '长度在 2 到 50 个字符' }
	],
	RealName: [
		{ required: true, message: '请输入姓名' },
		{ max: 50, message: '长度不能超过50 个字符' }
	],
	DepartmentId: [{ required: true, message: '请选择部门' }],
	newPwd: [{ min: 6, max: 20, message: '长度在 6 到 20 个字符' }]
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
		}
	})
}
onMounted(() => {
	getDepartment()
})
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
