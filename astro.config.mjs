// @ts-check
import { defineConfig } from 'astro/config';
import starlight from '@astrojs/starlight';
import starlightSidebarTopics from 'starlight-sidebar-topics';
import starlightLinksValidator from 'starlight-links-validator';

// docs.ohrtech.net: the OhrTech SDK's two parts, each with its own sidebar. The Management API is
// called from customers' servers and tools; the Device SDK (coming) from apps on the device.
export default defineConfig({
	site: 'https://docs.ohrtech.net',
	trailingSlash: 'always',
	integrations: [
		starlight({
			title: 'OhrTech Developers',
			description:
				'Build on OhrTech devices: the Management API for your servers and tools, and the Device SDK for your apps.',
			logo: { src: './src/assets/logo-inverse.svg', alt: 'OhrTech' },
			favicon: '/favicon.svg',
			head: [
				{ tag: 'link', attrs: { rel: 'icon', href: '/favicon.png', type: 'image/png' } },
				{ tag: 'link', attrs: { rel: 'apple-touch-icon', href: '/apple-touch-icon.png' } },
			],
			social: [{ icon: 'github', label: 'GitHub', href: 'https://github.com/OhrTech' }],
			editLink: { baseUrl: 'https://github.com/OhrTech/docs/edit/main/' },
			lastUpdated: true,
			customCss: [
				'@fontsource-variable/source-sans-3',
				'@fontsource/jetbrains-mono/400.css',
				'@fontsource/jetbrains-mono/600.css',
				'./src/styles/theme.css',
			],
			components: {
				SiteTitle: './src/components/SiteTitle.astro',
				Hero: './src/components/Hero.astro',
			},
			expressiveCode: {
				themes: ['github-light', 'github-dark'],
				styleOverrides: {
					borderRadius: '6px',
					codeFontFamily: "'JetBrains Mono', ui-monospace, 'SFMono-Regular', Menlo, Consolas, monospace",
					codeFontSize: '0.875rem',
					uiFontFamily: "'Source Sans 3 Variable', 'Source Sans 3', system-ui, sans-serif",
				},
			},
			plugins: [
				starlightLinksValidator({ errorOnLocalLinks: false, exclude: ['/management-api/reference/', '/management-api/reference/**'] }),
				starlightSidebarTopics(
					[
						{
							label: 'Management API',
							link: '/management-api/',
							icon: 'laptop',
							id: 'management-api',
							items: [
								{
									label: 'Start here',
									items: [
										{ label: 'Overview', slug: 'management-api' },
										'management-api/quickstart',
										'management-api/sign-in',
										'management-api/permissions',
									],
								},
								{
									label: 'Guides',
									items: [
										'management-api/guides/devices-and-fleets',
										'management-api/guides/settings',
										'management-api/guides/apps',
										'management-api/guides/messages-and-tasks',
										'management-api/guides/remote-control',
										'management-api/guides/embed-remote-control',
										'management-api/guides/shell-commands',
										'management-api/guides/remote-adb',
										'management-api/guides/errors',
									],
								},
								{
									label: 'Recipes',
									items: [
										'management-api/recipes/help-desk-connect',
										'management-api/recipes/release-from-ci',
										'management-api/recipes/sort-into-fleets',
										'management-api/recipes/fleet-health-alerts',
										'management-api/recipes/compliance-report',
										'management-api/recipes/broadcast-message',
									],
								},
								{
									label: 'SDKs and tools',
									items: ['management-api/sdks/dotnet', 'management-api/cli', 'management-api/sdks/other-languages'],
								},
								{
									label: 'Reference',
									items: [
										{ label: 'API reference', link: '/management-api/reference/', attrs: { 'data-reference': '' } },
										'management-api/changelog',
									],
								},
							],
						},
						{
							label: 'Device SDK',
							link: '/device-sdk/',
							icon: 'phone',
							id: 'device-sdk',
							badge: { text: 'Coming soon', variant: 'note' },
							items: [{ label: 'Overview', slug: 'device-sdk' }],
						},
					],
					{ exclude: ['/'] },
				),
			],
		}),
	],
});
