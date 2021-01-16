"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

var main = new Vue({
	el: '#kt_content',
	data: {
		products: products,
		user: user,
		session_time: session_time,
		product: product,
		selected_product: '',
		p_first: product,
		image: '',
		passCount: 0,
		transactions: [],
	},
	mounted: function () {
		this.initDatatable();
		if (this.product.acc_number != null) {
			this.selected_product = this.product.id;
			this.getTransactions();
		}
	},
	watch: {
		product: function () {
			if (this.product.acc_number != null) {
				this.selected_product = this.product.id;
				this.getTransactions();
            }
		},
		transactions: function () {
			var _this = this;
			_this.table.DataTable().clear().draw();
			_this.table.DataTable().rows.add(this.transactions);
			_this.table.DataTable().columns.adjust().draw();
        }
    },
	computed: {
    },
	methods: {
		searchProduct: function () {
			var _this = this;
			var request_link;
			(this.p_first.acc_number == null) ? request_link = "../Banking/Consult/getProduct" : request_link = "../Consult/getProduct";
			$('#accountInfo').LoadingOverlay('show');
			$.post(request_link, { product: this.selected_product }, function (result) {
				_this.product = result;
				$('#accountInfo').LoadingOverlay('hide');
			});
        },
		getTransactions: function () {
			var _this = this;
			var request_link;
			(this.p_first.acc_number == null) ? request_link = "../Banking/Consult/getTransactions" : request_link = "../Consult/getTransactions";
			$('#kt_datatable').LoadingOverlay('show');
			$.post(request_link, { product: this.product.acc_number }, function (result) {
				_this.transactions = result;
				$('#kt_datatable').LoadingOverlay('hide');
			});
		},
		initDatatable: function () {
			var _this = this;
			this.table = $('#kt_datatable');

			this.table.DataTable({
				data: this.transactions,
				dom: 'lBfrtip',
				columns: [
					{ data: 'date_done' },
					{
						render: function (data, type, full, meta) {
							if (full.acc_from == _this.product.acc_number) {
								return '<span class="badge badge-danger">Expense</span>'
							} else {
								return '<span class="badge badge-success">Income</span>'
                            }
						}
					},
					{ data: 'acc_from' },
					{ data: 'acc_to' },
					{ data: 'description' },
					{ data: 'amount' },
					{ data: 'bank' },
					{ data: 'trans_type' },
					{ data: 'status' }
					
				],
				buttons: [
					{
						extend: 'excel',
						text: '<i class="la la-file-excel-o"></i> Excel',
						className: "btn btn-light-linkedin font-weight-bolder"
					}
				],
			});
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


