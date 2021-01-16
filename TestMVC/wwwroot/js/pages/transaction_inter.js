"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

var main = new Vue({
	el: '#transaction',
	data: {
		user: user,
		session_time: session_time,
		products: products,
		table: null,
		transaction: {
			acc_from: '',
			acc_to: '',
			trans_type: 'Interbanking',
			status: '2',
			description: '',
			done_to: '',
			done_by: user.tax_id,
			bank: '',
			amount: ''
		}
	},
	mounted: function () {
		var _this = this;
	},
	computed: {
		product_from: function () {
			var _this = this;
			return _this.products.filter(function (p) {
				return p.acc_number == _this.transaction.acc_from;
			})[0];
		},
	},
	methods: {
		confirmTransaction: function () {
			var _this = this;
			$.post("../Transaction/InterbankingTransaction", { transaction: _this.transaction }, function (result) {
				Swal.fire('Transaction Done!', 'Your transaction will be processed', 'success').then(function (result_swal) {
					window.location = "../Transaction/Interbanking";
				});
			});
		},
		openConfirmModal: function () {
			$('#confirmModal').modal('show');
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


