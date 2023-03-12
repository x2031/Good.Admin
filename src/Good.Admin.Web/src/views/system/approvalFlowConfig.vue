<template>
	<a-form :label-col="labelCol" :wrapper-col="wrapperCol">
		<a-form-item label="Activity name" v-bind="validateInfos.name">
			<a-input v-model:value="modelRef.name" />
		</a-form-item>
		<a-form-item label="Sub name" v-bind="validateInfos['sub.name']">
			<a-input v-model:value="modelRef.sub.name" />
		</a-form-item>
		<a-form-item :wrapper-col="{ span: 14, offset: 4 }">
			<a-button type="primary" @click="onSubmit">Create</a-button>
			<a-button style="margin-left: 10px" @click="reset">Reset</a-button>
		</a-form-item>
	</a-form>
	<button @click="onSubmit">check</button>
</template>
<script setup>
import { defineComponent, reactive, toRaw } from 'vue'
import { Form } from 'ant-design-vue'

const useForm = Form.useForm

const modelRef = reactive({
	name: '',
	sub: {
		name: ''
	}
})
const rules = reactive({
	name: [
		{
			required: true,
			message: 'Please input name'
		}
	],
	'sub.name': [
		{
			required: true,
			message: 'Please input sub name'
		}
	]
})
const { resetFields, validate, validateInfos } = useForm(modelRef, rules)
const onSubmit = () => {
	validate()
		.then((res) => {
			console.log(res, toRaw(modelRef))
		})
		.catch((err) => {
			console.log('error', err)
		})
}
const reset = () => {
	resetFields()
}
</script>
