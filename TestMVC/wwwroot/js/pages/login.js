"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

const customerBase = { firstName: '', lastName: '', email: '', telephone: '', account: '', password: '', password2: '', image:'' };
var main = new Vue({
	el: '#register',
	data: {
		data: [],
		email: '',
		password: '',
		image: '',
		equalPass: false,
		user_pi: false,
		userNf: false,
		passCount: 0,
	},
	mounted: function () {
		var _this = this;
	},
	watch: {

    },
	computed: {
    },
	methods: {
		sendEmail: function () {
			var _this = this;
			$('.card').LoadingOverlay('show');
			$.post("../User/Login/checkUser", { email: this.email }, function (result) {
				if (result == 'not_found') {
					_this.user_pi = false;
					_this.userNf = true;
					$('.card').LoadingOverlay('hide');
				} else {
					_this.image = '/assets/media/stock-600x400/' + result;
					_this.user_pi = true;
					_this.userNf = false;
					$('.card').LoadingOverlay('hide');
                }
			});
        },
		login: function () {
			var _this = this;
			if (this.password != '') {
				$.post("../User/Login/login", { email: this.email, password: this.password }, function (result) {
					if (!result) {
						_this.passCount++;
					} else {
						window.location = "/Banking/Landing";
					}
				});
			} else {
				_this.passCount++;
            }

		},
		back: function () {
			this.user_pi = false;
        }
	}
})


