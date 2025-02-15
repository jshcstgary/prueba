/** @type {import('tailwindcss').Config} */

const defaultTheme = require("tailwindcss/defaultTheme");

module.exports = {
	content: ["./node_modules/flyonui/dist/js/*.js", "./node_modules/notyf/notyf.min.js", "./src/**/*.{html,ts}"],
	theme: {
		screens: {
			xs: "375px",
			...defaultTheme.screens
		},
		extend: {}
	},
	plugins: [require("flyonui"), require("flyonui/plugin")],
	flyonui: {
		themes: ["dark"],
		vendors: true
	}
};
