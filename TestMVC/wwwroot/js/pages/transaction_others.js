"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

var main = new Vue({
	el: '#transaction',
	data: {
		user: user,
		session_time: session_time,
		products: products,
		recipients: recipients,
		table: null,
		transaction: {
			acc_from: '',
			acc_to: '',
			trans_type: 'To Others',
			status: '2',
			description: '',
			done_to: '',
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
		selected_recipient: function () {
			var _this = this;
			return _this.recipients.filter(function (r) {
				return r.recipient_acc == _this.transaction.acc_to;
			})[0];
		},
	},
	methods: {
		confirmTransaction: function () {
			var _this = this;
			_this.transaction.done_to = _this.selected_recipient.recipient_id;
			$.post("../Transaction/OtherTransaction", { transaction: _this.transaction }, function (result) {
				Swal.fire('Transaction Done!', 'Your transaction will be processed', 'success').then(function (result_swal) {
					window.location = "../Transaction/Others";
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


