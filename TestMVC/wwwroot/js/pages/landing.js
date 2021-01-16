"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

var main = new Vue({
	el: '#kt_content',
	data: {
		products: products,
		user: user,
		session_time: session_time,
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
		consultAcc: function (acc_number) {
			window.location = 'Consult/' + acc_number;
		},
		logout: function () {
			Swal.fire({
				title: 'Are you sure you want to log out?',
				showCancelButton: true,
				confirmButtonText: 'Yes',
				icon: 'warning',
			}).then((result) => {
				if (result.isConfirmed) {
					$.post("../User/Logout", function (result) {
						window.location = "/";
					});
				}
			})
		}
	}
})


