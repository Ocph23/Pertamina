"use strict";

angular.module('service', []).factory('HelperService', HelperService).factory('JenisService', JenisService).factory('KaryawanService', KaryawanService).factory('PelanggaranService', PelanggaranService).factory('PerusahaanService', PerusahaanService).factory('UserService', UserService).factory('PeriodeService', PeriodeService).factory('PointService', PointService);

function JenisService($http, $q) {
  var controller = 'api/level';
  var controllerDetail = 'api/jenispelanggaran';
  var service = {};
  var datas = [];

  service.get = function () {
    var def = $q.defer();

    if (!service.instance) {
      $http({
        url: controller,
        method: 'Get'
      }).then(function (response) {
        datas = response.data;
        service.instance = true;
        def.resolve(datas);
      }, function (err) {
        def.reject(err.message);
      });
    } else {
      def.resolve(datas);
    }

    return def.promise;
  };

  service.post = function (model) {
    var def = $q.defer();
    $http({
      url: controller,
      method: 'POST',
      data: model
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.put = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idlevel,
      method: 'PUT',
      data: model
    }).then(function (response) {
      var item = datas.find(function (x) {
        return x.idlevel == model.idlevel;
      });

      if (item) {
        item.level = model.level;
      }

      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service["delete"] = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idlevel,
      method: 'Delete'
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  }; //deletail


  service.postDetail = function (model) {
    var def = $q.defer();
    $http({
      url: controllerDetail,
      method: 'POST',
      data: model
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.putDetail = function (model) {
    var def = $q.defer();
    $http({
      url: controllerDetail + '/' + model.idjenispelanggaran,
      method: 'PUT',
      data: model
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.deleteDetail = function (model) {
    var def = $q.defer();
    $http({
      url: controllerDetail + '/' + model.idjenispelanggaran,
      method: 'Delete'
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  return service;
}

function PerusahaanService($http, $q) {
  var controller = 'api/perusahaan';
  var service = {};
  var datas = [];

  service.get = function () {
    var def = $q.defer();

    if (!service.instance) {
      $http({
        url: controller,
        method: 'Get'
      }).then(function (response) {
        datas = response.data;
        service.instance = true;
        def.resolve(datas);
      }, function (err) {
        def.reject(err.message);
      });
    } else {
      def.resolve(datas);
    }

    return def.promise;
  };

  service.getById = function (id) {
    var def = $q.defer();
    $http({
      url: controller + '/' + id,
      method: 'Get'
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.post = function (model) {
    var def = $q.defer();
    $http({
      url: controller,
      method: 'POST',
      data: model
    }).then(function (response) {
      datas.push(response.data);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.put = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idperusahaan,
      method: 'PUT',
      data: model
    }).then(function (response) {
      var item = datas.find(function (x) {
        return x.idperusahaan == model.idperusahaan;
      });

      if (item) {
        item.level = model.level;
      }

      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service["delete"] = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idperusahaan,
      method: 'Delete'
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  return service;
}

function PeriodeService($http, $q) {
  var controller = 'api/periode';
  var service = {};
  var datas = [];

  service.get = function () {
    var def = $q.defer();

    if (!service.instance) {
      $http({
        url: controller,
        method: 'Get'
      }).then(function (response) {
        datas = response.data;
        service.instance = true;
        def.resolve(datas);
      }, function (err) {
        def.reject(err.message);
      });
    } else {
      def.resolve(datas);
    }

    return def.promise;
  };

  service.active = function () {
    var def = $q.defer();
    var data = datas.find(function (x) {
      return x.status == true;
    });
    if (data) def.resolve(data);else {
      $http({
        url: controller + '/active',
        method: 'Get'
      }).then(function (response) {
        datas.push(response.data);
        def.resolve(response.data);
      }, function (err) {
        def.reject(err.message);
      });
    }
    return def.promise;
  };

  service.post = function (model) {
    var def = $q.defer();
    $http({
      url: controller,
      method: 'POST',
      data: model
    }).then(function (response) {
      var data = datas.find(function (x) {
        return x.status;
      });

      if (data) {
        data.status = false;
      }

      datas.push(response.data);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.put = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idperiode,
      method: 'PUT',
      data: model
    }).then(function (response) {
      var item = datas.find(function (x) {
        return x.idperiode == model.idperiode;
      });

      if (item) {
        item.tanggalmulai = model.tanggalmulai;
        item.tanggalselesai = model.tanggalselesai;
        item.tanggalundian = model.tanggalundian;
      }

      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service["delete"] = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idperusahaan,
      method: 'Delete'
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  return service;
}

function KaryawanService($http, $q) {
  var controller = 'api/karyawan';
  var service = {};
  var datas = [];

  service.get = function () {
    var def = $q.defer();

    if (!service.instance) {
      $http({
        url: controller,
        method: 'Get'
      }).then(function (response) {
        datas = response.data;
        service.instance = true;
        def.resolve(datas);
      }, function (err) {
        def.reject(err.message);
      });
    } else {
      def.resolve(datas);
    }

    return def.promise;
  };

  service.getById = function (id) {
    var def = $q.defer();
    var data = datas.find(function (x) {
      return x.idKaryawan == id;
    });

    if (data) {
      if (!data.roles) {
        $http({
          url: controller + '/roles/' + id,
          method: 'Get'
        }).then(function (response) {
          data.roles = response.data;
          def.resolve(data);
        });
      } else def.resolve(data);
    } else {
      $http({
        url: controller + '/' + id,
        method: 'Get'
      }).then(function (response) {
        datas.push(response.data);
        def.resolve(response.data);
      }, function (err) {
        def.reject(err.message);
      });
    }

    return def.promise;
  };

  service.post = function (model) {
    var def = $q.defer();
    $http({
      url: controller,
      method: 'POST',
      data: model
    }).then(function (response) {
      datas.push(response.data);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.put = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idKaryawan,
      method: 'PUT',
      data: model
    }).then(function (response) {
      var item = datas.find(function (x) {
        return x.idKaryawan == model.idperusahaan;
      });

      if (item) {
        item.level = model.level;
      }

      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service["delete"] = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idKaryawan,
      method: 'Delete'
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.setRole = function (role) {
    var def = $q.defer();
    $http({
      url: controller + '/changerole',
      method: 'Post',
      data: role
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
  };

  return service;
}

function PelanggaranService($http, $q) {
  var controller = 'api/pelanggaran';
  var service = {};
  var datas = [];

  service.get = function () {
    var def = $q.defer();

    if (!service.instance) {
      $http({
        url: controller,
        method: 'Get'
      }).then(function (response) {
        datas = response.data;
        service.instance = true;
        def.resolve(datas);
      }, function (err) {
        def.reject(err.message);
      });
    } else {
      def.resolve(datas);
    }

    return def.promise;
  };

  service.getById = function (id) {
    var def = $q.defer();
    $http({
      url: controller + '/karyawan/' + id,
      method: 'Get'
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.post = function (model) {
    var def = $q.defer();

    if (model.files) {
      model.files.forEach(function (file) {
        file.thumb = null;
      });
    }

    var modelData = {
      idpelanggaran: 0,
      idperusahaan: model.idperusahaan,
      idjenispelanggaran: model.idjenispelanggaran,
      idKaryawan: model.idKaryawan,
      karyawan: model.karyawan,
      perusahaan: model.perusahaan,
      tanggal: new Date(),
      files: model.files
    };
    model.tanggal = null;
    $http({
      url: controller,
      method: 'POST',
      data: modelData
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.put = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idKaryawan,
      method: 'PUT',
      data: model
    }).then(function (response) {
      var item = datas.find(function (x) {
        return x.idKaryawan == model.idperusahaan;
      });

      if (item) {
        item.level = model.level;
      }

      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service["delete"] = function (model) {
    var def = $q.defer();
    $http({
      url: controller + '/' + model.idKaryawan,
      method: 'Delete'
    }).then(function (response) {
      var index = datas.indexOf(model);
      datas.splice(index, 1);
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  service.updateStatus = function (id, status) {
    var def = $q.defer();
    $http({
      url: controller + '/status/' + id + '/' + status,
      method: 'Get'
    }).then(function (response) {
      def.resolve(response.data);
    }, function (err) {
      def.reject(err.message);
    });
    return def.promise;
  };

  return service;
}

function HelperService() {
  var service = {};

  service.toDate = function (stringDate) {
    return new Date(stringDate);
  };

  service.bulans = [{
    name: 'Januari',
    value: 0
  }, {
    name: 'Februari',
    value: 1
  }, {
    name: 'Maret',
    value: 2
  }, {
    name: 'April',
    value: 3
  }, {
    name: 'Mei',
    value: 4
  }, {
    name: 'Juni',
    value: 5
  }, {
    name: 'Juli',
    value: 6
  }, {
    name: 'Agustus',
    value: 7
  }, {
    name: 'September',
    value: 8
  }, {
    name: 'Oktober',
    value: 9
  }, {
    name: 'November',
    value: 10
  }, {
    name: 'Desember',
    value: 11
  }];
  service.roles = [{
    role: 'Manager',
    checked: false
  }, {
    role: 'Admin',
    checked: false
  }, {
    role: 'Karyawan',
    checked: false
  }];

  service.tahuns = function getTahun() {
    var tahun = new Date().getFullYear();
    var tahuns = [];

    for (var index = tahun; index > tahun - 5; index--) {
      tahuns.push(index);
    }

    return tahuns;
  };

  return service;
}

function PointService() {
  var date = new Date();
  var service = {};

  service.point = function (karyawan, periode) {
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
        karyawan.pelanggarans.forEach(function (element) {
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

  service.profile = function () {
    var def = $q.defer();

    if (service.data) {
      def.resolve(service.data);
    } else {
      $http({
        url: controller + '/profile',
        method: 'Get'
      }).then(function (response) {
        service.data = response.data;

        service.data.InRole = function (role) {
          if (service.data.roles.find(function (x) {
            return x == role;
          })) {
            return true;
          }

          return false;
        };

        def.resolve(service.data);
      }, function (err) {
        def.reject(err.message);
      });
    }

    return def.promise;
  };

  return service;
}