<template>
	<a-layout-footer class="page_footer">
		<div class="page_footer_link">
			<div>
				<a href="https://github.com/x2031/" target="_blank">
					Good.Admin Copyright <CopyrightOutlined /> 2022 Created by x2031
				</a>
			</div>
			<a href="https://github.com/x2031/Good.Admin" target="_blank">
				<GithubOutlined />
			</a>
		</div>
	</a-layout-footer>
</template>

<script>
export default {
	name: 'PageFooter',
	setup() {}
}
</script>
<style lang="less">
.page_footer {
	text-align: center;
	a {
		color: rgba(0, 0, 0, 0.45);
		margin-left: 10px;
	}
	.page_footer_link {
		display: flex;
		justify-content: center;
		margin-bottom: 8px;
		a:not(:last-child) {
			margin-right: 40px;
		}
		.copyright {
			color: rgba(0, 0, 0, 0.45);
		}
	}
}
</style>
