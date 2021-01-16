"use strict";

Vue.directive('mask', VueMask.VueMaskDirective);

const customerBase = { firstName: '', lastName: '', email: '', telephone: '', account: '', password: '', password2: '', image:'', tax_id: '' };
var main = new Vue({
	el: '#register',
	data: {
		data: [],
		customer: customerBase,
		table: null,
		images: [],
		equalPass: false,
	},
	mounted: function () {
		var _this = this;
		this.defineImages();
	},
	computed: {
		registerButton: function () {
			var bool = true;

			for (var c in this.customer) {
				(this.customer[c] == '') ? bool = false : '';
            }

			if (this.customer.password == this.customer.password2) {
				this.equalPass = true;
			} else {
				bool = false;
				this.equalPass = false;
			}

			return bool;
        }
    },
	methods: {
		defineImages: function () {
			for (var i = 0; i < 4; i++) {
				var number = Math.floor(Math.random() * 72);
				var url = '/assets/media/stock-600x400/img-' + number + '.jpg';
				this.images.push(url);
            }
		},
		register: function () {
			var image = this.customer.image.split("/");
			var user = {
				id: "1",
				first_name: this.customer.firstName,
				last_name: this.customer.lastName,
				email: this.customer.email,
				telephone: this.customer.telephone,
				password: this.customer.password,
				image_src: image[image.length - 1],
				user_core_id: this.customer.account,
				tax_id: this.customer.tax_id
			}

			$.post("../User/Register", { json: user }, function (result) {
				Swal.fire('User Registered!', 'You will be redirected to login', 'success').then(function (result_swal) {
					window.location = '/User/Login'
				});
			});
        }
	}
})


