const Toast = Swal.mixin({
	toast: true,
	position: 'top-end',
	showConfirmButton: false,
	timer: 4000,
	timerProgressBar: true,
	onOpen: (toast) => {
		toast.addEventListener('mouseenter', Swal.stopTimer);
		toast.addEventListener('mouseleave', Swal.resumeTimer);
	}
});

angular
	.module('admin', [ 'service', 'ui.router', 'datatables', 'files', 'ngLocale' ])
	.directive('dateInput', function() {
		return {
			restrict: 'A',
			require: 'ngModel',
			link: textDateLink
		};
		function textDateLink(scope, element, attributes, ngModel) {
			// Simple date regex to accept YYYY/MM/DD formatted dates.
			var dateTestRegex = /\d{4}\/\d{1,2}\/\d{1,2}/;
			ngModel.$parsers.push(parser);
			ngModel.$formatters.push(formatter);
			function parser(value) {
				if (!isNaN(Date.parse(value))) {
					value = new Date(value);
				}

				return value;
			}
			function formatter(value) {
				var formatted = '';
				if (value && !angular.isDate(value)) {
					value = new Date(value);
				}
				return value;
			}
		}
	})
	.directive('numberInput', function() {
		return {
			restrict: 'A',
			require: 'ngModel',
			link: textNumberLink
		};
		function textNumberLink(scope, element, attributes, ngModel) {
			ngModel.$formatters.push(formatter);
			ngModel.$parsers.push(parser);
			function parser(value) {
				var retValue = value;
				if (value !== null) {
					if (value.length > 0) {
						if (!isNaN(value)) {
							retValue = parseInt(str);
						}
					}
				}
				return retValue;
			}
			function formatter(value) {
				!isNaN(value);
				{
					value = parseInt(value);
				}

				return value;
			}
		}
	})
	.directive('tooltip', function() {
		return {
			restrict: 'A',
			link: function(scope, element, attrs) {
				element.hover(
					function() {
						// on mouseenter
						element.tooltip('show');
					},
					function() {
						// on mouseleave
						element.tooltip('hide');
					}
				);
			}
		};
	})
	.config(function($urlRouterProvider, $stateProvider) {
		$urlRouterProvider.otherwise('/home');
		$stateProvider
			.state({
				name: 'home',
				url: '/home',
				controller: 'homeController',
				templateUrl: './views/home.html'
			})
			.state({
				name: 'jenis',
				url: '/jenis',
				controller: 'jenisController',
				templateUrl: './views/jenis.html'
			})
			.state({
				name: 'periode',
				url: '/periode',
				controller: 'periodeController',
				templateUrl: './views/periode.html'
			})
			.state({
				name: 'karyawan',
				url: '/karyawan',
				controller: 'karyawanController',
				templateUrl: './views/karyawan.html'
			})
			.state({
				name: 'karyawan-detail',
				url: '/karyawan/:id',
				controller: 'detailKaryawanController',
				templateUrl: './views/karyawan-detail.html'
			})
			.state({
				name: 'perusahaan',
				url: '/perusahaan',
				controller: 'perusahaanController',
				templateUrl: './views/perusahaan.html'
			})
			.state({
				name: 'perusahaan-detail',
				url: '/perusahaan/:id',
				controller: 'detailPerusahaanController',
				templateUrl: './views/perusahaan-detail.html'
			})
			.state({
				name: 'pelanggaran-baru',
				url: '/pelanggaran-baru',
				controller: 'pelanggaranBaru',
				templateUrl: './views/pelanggaran-baru.html'
			})
			.state({
				name: 'pelanggaran',
				url: '/pelanggaran',
				controller: 'pelanggaranController',
				templateUrl: './views/pelanggaran.html'
			})
			.state({
				name: 'qrcodegen',
				url: '/qrcodegen',
				controller: 'qrcodegenController',
				templateUrl: './views/qrcodegen.html'
			})
			.state({
				name: 'absen',
				url: '/absen',
				controller: 'absenController',
				templateUrl: './views/absen.html'
			});
	})
	.controller('homeController', homeController)
	.controller('periodeController', periodeController)
	.controller('pelanggaranController', pelanggaranController)
	.controller('pelanggaranBaru', pelanggaranBaru)
	.controller('perusahaanController', perusahaanController)
	.controller('detailPerusahaanController', detailPerusahaanController)
	.controller('karyawanController', karyawanController)
	.controller('detailKaryawanController', detailKaryawanController)
	.controller('qrcodegenController', qrcodegenController)
	.controller('jenisController', jenisController)
	.controller('absenController', absenController);

function homeController($scope, UserService) {
	UserService.profile().then((x) => {
		$scope.profile = x;
		setTimeout(() => {
			$('#avatar').attr('src', '/images/src/noimage.png');
			if (x.karyawan && x.karyawan.photo) {
				$('#avatar').attr('src', '/images/profiles/' + x.karyawan.photo);
			}
			$('#userName').text(x.userName);
			$('#role').text(x.roles[0]);
			$.LoadingOverlay('hide');
		}, 200);
	});
}

function periodeController($scope, PeriodeService, DTOptionsBuilder) {
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');
	var e = document.getElementById('close-left');

	PeriodeService.get().then((data) => {
		$scope.datas = data;
		$.LoadingOverlay('hide');
	});

	$scope.save = (model) => {
		if (!model.idperiode) {
			PeriodeService.post(model).then((x) => {
				Toast.fire(
					{
						icon: 'success',
						title: 'Data Berhasil Ditambah !'
					},
					(err) => {
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Ditambah !'
						});
					}
				);
			});
		} else {
			PeriodeService.put(model).then(
				(x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil diubah !'
					});
				},
				(err) => {
					Toast.fire({
						icon: 'error',
						title: 'Data Tidak Berhasil Diubah !'
					});
				}
			);
		}
	};

	$scope.select = (model) => {
		$scope.model = model;
		$scope.addItem = true;
	};

	$scope.closeAddItem = () => {
		$scope.addItem = false;
	};

	$scope.newItem = () => {
		$scope.addItem = true;
		$scope.model = { photo: 'noimage.png' };
	};

	$scope.delete = (item) => {
		PeriodeService.delete(item.idperiode).then((x) => {
			Toast.fire({
				icon: 'success',
				title: 'Data Berhasil Diubah !'
			});
		});
	};
}

function pelanggaranBaru($scope, KaryawanService, JenisService, PelanggaranService, DTOptionsBuilder) {
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');
	var dropbox = document.getElementById('imgPelanggaran');
	var btndropbox = document.getElementById('btnAddFile');

	setFilesContainer(dropbox);
	setFilesContainer(btndropbox);
	$.LoadingOverlay('hide');
	function setFilesContainer(e) {
		e.addEventListener('click', (x) => {
			$('#fileInput').click();
		});
		e.addEventListener('dragenter', dragenter, false);
		e.addEventListener('dragover', dragover, false);
		e.addEventListener('drop', drop, false);
	}

	function drop(e) {
		e.stopPropagation();
		e.preventDefault();
		const dt = e.dataTransfer;
		const files = dt.files;
		$scope.proccessFile(files);
	}
	function dragenter(e) {
		e.stopPropagation();
		e.preventDefault();
	}

	function dragover(e) {
		e.stopPropagation();
		e.preventDefault();
	}

	KaryawanService.get().then((x) => {
		$scope.datas = x;
		var datas = [];
		JenisService.get().then((jenis) => {
			jenis.forEach((element) => {
				var parent = { id: element.id, text: element.nama, children: [] };
				datas.push(parent);
				element.datas.forEach((item) => {
					parent.children.push({
						id: item.id,
						detailLevelId: item.id,
						text: item.nama,
						selected: false,
						parentId: parent.id,
						data: item
					});
				});
			});
			$scope.datajenis = datas;
		});
	});

	$scope.selectUser = (data) => {
		if (!data.userSelect) {
			$('#pelanggaran-item' + data.id).select2({
				data: $scope.datajenis,
				templateSelection: function(data, container) {
					// Add custom attributes to the <option> tag for the selected option
					$(data.element).attr(
						'data-custom-attribute',
						JSON.stringify({
							detailLevelId: data.id,
							nama: data.data.nama,
							levelId: data.parentId,
							nilaiKaryawan: data.data.nilaiKaryawan,
							nilaiPerusahaan: data.data.nilaiPerusahaan,
							penambahan: data.data.penambahan
						})
					);
					return data.text;
				}
			});
		} else {
			$scope.datas.forEach((x) => {
				if (x.userSelect && x.id != data.id) {
					x.userSelect = false;
				}
			});
		}

		data.userSelect = true;
	};

	$scope.cancelUser = (data) => {
		setTimeout((x) => {
			$scope.$apply((x) => {
				data.userSelect = false;
			});
		}, 300);
	};

	$scope.save = (item, files) => {
		var pelanggaran = {
			id: 0,
			tanggalKejadian: new Date(),
			terlaporId: item.id,
			tanggal: new Date(),
			deskripsi: null,
			status: false,
			itemPelanggarans: [],
			files: angular.copy(files),
			perusahaanId: item.perusahaan.id
		};

		var items = $('#pelanggaran-item' + item.id).find(':selected');

		$.each(items, (x, y) => {
			var data = $(y).data('custom-attribute');
			pelanggaran.itemPelanggarans.push(data);
		});

		PelanggaranService.post(pelanggaran).then((x) => {
			Toast.fire({
				icon: 'success',
				title: 'Data Berhasil Ditambah !'
			});
			$scope.files = [];
			item.files = null;
			item.selectedJenis = null;
			item.jenis = null;
			item.userSelect = false;
			var video = document.getElementById('myVideo');
			video.style.display = 'none';
			dropbox.style.backgroundImage = 'url()';
		});
	};

	$scope.selectImage = (item) => {
		if (item.filetype.includes('image')) {
			dropbox.style.backgroundImage = 'url(' + 'data:' + item.filetype + ';base64,' + item.data + ')';
			var video = document.getElementById('myVideo');
			video.style.display = 'none';
			if (!video.paused) {
				video.pause();
			}
		} else {
			var video = document.getElementById('myVideo');
			video.src = 'data:' + item.filetype + ';base64,' + item.data;
			dropbox.style.backgroundImage = 'url()';
			video.style.display = 'block';
		}
	};

	$scope.getFiles = (files) => {
		$scope.proccessFile(files);
	};

	$scope.removeFile = (file) => {
		var index = $scope.files.indexOf(file);
		$scope.files.splice(index, 1);
		dropbox.style.backgroundImage = 'url()';
	};

	$scope.proccessFile = (files) => {
		$scope.files = [];
		Array.from(files).forEach((element, index) => {
			var reader = new FileReader();
			reader.onload = function(e) {
				var data = { filename: '', data: null, filetype: element.type };
				if (element.type.includes('video')) {
					//	var src = thumbnail; ///video url not youtube or vimeo,just video on server
					var video = document.createElement('video');

					video.width = 360;
					video.height = 240;

					var canvas = document.createElement('canvas');
					canvas.width = 360;
					canvas.height = 240;
					var context = canvas.getContext('2d');

					video.addEventListener('loadeddata', function(xx) {
						setTimeout(() => {
							context.drawImage(video, 0, 0, canvas.width, canvas.height);
							data.thumb = canvas.toDataURL('image/jpeg');
							data.data = e.target.result.split(',')[1];
							if (index == 0) {
								dropbox.style.backgroundImage = 'url(' + data.source + ')';
							}
							$scope.$apply((x) => {
								$scope.files.push(data);
							});
						}, 1000);
					});
					video.src = e.target.result;
				} else {
					data.data = e.target.result.split(',')[1];
					data.thumb = e.target.result;
					if (index == 0) {
						if (!element.type.includes('video')) {
							dropbox.style.backgroundImage = 'url(' + e.target.result + ')';
						}
					}
					$scope.$apply((x) => {
						$scope.files.push(data);
					});
				}
			};

			reader.readAsDataURL(element);
		});
	};
}

function jenisController($scope, JenisService, UserService, DTOptionsBuilder) {
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');

	// "bPaginate": false,
	// "bLengthChange": false,
	// "bFilter": true,
	// "bInfo": false,

	UserService.profile().then((x) => {
		$scope.profile = x;
		JenisService.get().then((x) => {
			$scope.items = x;
			$scope.createNew();
		});
	});

	$scope.createNew = () => {
		var emptyExists = $scope.items.find((x) => x.isNew);
		if (!emptyExists)
			$scope.items.push({
				idlevel: 0,
				nama: '',
				isNew: true
			});
	};

	$scope.createNewDetail = () => {
		if ($scope.selected) {
			var emptyExists = $scope.items.find((x) => x.isNew);
		}
		if (!emptyExists)
			$scope.items.push({
				nama: '',
				nilaiKaryawan: 0,
				nilaiPerusahaan: 0,
				penambahan: 0,
				isNew: true
			});
	};

	$scope.selectItem = (item) => {
		var selectExists = $scope.items.find((x) => x.select);
		if (selectExists) {
			selectExists.select = false;
		}
		$scope.selected = item;
		$scope.selected.select = true;
		if ($scope.selected) {
		}
	};

	$scope.saveJenis = (model) => {
		if (!model.proccess) {
			model.proccess = true;
			if (!model.id) {
				JenisService.post(model).then(
					(x) => {
						model.proccess = false;
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Ditambah !'
						});
						model.isNew = false;
						model.id = x.id;
						$scope.selected = model;
						$scope.selected.selected = true;
						$scope.createNew();
					},
					(e) => {}
				);
			} else {
				JenisService.put(model).then(
					(x) => {
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Diubah !'
						});
						model.proccess = false;
						model.isNew = false;
					},
					(e) => {}
				);
			}
		}
	};

	$scope.deleteJenis = (item) => {
		Swal.fire({
			title: 'Yakin ?',
			text: 'Hapus Data !',
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Yes'
		}).then((result) => {
			if (result.value) {
				JenisService.delete(item).then(
					(x) => {
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Di Hapus !'
						});
					},
					(e) => {}
				);
			}
		});
	};

	$scope.addDetail = () => {
		if ($scope.selected) {
			if (!$scope.selected.datas) {
				$scope.selected.datas = [];
			}
			var emptyExist = $scope.selected.datas.find((x) => x.isNew);
			if (!emptyExist) {
				$scope.selected.datas.push({ nama: '', point: 0, deskripsi: '', isNew: true });
			}
		}
	};

	$scope.saveDetail = (model) => {
		if (!model.proccess) {
			model.proccess = true;
			if (!model.id) {
				model.levelId = $scope.selected.id;
				JenisService.postDetail(model).then(
					(x) => {
						model.proccess = false;
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Ditambah !'
						});

						model.id = x.id;
						model.isNew = false;
					},
					(e) => {}
				);
			} else {
				JenisService.putDetail(model).then(
					(x) => {
						var detail = $scope.selected.datas.find((x) => x.id == model.id);
						if (detail) {
							detail.nama = model.nama;
							detail.nilaiKaryawan = model.nilaiKaryawan;
							detail.nilaiPerusahaan = model.nilaiPerusahaan;
							detail.penambahan = model.penambahan;
						}
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Diubah !'
						});
						model.proccess = false;
						model.isNew = false;
					},
					(e) => {}
				);
			}
		}
	};

	$scope.deleteDetail = (item) => {
		Swal.fire({
			title: 'Are you sure?',
			text: "You won't be able to revert this!",
			icon: 'warning',
			showCancelButton: true,
			confirmButtonColor: '#3085d6',
			cancelButtonColor: '#d33',
			confirmButtonText: 'Yes, delete it!'
		}).then((result) => {
			if (result.value) {
				JenisService.deleteDetail(item).then(
					(x) => {
						var index = $scope.selected.datas.indexOf(item);
						$scope.selected.datas.splice(index, 1);
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Diubah !'
						});
					},
					(e) => {}
				);
			}
		});
	};
}

function perusahaanController($scope, PerusahaanService, HelperService, DTOptionsBuilder) {
	$scope.helper = HelperService;
	$scope.isBusy = false;
	$scope.addItem = false;
	$scope.datas = [];
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');

	$('#photo').on('click', (x) => {
		$('#fileInput').click();
	});
	PerusahaanService.get().then((x) => {
		var time = 0;
		x.forEach((m) => {
			setTimeout(() => {
				$scope.$apply(() => {
					$scope.datas.push(m);
				});
			}, (time += 200));
		});

		$scope.addItem = false;
		$.LoadingOverlay('hide');
	});

	$scope.save = (model) => {
		if (!$scope.isBusy) {
			$scope.isBusy = true;
			if (!model.id) {
				PerusahaanService.post(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.newItem();
				});
				$scope.isBusy = false;
			} else {
				PerusahaanService.put(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});

					$scope.newItem();
					$scope.isBusy = false;
				});
			}
		}
	};

	$scope.select = (item) => {
		$scope.model = item;
		if (!item.logo) {
			item.logo = 'noimage.png';
		}

		$scope.addItem = true;
	};

	$scope.newItem = () => {
		$scope.addItem = true;
		$scope.model = { logo: 'noimage.png' };
	};

	$scope.getPhoto = (files, model) => {
		Array.from(files).forEach((element, index) => {
			var reader = new FileReader();
			reader.onload = function(e) {
				model.dataPhoto = e.target.result.split(',')[1];
				//model.logo = e.target.result;
				var photo = document.getElementById('photo');
				photo.src = e.target.result;
			};

			reader.readAsDataURL(element);
		});
	};
}

function karyawanController($scope, PerusahaanService, KaryawanService, DTOptionsBuilder) {
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');
	$scope.isBusy = false;
	$scope.addItem = false;
	$scope.model = { photo: 'noimage.png', Perusahaan: {} };

	$('#photo').on('click', (x) => {
		$('#fileInput').click();
	});

	PerusahaanService.get().then((dataperusahaan) => {
		$scope.perusahaan = dataperusahaan;
		KaryawanService.get().then((karyawans) => {
			$scope.datas = karyawans;
			$.LoadingOverlay('hide');
		});
	});

	$scope.save = (model) => {
		if (!$scope.isBusy) {
			$scope.isBusy = true;

			if (!model.id) {
				KaryawanService.post(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.newItem();
				});
				$scope.isBusy = false;
			} else {
				KaryawanService.put(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.newItem();
					$scope.isBusy = false;
				});
			}
		}
	};

	$scope.selectItem = (model) => {
		$scope.model = model;

		if (!model.photo) {
			model.photo = 'noimage.png';
		}

		$scope.addItem = true;
	};

	$scope.newItem = () => {
		$scope.addItem = true;
		$scope.model = { photo: 'noimage.png', perusahaanKaryawan: {} };
	};

	$scope.getPhoto = (files, model) => {
		Array.from(files).forEach((element, index) => {
			var reader = new FileReader();
			reader.onload = function(e) {
				model.dataPhoto = e.target.result.split(',')[1];
				model.photo = e.target.result;
				var photo = document.getElementById('photo');
				photo.src = e.target.result;
			};

			reader.readAsDataURL(element);
		});
	};
}

function detailPerusahaanController(
	$scope,
	$stateParams,
	KaryawanService,
	PerusahaanService,
	HelperService,
	PointService,
	UserService,
	PeriodeService,
	DTOptionsBuilder,
	DTColumnDefBuilder
) {
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');
	$scope.helper = HelperService;

	PerusahaanService.getById($stateParams.id).then((x) => {
		$scope.model = x;
		PeriodeService.active().then((active) => {
			//var total = PointService.point($scope.model, active);

			$.LoadingOverlay('hide');
		});
	});

	$scope.myFilter = function(month, year) {
		if (month && year) {
			return function(event) {
				var bulan = new Date(event.tanggal).getMonth();
				var tahun = new Date(event.tanggal).getFullYear();
				return bulan == month.value && tahun == year ? true : false;
			};
		} else {
			return function(event) {
				return false;
			};
		}
	};

	$scope.onChangeRole = (model, role) => {
		role.idKaryawan = model.idKaryawan;
		KaryawanService.setRole(role).then(
			(x) => {},
			(err) => {
				role.checked = !role.checked;
				Toast.fire({
					icon: 'success',
					title: 'Data Berhasil Diubah !'
				});
			}
		);
	};

	$scope.selectBukti = (files) => {
		$scope.selectedFiles = files;
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');
		picture.src = '';
		video.src = '';
		picture.style.display = 'none';
		video.style.display = 'none';
	};

	$scope.selectFile = (file) => {
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');

		if (file.fileType.includes('image')) {
			picture.src = '/bukti/images/' + file.fileName;
			video.src = '';
			picture.style.display = 'block';
			video.style.display = 'none';
		} else {
			video.src = '/bukti/videos/' + file.fileName;
			picture.style.display = 'none';
			video.style.display = 'block';
			picture.src = '';
		}
	};
}

function detailKaryawanController(
	$scope,
	$stateParams,
	KaryawanService,
	PelanggaranService,
	HelperService,
	PointService,
	UserService,
	PeriodeService,
	DTOptionsBuilder
) {
	$scope.helper = HelperService;
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');

	UserService.profile().then((profile) => {
		$scope.profile = profile;
		KaryawanService.getById($stateParams.id).then((x) => {
			$scope.model = x;
			$scope.roles = [];
			$scope.helper.roles.forEach((element) => {
				var role = element;
				role.checked = false;
				if (x.roles.find((x) => x == element.role.toLowerCase())) {
					role.checked = true;
				}

				PeriodeService.active().then((active) => {
					var ctx = document.getElementById('chartPoint');
					const context = ctx.getContext('2d');
					context.clearRect(0, 0, ctx.width, ctx.height);
					var total = Math.round(PointService.point($scope.model, active)).toFixed(2);

					// write text plugin

					var myPieChart = new Chart(ctx, {
						type: 'doughnut',
						data: {
							datasets: [
								{
									data: [ total, $scope.model.pengurangan ],
									backgroundColor: [ '#84c125', '#d9241b' ]
								}
							],

							// These labels appear in the legend and in the tooltips when hovering different arcs
							labels: [ 'Point', 'Pengurangan' ]
						},
						options: {
							//cutoutPercentage: 40,
							responsive: false,
							legend: {
								display: false
							}
						}
					});
					Chart.pluginService.clear();
					Chart.pluginService.register({
						beforeDraw: function(chart) {
							var width = chart.chart.width,
								height = chart.chart.height,
								ctx = chart.chart.ctx;
							// ctx.fillText(());

							ctx.restore();
							var fontSize = (height / 114).toFixed(2);
							ctx.font = fontSize + 'em sans-serif';
							ctx.fillStyle = 'white';
							ctx.textBaseline = 'middle';

							var text = total,
								textX = Math.round((width - ctx.measureText(text).width) / 2),
								textY = height / 2;

							ctx.fillText(text, textX, textY);
							ctx.save();
						}
					});
				});
			});
			$.LoadingOverlay('hide');
		});
	});
	$scope.pelanggarans = [];
	$scope.setpelanggaran = (month, year) => {
		if (month && month.value == -1 && $scope.active) {
			$scope.pelanggarans = $scope.model.pelanggarans.filter(
				(x) =>
					new Date(x.tanggal) >= new Date($scope.active.mulai) &&
					new Date(x.tanggal) <= new Date($scope.active.selesai)
			);
		} else if (month && year) {
			$scope.pelanggarans = $scope.model.pelanggarans.filter(
				(x) => new Date(x.tanggal).getMonth() == month.value && new Date(x.tanggal).getFullYear() == year
			);
		}
	};

	$scope.myFilter = function(month, year) {
		if (month && year) {
			return function(event) {
				var bulan = new Date(event.tanggal).getMonth();
				var tahun = new Date(event.tanggal).getFullYear();
				return bulan == month.value && tahun == year ? true : false;
			};
		} else {
			return function(event) {
				return false;
			};
		}
	};

	$scope.onChangeRole = (model, role) => {
		role.idKaryawan = model.id;
		KaryawanService.setRole(role).then(
			(x) => {},
			(err) => {
				role.checked = !role.checked;
				Toast.fire({
					icon: 'success',
					title: 'Data Berhasil Diubah !'
				});
			}
		);
	};

	$scope.showdetail = 'Bukti';
	$scope.selectBukti = (data, item) => {
		$scope.selected = data;
		$scope.selectedFiles = data.files;
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');
		picture.src = '';
		video.src = '';
		picture.style.display = 'none';
		video.style.display = 'none';
		$scope.showdetail = item;
	};

	$scope.selectFile = (file) => {
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');

		if (file.fileType.includes('image')) {
			picture.src = '/bukti/images/' + file.fileName;
			video.src = '';
			picture.style.display = 'block';
			video.style.display = 'none';
		} else {
			video.src = '/bukti/videos/' + file.fileName;
			picture.style.display = 'none';
			video.style.display = 'block';
			picture.src = '';
		}
	};
}

function pelanggaranController(
	$scope,
	PelanggaranService,
	HelperService,
	UserService,
	PeriodeService,
	DTOptionsBuilder
) {
	$scope.pelanggarans = [];
	var vm = this;
	vm.dtOptions = DTOptionsBuilder.newOptions()
		.withPaginationType('full_numbers')
		.withDisplayLength(10)
		.withDOM('pitrfl');

	$scope.helper = HelperService;

	$scope.bulans = [];

	PeriodeService.active().then((active) => {
		$scope.active = active;
		PelanggaranService.get().then((datas) => {
			$scope.datas = datas;
			var bulan = { value: -1, name: 'Periode Active' };
			$scope.bulans.push(bulan);
			HelperService.bulans.forEach((element) => {
				$scope.bulans.push(element);
			});
			UserService.profile().then((x) => {
				$scope.profile = x;
				$.LoadingOverlay('hide');
			});
		});
	});

	$scope.setpelanggaran = (month, year) => {
		if (month && month.value == -1 && $scope.active) {
			$scope.pelanggarans = $scope.datas.filter(
				(x) =>
					new Date(x.tanggal) >= new Date($scope.active.mulai) &&
					new Date(x.tanggal) <= new Date($scope.active.selesai)
			);
		} else if (month && year) {
			$scope.pelanggarans = $scope.datas.filter(
				(x) => new Date(x.tanggal).getMonth() == month.value && new Date(x.tanggal).getFullYear() == year
			);
		}
	};

	$scope.myFilter = function(month, year) {
		if (month && month.value == -1 && $scope.active) {
			return function(event) {
				var tgl = new Date(event.tanggal);
				var result = tgl >= new Date($scope.active.mulai) && tgl <= new Date($scope.active.selesai);
				return tgl >= new Date($scope.active.mulai) && tgl <= new Date($scope.active.selesai);
			};
		} else if (month && year) {
			return function(event) {
				var bulan = new Date(event.tanggal).getMonth();
				var tahun = new Date(event.tanggal).getFullYear();
				return bulan == month.value && tahun == year ? true : false;
			};
		} else {
			return function(event) {
				return '';
			};
		}
	};
	$scope.showdetail = 'Bukti';
	$scope.selectBukti = (data, item) => {
		$scope.selected = data;
		$scope.selectedFiles = data.files;
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');
		picture.src = '';
		video.src = '';
		picture.style.display = 'none';
		video.style.display = 'none';
		$scope.showdetail = item;
	};

	$scope.selectFile = (file) => {
		var picture = document.getElementById('picture');
		var video = document.getElementById('myvideo');

		if (file.fileType.includes('image')) {
			picture.src = '/bukti/images/' + file.fileName;
			video.src = '';
			picture.style.display = 'block';
			video.style.display = 'none';
		} else {
			video.src = '/bukti/videos/' + file.fileName;
			picture.style.display = 'none';
			video.style.display = 'block';
			picture.src = '';
		}
	};

	$scope.updateStatus = (data, status) => {
		PelanggaranService.updateStatus(data.idpelanggaran, status).then(
			(x) => {
				data.status = x;
				$('#bukti').modal('hide');
			},
			(err) => {}
		);
	};
}

function qrcodegenController($scope) {
	$.LoadingOverlay('hide');
	$scope.model = {};
	var qrcode;
	$scope.generate = () => {
		if ($scope.model.mulai && $scope.model.selesai) {
			if (qrcode) {
				qrcode.clear();
			}
			qrcode = new QRCode(document.getElementById('qrcode'), {
				width: 600,
				height: 600,

				title: 'QRCode Absen',
				titleFont: 'bold 16px Arial',
				titleColor: '#000000',
				titleBackgroundColor: '#62a000',
				titleHeight: 70,
				titleTop: 25,
				text: JSON.stringify($scope.model),
				logo: '/images/src/logo2.png',
				logoWidth: undefined,
				logoHeight: undefined,
				logoBackgroundColor: 'lightgray',
				logoBackgroundTransparent: false
			});
		}
	};
	$scope.print = () => {
		var mode = 'iframe'; //popup
		var close = mode == 'popup';
		var options = { mode: mode, popClose: close };
		$('div.printableArea').printArea(options);
	};
}
function absenController($scope, PeriodeService, HelperService, AbsenService) {
	$scope.datas = [];
	$scope.datasAbsen = [];
	$scope.model = {};
	$scope.helper = HelperService;
	$scope.bulans = [];
	PeriodeService.active().then((active) => {
		$scope.active = active;
		AbsenService.get().then((x) => {
			$scope.datas = x;
			var bulan = { value: -1, name: 'Periode Active' };
			$scope.bulans.push(bulan);
			HelperService.bulans.forEach((element) => {
				$scope.bulans.push(element);
			});
			$scope.datasAbsen = $scope.datas.filter((x) => new Date().getDate() == new Date(x.masuk));
			$.LoadingOverlay('hide');
		});
	});

	$scope.setAbsen = (month, year) => {
		if (month && month.value == -1 && $scope.active) {
			$scope.datasAbsen = $scope.datas.filter(
				(x) =>
					new Date(x.masuk) <= new Date($scope.active.mulai) &&
					new Date(x.masuk) >= new Date($scope.active.selesai)
			);
		} else if (month && year) {
			$scope.datasAbsen = $scope.datas.filter(
				(x) => new Date(x.masuk).getMonth() == month.value && new Date(x.masuk).getFullYear() == year
			);
		}
	};

	$scope.print = () => {
		var mode = 'iframe'; //popup
		var close = mode == 'popup';
		var options = { mode: mode, popClose: close };
		$('div.printableArea').printArea(options);
	};
}
