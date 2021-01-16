"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

const recipientBase = { recipient_id: null, recipient_acc: null, alias: null, fullname: null };
var main = new Vue({
	el: '#recipient',
	data: {
		user: user,
		session_time: session_time,
		data: [],
		recipient: { recipient_id: null, recipient_acc: null, alias: null, fullname: null },
		table: null,
	},
	mounted: function () {
		var _this = this;

		$('#kt_datatable').LoadingOverlay('show');
		$.get("../Recipient", function (result) {
			_this.data = result;
			_this.initDatatable();
			$('#kt_datatable').LoadingOverlay('hide');
		});
    },
	methods: {
		initDatatable: function () {

			this.table = $('#kt_datatable');

			this.table.DataTable({
				data: this.data,
				dom: 'lBfrtip',
				columns: [
					{ data: 'alias' },
					{ data: 'fullname' },
					{ data: 'recipient_id' },
					{ data: 'recipient_acc' },
					{ data: 'status', responsivePriority: -1 },
				],
				columnDefs: [
					{
						targets: -1,
						title: 'Actions',
						orderable: false,
						render: function (data, type, full, meta) {
							return '<button class="btn btn-sm btn-clean btn-icon" onClick="main.openEditModal(\'' + full.id + '\')" title="Edit details"><i class="la la-edit"></i> </button> <button onClick="main.deleteRecipient(\'' + full.id + '\')" class="btn btn-sm btn-clean btn-icon" title="Delete"><i class="la la-trash"></i></button>'
						},
					},
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
		openAddModal: function () {
			recipientModal.account_correct = false;
			recipientModal.modalTitle = 'Add Recipient';
			recipientModal.recipient = { recipient_id: null, recipient_acc: null, alias: null, fullname: null };
			$('#recipientModal').modal('show');
		},
		openEditModal: function (id) {
			recipientModal.modalTitle = "Edit";
			recipientModal.account_correct = true;
			$.get("../Recipient/getRecipient/?" + $.param({ 'id': id }), function (result) {
				recipientModal.recipient = result;
				$('#recipientModal').modal('show');
			});
        },
		deleteRecipient: function (id) {
			$.ajax({
				url: "../Recipient/?" + $.param({ 'id': id }),
				type: "DELETE",
				success: function (result) {
					console.log(result);
					main.refreshDatatable();
				}
			});
		},
		refreshDatatable: function () {
			$('#kt_datatable').LoadingOverlay('show');
			var _this = this;
			$.get("../Recipient", function (result) {
				_this.data = result;

				_this.table.DataTable().clear().draw();
				_this.table.DataTable().rows.add(result);
				_this.table.DataTable().columns.adjust().draw();
				$('#kt_datatable').LoadingOverlay('hide');
			});
        }
	}
})

var recipientModal = new Vue({
	el: '#recipientModal',
	data: {
		recipient: { recipient_id: null, recipient_acc: null, alias: null, fullname: null },
		modalTitle: '',
		account_correct: false,
		account_nf: false,
		recipient_nf: false,
	},
	watch: {
		
    },
	methods: {
		searchUser: function () {
			var _this = this;
			$.post("../Recipient/getUSer", { tax_id: _this.recipient.recipient_id }, function (result) {
				if (result.id) {
					_this.recipient.fullname = result.first_name + ' ' + result.last_name;
					_this.recipient_nf = false;
				} else {
					_this.recipient_nf = true;
                }
			});
		},
		confirmAccount: function () {
			var _this = this;
			$.post("../Recipient/confirmAccount", { recipient: _this.recipient.recipient_id, account: _this.recipient.recipient_acc }, function (result) {
				if (result.id) {
					_this.account_correct = true;
					_this.account_nf = false;
				} else {
					_this.account_correct = false;
					_this.account_nf = true;
				}
			});
		},
		addRecipient: function () {
			var _this = this;
			$.post("../Recipient", { recipient: _this.recipient, id: user.tax_id }, function (result) {
				console.log(result);
				main.refreshDatatable();
				$('#recipientModal').modal('hide');
			});
		},
		editRecipient: function () {
			var _this = this;
			$.ajax({
				url: "../Recipient/?",
				type: "PUT",
				data: {
					'recipientIn': _this.recipient
                },
				success: function (result) {
					console.log(result);
					main.refreshDatatable();
					$('#recipientModal').modal('hide');
				}
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

