angular
	.module('service', [])
	.factory('HelperService', HelperService)
	.factory('JenisService', JenisService)
	.factory('KaryawanService', KaryawanService)
	.factory('PelanggaranService', PelanggaranService)
	.factory('PerusahaanService', PerusahaanService)
	.factory('PointService', PointService);

function JenisService($http, $q) {
	var controller = 'api/level';
	var controllerDetail = 'api/jenispelanggaran';
	var service = {};
	var datas = [];

	service.get = () => {
		var def = $q.defer();

		if (!service.instance) {
			$http({ url: controller, method: 'Get' }).then(
				(response) => {
					datas = response.data;
					service.instance = true;
					def.resolve(datas);
				},
				(err) => {
					def.reject(err.message);
				}
			);
		} else {
			def.resolve(datas);
		}

		return def.promise;
	};

	service.post = (model) => {
		var def = $q.defer();
		$http({ url: controller, method: 'POST', data: model }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.put = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idlevel, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idlevel == model.idlevel);
				if (item) {
					item.level = model.level;
				}
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.delete = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idlevel, method: 'Delete' }).then(
			(response) => {
				var index = datas.indexOf(model);
				datas.splice(index, 1);
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	//deletail
	service.postDetail = (model) => {
		var def = $q.defer();
		$http({ url: controllerDetail, method: 'POST', data: model }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.putDetail = (model) => {
		var def = $q.defer();
		$http({ url: controllerDetail + '/' + model.idjenispelanggaran, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idlevel);
				if (item) {
					var detail = item.datas.find((x) => (x.idjenispelanggaran = model.idjenispelanggaran));
					if (detail) {
						detail.jenispelanggaran = model.jenispelanggaran;
					}
				}
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.deleteDetail = (model) => {
		var def = $q.defer();
		$http({ url: controllerDetail + '/' + model.idjenispelanggaran, method: 'Delete' }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	return service;
}

function PerusahaanService($http, $q) {
	var controller = 'api/perusahaan';
	var service = {};
	var datas = [];

	service.get = () => {
		var def = $q.defer();

		if (!service.instance) {
			$http({ url: controller, method: 'Get' }).then(
				(response) => {
					datas = response.data;
					service.instance = true;
					def.resolve(datas);
				},
				(err) => {
					def.reject(err.message);
				}
			);
		} else {
			def.resolve(datas);
		}

		return def.promise;
	};

	service.post = (model) => {
		var def = $q.defer();
		$http({ url: controller, method: 'POST', data: model }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.put = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idperusahaan, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idperusahaan == model.idperusahaan);
				if (item) {
					item.level = model.level;
				}
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.delete = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idperusahaan, method: 'Delete' }).then(
			(response) => {
				var index = datas.indexOf(model);
				datas.splice(index, 1);
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	return service;
}

function KaryawanService($http, $q) {
	var controller = 'api/karyawan';
	var service = {};
	var datas = [];

	service.get = () => {
		var def = $q.defer();

		if (!service.instance) {
			$http({ url: controller, method: 'Get' }).then(
				(response) => {
					datas = response.data;
					service.instance = true;
					def.resolve(datas);
				},
				(err) => {
					def.reject(err.message);
				}
			);
		} else {
			def.resolve(datas);
		}

		return def.promise;
	};

	service.getById = (id) => {
		var def = $q.defer();
		var data = datas.find((x) => x.idkaryawan == id);
		if (data) {
			def.resolve(data);
		} else {
			$http({ url: controller + '/' + id, method: 'Get' }).then(
				(response) => {
					datas.push(response.data);
					def.resolve(response.data);
				},
				(err) => {
					def.reject(err.message);
				}
			);
		}
		return def.promise;
	};

	service.post = (model) => {
		var def = $q.defer();
		$http({ url: controller, method: 'POST', data: model }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.put = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idkaryawan, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idkaryawan == model.idperusahaan);
				if (item) {
					item.level = model.level;
				}
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.delete = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idkaryawan, method: 'Delete' }).then(
			(response) => {
				var index = datas.indexOf(model);
				datas.splice(index, 1);
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	return service;
}

function PelanggaranService($http, $q) {
	var controller = 'api/pelanggaran';
	var service = {};
	var datas = [];

	service.getById = (id) => {
		var def = $q.defer();
		$http({ url: controller + '/karyawan/' + id, method: 'Get' }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);
		return def.promise;
	};

	service.post = (model) => {
		var def = $q.defer();
		$http({ url: controller, method: 'POST', data: model }).then(
			(response) => {
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.put = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idkaryawan, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idkaryawan == model.idperusahaan);
				if (item) {
					item.level = model.level;
				}
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	service.delete = (model) => {
		var def = $q.defer();
		$http({ url: controller + '/' + model.idkaryawan, method: 'Delete' }).then(
			(response) => {
				var index = datas.indexOf(model);
				datas.splice(index, 1);
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);

		return def.promise;
	};

	return service;
}

function HelperService() {
	var service = {};

	service.bulans = [
		{ name: 'Januari', value: 0 },
		{ name: 'Februari', value: 1 },
		{ name: 'Maret', value: 2 },
		{ name: 'April', value: 3 },
		{ name: 'Mei', value: 4 },
		{ name: 'Juni', value: 5 },
		{ name: 'Juli', value: 6 },
		{ name: 'Agustus', value: 7 },
		{ name: 'September', value: 8 },
		{ name: 'Oktober', value: 9 },
		{ name: 'November', value: 10 },
		{ name: 'Desember', value: 11 }
	];

	service.tahuns = function getTahun() {
		var tahun = new Date().getFullYear();
		var tahuns = [];
		for (let index = tahun; index > tahun - 5; index--) {
			tahuns.push(index);
		}

		return tahuns;
	};

	return service;
}

function PointService() {
	var date = new Date();
	var service = {};
	service.tahun = date.getFullYear();
	service.bulan = date.getMonth();
	service.defaultPoint = 100;

	service.setPelanggaran = (datas) => {
		var items = datas.filter(
			(x) => new Date(x.tanggal).getFullYear() == service.tahun && new Date(x.tanggal).getMonth() == service.bulan
		);
		service.pengurangan = items.reduce(function(a, item) {
			return item.karyawan + a;
		}, 0);
	};

	service.penambahan = () => {
		return date.getDate() * 0.5;
	};

	service.point = () => {
		return service.defaultPoint + service.penambahan();
	};

	service.totalPoint = () => {
		return service.point() - service.pengurangan;
	};

	return service;
}
