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
			trans_type: 'Self',
			status: '2',
			description: '',
			done_to: user.tax_id,
			done_by: user.tax_id,
			bank: '1Bank',
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
		product_to: function () {
			var _this = this;
			return _this.products.filter(function (p) {
				return p.acc_number == _this.transaction.acc_to;
			})[0];
		},
		account_correct: function () {
			if (this.product_from && this.product_to) {
				if (this.product_from.acc_number != this.product_to.acc_number) {
					return true;
				}
			} else {
				return false;
            }
        }
	},
	methods: {
		confirmTransaction: function () {
			var _this = this;
			$.post("../Transaction/SelfTransaction", { transaction: _this.transaction }, function (result) {
				Swal.fire('Transaction Done!', 'Your transaction will be processed', 'success').then(function (result_swal) {
					window.location = "../Transaction/Self";
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


