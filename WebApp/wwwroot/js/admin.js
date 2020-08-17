const Toast = Swal.mixin({
	toast: true,
	position: 'top-end',
	showConfirmButton: false,
	timer: 2000,
	timerProgressBar: true,
	onOpen: (toast) => {
		toast.addEventListener('mouseenter', Swal.stopTimer);
		toast.addEventListener('mouseleave', Swal.resumeTimer);
	}
});

angular
	.module('admin', [ 'service', 'ui.router', 'files' ])
	.config(function($urlRouterProvider, $stateProvider) {
		$urlRouterProvider.otherwise('/home');
		$stateProvider
			.state({
				name: 'home',
				url: '/home',
				template: 'Home'
			})
			.state({
				name: 'jenis',
				url: '/jenis',
				controller: 'jenisController',
				templateUrl: './views/jenis.html'
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
				name: 'pelanggaran-baru',
				url: '/pelanggaran-baru',
				controller: 'pelanggaranBaru',
				templateUrl: './views/pelanggaran-baru.html'
			});
	})
	.controller('pelanggaranBaru', pelanggaranBaru)
	.controller('perusahaanController', perusahaanController)
	.controller('karyawanController', karyawanController)
	.controller('detailKaryawanController', detailKaryawanController)
	.controller('jenisController', jenisController);

function pelanggaranBaru($scope, KaryawanService, JenisService, PelanggaranService) {
	var dropbox = document.getElementById('imgPelanggaran');
	dropbox.addEventListener('dragenter', dragenter, false);
	dropbox.addEventListener('dragover', dragover, false);
	dropbox.addEventListener('drop', drop, false);

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

	dropbox.addEventListener('click', (x) => {
		$('#fileInput').click();
	});

	KaryawanService.get().then((x) => {
		$scope.datas = x;
		JenisService.get().then((jenis) => {
			$scope.datajenis = jenis;
		});
	});

	$scope.selectUser = (data) => {
		data.userSelect = true;
	};

	$scope.save = (item, files) => {
		item.idjenispelanggaran = item.jenis.idjenispelanggaran;
		item.files = angular.copy(files);
		item.karyawan = item.jenis.pengurangankaryawan;
		item.perusahaan = item.jenis.penguranganperusahaan;
		PelanggaranService.post(item).then((x) => {
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

function jenisController($scope, JenisService) {
	JenisService.get().then((x) => {
		$scope.items = x;

		$scope.createNew();
	});

	$scope.createNew = () => {
		var emptyExists = $scope.items.find((x) => x.isNew);
		if (!emptyExists)
			$scope.items.push({
				idlevel: 0,
				level: '',
				isNew: true
			});
	};

	$scope.createNewDetail = () => {
		if ($scope.selected) var emptyExists = $scope.items.find((x) => x.isNew);
		if (!emptyExists)
			$scope.items.push({
				nama: '',
				pengurangankaryawan: 0,
				penguranganperusahaan: 0,
				penambahanpoint: 0,
				isNew: true
			});
	};

	$scope.selectItem = (item) => {
		var selectExists = $scope.items.find((x) => x.select);
		if (selectExists) selectExists.select = false;
		$scope.selected = item;
		$scope.selected.select = true;
	};

	$scope.saveJenis = (model) => {
		if (!model.proccess) {
			model.proccess = true;
			if (!model.idlevel) {
				JenisService.post(model).then(
					(x) => {
						model.proccess = false;
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Ditambah !'
						});
						model.isNew = false;
						model.idlevel = x.idlevel;
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
			var emptyExist = $scope.selected.datas.find((x) => x.isNew);
			if (!emptyExist) $scope.selected.datas.push({ nama: '', point: 0, deskripsi: '', isNew: true });
		}
	};

	$scope.saveDetail = (model) => {
		if (!model.proccess) {
			model.proccess = true;
			if (!model.idjenispelanggaran) {
				model.idlevel = $scope.selected.idlevel;
				JenisService.postDetail(model).then(
					(x) => {
						model.proccess = false;
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Ditambah !'
						});

						model.idjenispelanggaran = x.idjenispelanggaran;
						model.isNew = false;
					},
					(e) => {}
				);
			} else {
				JenisService.putDetail(model).then(
					(x) => {
						Toast.fire({
							icon: 'success',
							title: 'Data Berhasil Diubah !'
						});
						model.idjenispelanggaran = x.idjenispelanggaran;
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

function perusahaanController($scope, PerusahaanService) {
	$scope.isBusy = false;
	$scope.addItem = false;
	PerusahaanService.get().then((x) => {
		$scope.datas = x;
	});

	$scope.save = (model) => {
		if (!$scope.isBusy) {
			$scope.isBusy = true;
			if (!model.idperusahaan) {
				PerusahaanService.post(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.model = {};
				});
				$scope.isBusy = false;
			} else {
				PerusahaanService.put(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.model = {};
					$scope.isBusy = false;
				});
			}
		}
	};

	$scope.select = (item) => {
		$scope.model = item;
	};
}

function karyawanController($scope, PerusahaanService, KaryawanService) {
	$scope.isBusy = false;
	$scope.addItem = false;
	PerusahaanService.get().then((dataperusahaan) => {
		$scope.perusahaan = dataperusahaan;
		KaryawanService.get().then((karyawans) => {
			$scope.datas = karyawans;
		});
	});

	$scope.save = (model) => {
		if (!$scope.isBusy) {
			$scope.isBusy = true;

			model.idperusahaan = model.perusahaan.idperusahaan;
			if (!model.idkaryawan) {
				KaryawanService.post(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.model = {};
				});
				$scope.isBusy = false;
			} else {
				KaryawanService.put(model).then((x) => {
					Toast.fire({
						icon: 'success',
						title: 'Data Berhasil Diubah !'
					});
					$scope.model = {};
					$scope.isBusy = false;
				});
			}
		}
	};

	$scope.selectItem = (model) => {
		$scope.model = model;
	};

	$scope.newItem = () => {
		$scope.addItem = true;
		$scope.model = {};
	};
}

function detailKaryawanController(
	$scope,
	$stateParams,
	KaryawanService,
	PelanggaranService,
	HelperService,
	PointService
) {
	$scope.helper = HelperService;
	KaryawanService.getById($stateParams.id).then((x) => {
		$scope.model = x;
		PelanggaranService.getById(x.idkaryawan).then((pelanggaran) => {
			$scope.pelanggarans = pelanggaran;
			PointService.setPelanggaran(pelanggaran);

			// write text plugin
			Chart.pluginService.register({
				beforeDraw: function(chart) {
					var width = chart.chart.width,
						height = chart.chart.height,
						ctx = chart.chart.ctx;

					ctx.restore();
					var fontSize = (height / 114).toFixed(2);
					ctx.font = fontSize + 'em sans-serif';
					ctx.fillStyle = 'white';
					ctx.textBaseline = 'middle';

					var text = PointService.point(),
						textX = Math.round((width - ctx.measureText(text).width) / 2),
						textY = height / 2;

					ctx.fillText(text, textX, textY);
					ctx.save();
				}
			});
			var ctx = document.getElementById('chartPoint');
			var myPieChart = new Chart(ctx, {
				type: 'doughnut',
				data: {
					datasets: [
						{
							data: [ PointService.point(), PointService.pengurangan ],
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
}
