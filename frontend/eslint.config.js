// @ts-check
const eslint = require("@eslint/js");
const tseslint = require("typescript-eslint");
const angular = require("angular-eslint");
const prettier = require("eslint-plugin-prettier");
const prettierConfig = require("eslint-config-prettier");

module.exports = tseslint.config(
	{
		files: ["**/*.ts"],
		ignores: ["**/dist/**", "**/tmp/**", "**/out-tsc/**", "**/bazel-out/**", "**/node_modules/**", "npm-debug.log", "yarn-error.log", "**/.idea/**", "**/.project", "**/.classpath", "**/.c9/**", "**/*.launch", "**/.settings/**", "**/*.sublime-workspace", "**/.history/**", "**/.angular/cache/**", "**/.sass-cache/**", "**/connect.lock", "**/coverage/**", "**/libpeerconnection.log", "**/testem.log", "**/typings/**", "**/.DS_Store", "**/Thumbs.db", "**/.husky/**"],
		extends: [eslint.configs.recommended, ...tseslint.configs.recommended, ...tseslint.configs.stylistic, ...angular.configs.tsRecommended],
		processor: angular.processInlineTemplates,
		plugins: {
			prettier: prettier
		},
		rules: {
			...prettierConfig.rules,
			"no-magic-numbers": "off",
			eqeqeq: ["error", "always"],
			indent: ["error", "tab"],
			"linebreak-style": ["error", "unix"],
			quotes: ["error", "double"],
			semi: ["error", "always"],
			"no-debugger": "warn",
			"no-useless-constructor": "off",
			"no-empty-function": [
				"error",
				{
					allow: ["constructors"]
				}
			],
			"@angular-eslint/component-class-suffix": ["error"],
			"@angular-eslint/component-max-inline-declarations": ["error"],
			"@angular-eslint/component-selector": [
				"error",
				{
					type: "element",
					prefix: "app",
					style: "kebab-case"
				}
			],
			"@angular-eslint/consistent-component-styles": ["error", "array"],
			"@angular-eslint/contextual-decorator": ["error"],
			"@angular-eslint/contextual-lifecycle": ["error"],
			"@angular-eslint/directive-class-suffix": ["error"],
			"@angular-eslint/directive-selector": [
				"error",
				{
					type: "attribute",
					prefix: "app",
					style: "camelCase"
				}
			],
			"@angular-eslint/no-async-lifecycle-method": ["error"],
			"@angular-eslint/no-attribute-decorator": ["error"],
			"@angular-eslint/no-conflicting-lifecycle": ["error"],
			"@angular-eslint/no-duplicates-in-metadata-arrays": ["error"],
			"@angular-eslint/no-empty-lifecycle-method": ["error"],
			"@angular-eslint/no-input-rename": ["error"],
			"@angular-eslint/no-output-native": ["error"],
			"@angular-eslint/no-output-on-prefix": ["off"],
			"@angular-eslint/no-output-rename": ["error"],
			"@angular-eslint/no-pipe-impure": ["error"],
			"@angular-eslint/prefer-output-readonly": ["error"],
			"@angular-eslint/relative-url-prefix": ["error"],
			"@angular-eslint/sort-lifecycle-methods": ["error"],
			"@angular-eslint/use-component-selector": ["error"],
			"@angular-eslint/use-component-view-encapsulation": ["error"],
			"@angular-eslint/use-lifecycle-interface": ["error"],
			"@angular-eslint/use-pipe-transform-interface": ["error"],
			"@typescript-eslint/consistent-type-definitions": ["off"]
		}
	},
	{
		files: ["**/*.html"],
		ignores: ["**/dist/**", "**/tmp/**", "**/out-tsc/**", "**/bazel-out/**", "**/node_modules/**", "npm-debug.log", "yarn-error.log", "**/.idea/**", "**/.project", "**/.classpath", "**/.c9/**", "**/*.launch", "**/.settings/**", "**/*.sublime-workspace", "**/.history/**", "**/.angular/cache/**", "**/.sass-cache/**", "**/connect.lock", "**/coverage/**", "**/libpeerconnection.log", "**/testem.log", "**/typings/**", "**/.DS_Store", "**/Thumbs.db", "**/.husky/**"],
		extends: [...angular.configs.templateRecommended, ...angular.configs.templateAccessibility],
		plugins: {
			prettier: prettier
		},
		rules: {
			...prettierConfig.rules,
			"@angular-eslint/template/alt-text": ["error"],
			"@angular-eslint/template/attributes-order": [
				"error",
				{
					alphabetical: false,
					order: ["STRUCTURAL_DIRECTIVE", "TEMPLATE_REFERENCE", "ATTRIBUTE_BINDING", "INPUT_BINDING", "TWO_WAY_BINDING", "OUTPUT_BINDING"]
				}
			],
			"@angular-eslint/template/banana-in-box": ["error"],
			"@angular-eslint/template/button-has-type": ["error"],
			"@angular-eslint/template/click-events-have-key-events": ["off"],
			"@angular-eslint/template/conditional-complexity": [
				"error",
				{
					maxComplexity: 5
				}
			],
			"@angular-eslint/template/cyclomatic-complexity": [
				"error",
				{
					maxComplexity: 5
				}
			],
			"@angular-eslint/template/elements-content": [
				"error",
				{
					allowList: []
				}
			],
			"@angular-eslint/template/eqeqeq": [
				"error",
				{
					allowNullOrUndefined: true
				}
			],
			"@angular-eslint/template/interactive-supports-focus": [
				"error",
				{
					allowList: []
				}
			],
			"@angular-eslint/template/label-has-associated-control": [
				"error",
				{
					checkIds: true,
					controlComponents: ["input", "meter", "output", "progress", "select", "textarea"],
					labelComponents: [
						{
							inputs: ["for", "htmlFor"],
							selector: "label"
						}
					]
				}
			],
			"@angular-eslint/template/mouse-events-have-key-events": ["off"],
			"@angular-eslint/template/no-any": ["error"],
			"@angular-eslint/template/no-autofocus": ["error"],
			"@angular-eslint/template/no-distracting-elements": ["error"],
			"@angular-eslint/template/no-duplicate-attributes": [
				"error",
				{
					allowTwoWayDataBinding: true,
					ignore: []
				}
			],
			"@angular-eslint/template/no-inline-styles": [
				"error",
				{
					allowNgStyle: false,
					allowBindToStyle: false
				}
			],
			"@angular-eslint/template/no-interpolation-in-attributes": ["error"],
			"@angular-eslint/template/no-negated-async": ["error"],
			"@angular-eslint/template/no-positive-tabindex": ["error"],
			"@angular-eslint/template/prefer-control-flow": ["error"],
			"@angular-eslint/template/prefer-ngsrc": ["off"],
			"@angular-eslint/template/prefer-self-closing-tags": ["error"],
			"@angular-eslint/template/role-has-required-aria": ["error"],
			"@angular-eslint/template/table-scope": ["error"],
			"@angular-eslint/template/use-track-by-function": [
				"error",
				{
					alias: []
				}
			],
			"@angular-eslint/template/valid-aria": ["error"]
		}
	}
);
