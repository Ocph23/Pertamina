angular
	.module('service', [])
	.factory('HelperService', HelperService)
	.factory('JenisService', JenisService)
	.factory('KaryawanService', KaryawanService)
	.factory('PelanggaranService', PelanggaranService)
	.factory('PerusahaanService', PerusahaanService)
	.factory('UserService', UserService)
	.factory('PeriodeService', PeriodeService)
	.factory('PointService', PointService);

function JenisService($http, $q) {
	var controller = 'api/level';
	var controllerDetail = 'api/detaillevel';
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
		$http({ url: controller + '/' + model.id, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.id == model.id);
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
		$http({ url: controller + '/' + model.id, method: 'Delete' }).then(
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
		$http({ url: controllerDetail + '/' + model.id, method: 'PUT', data: model }).then(
			(response) => {
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
		$http({ url: controllerDetail + '/' + model.id, method: 'Delete' }).then(
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

	service.getById = (id) => {
		var def = $q.defer();
		$http({ url: controller + '/' + id, method: 'Get' }).then(
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
				datas.push(response.data);
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
		$http({ url: controller + '/' + model.id, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.id == model.id);
				if (item) {
					item.nama = response.data.nama;
					item.direktur = response.data.direktur;
					item.kontak = response.data.kontak;
					item.alamat = response.data.alamat;
					item.email = response.data.email;
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
		$http({ url: controller + '/' + model.id, method: 'Delete' }).then(
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

function PeriodeService($http, $q) {
	var controller = 'api/periode';
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

	service.active = () => {
		var def = $q.defer();
		var data = datas.find((x) => x.status == true);
		if (data) def.resolve(data);
		else {
			$http({ url: controller + '/active', method: 'Get' }).then(
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
				var data = datas.find((x) => x.status);
				if (data) {
					data.status = false;
				}
				datas.push(response.data);
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
		$http({ url: controller + '/' + model.idperiode, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idperiode == model.idperiode);
				if (item) {
					item.tanggalmulai = model.tanggalmulai;
					item.tanggalselesai = model.tanggalselesai;
					item.tanggalundian = model.tanggalundian;
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
		var data = datas.find((x) => x.idKaryawan == id);
		if (data) {
			if (!data.roles) {
				$http({ url: controller + '/roles/' + id, method: 'Get' }).then((response) => {
					data.roles = response.data;
					def.resolve(data);
				});
			} else def.resolve(data);
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
		if (model.dataPhoto) {
			model.photo = null;
		}

		$http({ url: controller, method: 'POST', data: model }).then(
			(response) => {
				datas.push(response.data);
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

		if (model.dataPhoto) {
			model.photo = null;
		}

		$http({ url: controller + '/' + model.id, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idKaryawan == model.idperusahaan);
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
		$http({ url: controller + '/' + model.idKaryawan, method: 'Delete' }).then(
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

	service.setRole = (role) => {
		var def = $q.defer();
		$http({ url: controller + '/changerole', method: 'Post', data: role }).then(
			(response) => {
				var index = datas.indexOf(model);
				datas.splice(index, 1);
				def.resolve(response.data);
			},
			(err) => {
				def.reject(err.message);
			}
		);
	};

	return service;
}

function PelanggaranService($http, $q) {
	var controller = 'api/pelanggaran';
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
		if (model.files) {
			model.files.forEach((file) => {
				file.thumb = null;
			});
		}

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
		$http({ url: controller + '/' + model.idKaryawan, method: 'PUT', data: model }).then(
			(response) => {
				var item = datas.find((x) => x.idKaryawan == model.idperusahaan);
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
		$http({ url: controller + '/' + model.idKaryawan, method: 'Delete' }).then(
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

	service.updateStatus = (id, status) => {
		var def = $q.defer();
		$http({ url: controller + '/status/' + id + '/' + status, method: 'Get' }).then(
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

function HelperService() {
	var service = {};

	service.toDate = (stringDate) => {
		return new Date(stringDate);
	};

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

	service.roles = [
		{ role: 'Manager', checked: false },
		{ role: 'Admin', checked: false },
		{ role: 'Karyawan', checked: false }
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

	service.point = (karyawan, periode) => {
		karyawan.point = 100;
		karyawan.pengurangan = 0;
		var currentdate = new Date();
		var tglawal = new Date(periode.tanggalmulai);
		var tglcompare = angular.copy(tglawal);
		var tglselesai = new Date(periode.tanggalselesai);
		var miliday = 24 * 60 * 60 * 1000;
		var rr = Math.abs(new Date(2020, 7, 30) - currentdate.getDate()) / miliday;
		for (var index = 0; index < currentdate.getDate() - tglawal.getDate(); index++) {
			tglcompare.setDate(tglcompare.getDate() + (index == 0 ? 0 : 1));
			if (tglawal.getDate() + index <= currentdate.getDate() && currentdate.getDate() <= tglselesai.getDate()) {
				var pelanggaran = false;
				karyawan.pelanggarans.forEach((element) => {
					if (element && new Date(element.tanggal).toLocaleDateString() == tglcompare.toLocaleDateString()) {
						pelanggaran = true;
						karyawan.pengurangan += element.karyawan;
					}
				});

				if (!pelanggaran && karyawan.point < 110) {
					karyawan.point += 0.5;
				}
			}
		}
		if (karyawan.point > 110) {
			karyawan.point = 110;
		}

		return karyawan.point - karyawan.pengurangan;
	};
	return service;
}

function UserService($http, $q) {
	var controller = 'api/user';
	var service = {};
	service.profile = () => {
		var def = $q.defer();
		if (service.data) {
			def.resolve(service.data);
		} else {
			$http({ url: controller + '/profile', method: 'Get' }).then(
				(response) => {
					service.data = response.data;

					service.data.InRole = (role) => {
						if (service.data.roles.find((x) => x == role)) {
							return true;
						}
						return false;
					};
					def.resolve(service.data);
				},
				(err) => {
					def.reject(err.message);
				}
			);
		}
		return def.promise;
	};

	return service;
}
