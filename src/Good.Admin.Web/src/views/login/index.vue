<template>
	<div class="jc-login">
		<div class="container">
			<div class="wrapper">
				<div class="login-logo enter-x-l">
					<p>
						<img alt="Good.Admin" src="../../assets/images/logo.png" />
						<span>Good.Admin</span>
					</p>
				</div>
				<div class="flex-item login-info">
					<div class="info-box">
						<img class="enter-x-r" alt="Good.Admin" src="../../assets/svg/login-box-bg.svg" />
						<h1 class="enter-x-r">Good.Admin后台管理系统</h1>
						<p class="enter-x-r"><span>欢迎使用</span></p>
					</div>
				</div>
				<div class="flex-item login-form">
					<div class="form-box enter-x-l">
						<h1 class="login-title enter-x-l">登录</h1>
						<a-form ref="loginForm" :model="formData" @finish="handleRegister" :rules="rules">
							<a-form-item name="username" class="enter-x-l">
								<a-input v-model:value="formData.username" size="large" placeholder="用户名" />
							</a-form-item>
							<a-form-item name="password" class="enter-x-l">
								<a-input-password
									v-model:value="formData.password"
									size="large"
									placeholder="密码"
								/>
								<!-- @keyup.enter="handleRegister" -->
							</a-form-item>
							<a-form-item class="enter-x-l" name="isCheckd">
								<a-checkbox v-model:checked="formData.isChecked">记住我</a-checkbox>
							</a-form-item>
							<a-form-item class="enter-x-l">
								<a-button
									size="large"
									:loading="loading"
									class="btn-login"
									type="primary"
									html-type="submit"
									>登录</a-button
								>
								<!-- @click="handleRegister" -->
							</a-form-item>
						</a-form>
					</div>
				</div>
			</div>
		</div>
	</div>
</template>
<script>
import { ref, reactive, toRaw, watch } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useUserStore } from '@/store/user'

export default {
	name: 'Login',
	setup() {
		// data
		const userstore = useUserStore()
		const router = useRouter()
		const route = useRoute()
		const loginForm = ref()
		const formData = reactive({
			username: 'admin',
			password: '123456',
			isChecked: false
		})
		const loading = ref(false)
		const redirect = ref(null)
		const otherQuery = reactive({})
		const rules = {
			username: [{ required: true, trigger: 'blur', message: '请输入用户名' }],
			password: [{ min: 6, required: true, trigger: 'blur', message: '密码不能小于6位' }]
		}
		// methods
		const getOtherQuery = (query) => {
			return Object.keys(query).reduce((acc, cur) => {
				if (cur !== 'redirect') {
					acc[cur] = query[cur]
				}
				return acc
			}, {})
		}
		//提交登录信息
		const handleRegister = async (values) => {
			console.log(values)
			loading.value = true
			await userstore
				.userlogin(values)
				.then((res) => {
					loading.value = false
					router.push({ path: redirect.value || '/', query: otherQuery.value })
				})
				.catch((err) => {
					loading.value = false
				})
		}
		// watch
		watch(
			() => route,
			(curRoute) => {
				console.log(curRoute)
				const query = curRoute.query
				console.log(query)
				if (query) {
					redirect.value = query.redirect
					otherQuery.value = getOtherQuery(query)
				}
			},
			{ immediate: true }
		)

		return {
			loginForm,
			loading,
			formData,
			rules,
			handleRegister
		}
	}
}
</script>

<style lang="less" scoped>
@bg-dark: #293146;
.jc-login {
	overflow: hidden;
	height: 100%;
	padding: 0 16px;
	position: relative;
	&::before {
		position: absolute;
		top: 0;
		left: 0;
		width: 100%;
		height: 100%;
		margin-left: -48%;
		background-image: url('../../assets/svg/login-bg.svg');
		background-position: 100%;
		background-repeat: no-repeat;
		background-size: auto 100%;
		content: '';
	}
	.container {
		margin: 0 auto;
		height: 100%;
		box-sizing: border-box;
		.wrapper {
			display: flex;
			height: 100%;
			.login-logo {
				position: absolute;
				top: 8px;
				display: none;
				p {
					color: #fff;
					img {
						width: 30px;
						vertical-align: -7px;
					}
					span {
						font-weight: bold;
						font-size: 16px;
						display: inline-block;
						padding-left: 5px;
					}
				}
			}
			.flex-item {
				width: 50%;
			}
			.login-info {
				position: relative;
				.info-box {
					position: absolute;
					top: 50%;
					color: #fff;
					margin-top: -159px;
					img {
						width: 45%;
						vertical-align: middle;
					}
					h1 {
						margin: 56px 0 20px 0;
						font-weight: normal;
						color: #fff;
					}
				}
			}
			.login-form {
				position: relative;
				.form-box {
					position: absolute;
					top: 50%;
					margin-top: -168px;
					margin-left: 90px;
					width: 400px;
					.login-title {
						margin-bottom: 20px;
					}
					.btn-login {
						width: 100%;
					}
				}
			}
		}
	}
}

@media screen and (max-width: 1199px) {
	.jc-login {
		background-color: @bg-dark;
		&::before {
			background: none;
		}
		.container {
			.wrapper {
				display: block;
				.login-logo {
					display: block;
				}
				.login-info {
					display: none;
				}
				.login-form {
					position: static;
					.form-box {
						left: 50%;
						margin-left: -200px;
						padding-left: 0;
						background: #fff;
						padding: 40px 30px;
						border-radius: 6px;
						.login-title {
							text-align: center;
						}
					}
				}
			}
		}
	}
}

@media screen and (min-width: 1200px) {
	.container {
		max-width: 1200px;
	}
}

@media screen and (min-width: 1500px) {
	.container {
		max-width: 1500px;
	}
}

@media screen and (min-width: 1600px) {
	.container {
		max-width: 1600px;
	}
}

.loop1(@n, @i: 1) when (@i =< @n ) {
	.enter-x-l:nth-child(@{i}) {
		opacity: 0;
		z-index: @i;
		animation: enter-x-l-animation 0.4s ease-in-out 0.3s;
		animation-fill-mode: forwards;
		animation-delay: @i * 0.1s;
	}
	.loop1(@n, (@i + 1));
}

.loop2(@n, @i: 1) when (@i =< @n ) {
	.enter-x-r:nth-child(@{i}) {
		opacity: 0;
		z-index: @i;
		animation: enter-x-r-animation 0.4s ease-in-out 0.3s;
		animation-fill-mode: forwards;
		animation-delay: @i * 0.1s;
	}
	.loop2(@n, (@i + 1));
}

.loop1(5);
.loop2(3);

@keyframes enter-x-r-animation {
	from {
		transform: translateX(0);
	}
	to {
		opacity: 1;
		transform: translateX(50px);
	}
}

@keyframes enter-x-l-animation {
	from {
		transform: translateX(50px);
	}
	to {
		opacity: 1;
		transform: translateX(0);
	}
}
</style>
