"use strict";

var Toast = Swal.mixin({
  toast: true,
  position: 'top-end',
  showConfirmButton: false,
  timer: 4000,
  timerProgressBar: true,
  onOpen: function onOpen(toast) {
    toast.addEventListener('mouseenter', Swal.stopTimer);
    toast.addEventListener('mouseleave', Swal.resumeTimer);
  }
});
angular.module('admin', ['service', 'ui.router', 'datatables', 'files']).directive('dateInput', function () {
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
}).directive('numberInput', function () {
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
}).config(function ($urlRouterProvider, $stateProvider) {
  $urlRouterProvider.otherwise('/home');
  $stateProvider.state({
    name: 'home',
    url: '/home',
    controller: 'homeController',
    templateUrl: './views/home.html'
  }).state({
    name: 'jenis',
    url: '/jenis',
    controller: 'jenisController',
    templateUrl: './views/jenis.html'
  }).state({
    name: 'periode',
    url: '/periode',
    controller: 'periodeController',
    templateUrl: './views/periode.html'
  }).state({
    name: 'karyawan',
    url: '/karyawan',
    controller: 'karyawanController',
    templateUrl: './views/karyawan.html'
  }).state({
    name: 'karyawan-detail',
    url: '/karyawan/:id',
    controller: 'detailKaryawanController',
    templateUrl: './views/karyawan-detail.html'
  }).state({
    name: 'perusahaan',
    url: '/perusahaan',
    controller: 'perusahaanController',
    templateUrl: './views/perusahaan.html'
  }).state({
    name: 'perusahaan-detail',
    url: '/perusahaan/:id',
    controller: 'detailPerusahaanController',
    templateUrl: './views/perusahaan-detail.html'
  }).state({
    name: 'pelanggaran-baru',
    url: '/pelanggaran-baru',
    controller: 'pelanggaranBaru',
    templateUrl: './views/pelanggaran-baru.html'
  }).state({
    name: 'pelanggaran',
    url: '/pelanggaran',
    controller: 'pelanggaranController',
    templateUrl: './views/pelanggaran.html'
  });
}).controller('homeController', homeController).controller('periodeController', periodeController).controller('pelanggaranController', pelanggaranController).controller('pelanggaranBaru', pelanggaranBaru).controller('perusahaanController', perusahaanController).controller('detailPerusahaanController', detailPerusahaanController).controller('karyawanController', karyawanController).controller('detailKaryawanController', detailKaryawanController).controller('jenisController', jenisController);

function homeController($scope, UserService) {
  UserService.profile().then(function (x) {
    $scope.profile = x;
    setTimeout(function () {
      $('#avatar').attr('src', '/images/src/noimage.png');

      if (x.karyawan) {
        $('#avatar').attr('src', x.karyawan.photo);
      }

      $('#userName').text(x.userName);
      $('#role').text(x.roles[0]);
    }, 200);
  });
}

function periodeController($scope, PeriodeService, DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  var e = document.getElementById('close-left');
  PeriodeService.get().then(function (data) {
    $scope.datas = data;
  });

  $scope.save = function (model) {
    if (!model.idperiode) {
      PeriodeService.post(model).then(function (x) {
        Toast.fire({
          icon: 'success',
          title: 'Data Berhasil Ditambah !'
        }, function (err) {
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Ditambah !'
          });
        });
      });
    } else {
      PeriodeService.put(model).then(function (x) {
        Toast.fire({
          icon: 'success',
          title: 'Data Berhasil diubah !'
        });
      }, function (err) {
        Toast.fire({
          icon: 'error',
          title: 'Data Tidak Berhasil Diubah !'
        });
      });
    }
  };

  $scope.select = function (model) {
    $scope.model = model;
    $scope.addItem = true;
  };

  $scope.closeAddItem = function () {
    $scope.addItem = false;
  };

  $scope.newItem = function () {
    $scope.addItem = true;
    $scope.model = {
      photo: 'noimage.png'
    };
  };

  $scope["delete"] = function (item) {
    PeriodeService["delete"](item.idperiode).then(function (x) {
      Toast.fire({
        icon: 'success',
        title: 'Data Berhasil Diubah !'
      });
    });
  };
}

function pelanggaranBaru($scope, KaryawanService, JenisService, PelanggaranService, DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  var dropbox = document.getElementById('imgPelanggaran');
  var btndropbox = document.getElementById('btnAddFile');
  setFilesContainer(dropbox);
  setFilesContainer(btndropbox);

  function setFilesContainer(e) {
    e.addEventListener('click', function (x) {
      $('#fileInput').click();
    });
    e.addEventListener('dragenter', dragenter, false);
    e.addEventListener('dragover', dragover, false);
    e.addEventListener('drop', drop, false);
  }

  function drop(e) {
    e.stopPropagation();
    e.preventDefault();
    var dt = e.dataTransfer;
    var files = dt.files;
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

  KaryawanService.get().then(function (x) {
    $scope.datas = x;
    JenisService.get().then(function (jenis) {
      $scope.datajenis = jenis;
    });
  });

  $scope.selectUser = function (data) {
    $scope.datas.forEach(function (x) {
      x.userSelect = false;
    });
    data.userSelect = true;
  };

  $scope.cancelUser = function (data) {
    setTimeout(function (x) {
      $scope.$apply(function (x) {
        data.userSelect = false;
      });
    }, 300);
  };

  $scope.save = function (item, files) {
    item.idperusahaan = item.perusahaanKaryawan.idperusahaan;
    item.idjenispelanggaran = item.jenis.idjenispelanggaran;
    item.files = angular.copy(files);
    item.karyawan = item.jenis.pengurangankaryawan;
    item.perusahaan = item.jenis.penguranganperusahaan;
    PelanggaranService.post(item).then(function (x) {
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

  $scope.selectImage = function (item) {
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

  $scope.getFiles = function (files) {
    $scope.proccessFile(files);
  };

  $scope.removeFile = function (file) {
    var index = $scope.files.indexOf(file);
    $scope.files.splice(index, 1);
    dropbox.style.backgroundImage = 'url()';
  };

  $scope.proccessFile = function (files) {
    $scope.files = [];
    Array.from(files).forEach(function (element, index) {
      var reader = new FileReader();

      reader.onload = function (e) {
        var data = {
          filename: '',
          data: null,
          filetype: element.type
        };

        if (element.type.includes('video')) {
          //	var src = thumbnail; ///video url not youtube or vimeo,just video on server
          var video = document.createElement('video');
          video.width = 360;
          video.height = 240;
          var canvas = document.createElement('canvas');
          canvas.width = 360;
          canvas.height = 240;
          var context = canvas.getContext('2d');
          video.addEventListener('loadeddata', function (xx) {
            setTimeout(function () {
              context.drawImage(video, 0, 0, canvas.width, canvas.height);
              data.thumb = canvas.toDataURL('image/jpeg');
              data.data = e.target.result.split(',')[1];

              if (index == 0) {
                dropbox.style.backgroundImage = 'url(' + data.source + ')';
              }

              $scope.$apply(function (x) {
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

          $scope.$apply(function (x) {
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
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl'); // "bPaginate": false,
  // "bLengthChange": false,
  // "bFilter": true,
  // "bInfo": false,

  UserService.profile().then(function (x) {
    $scope.profile = x;
    JenisService.get().then(function (x) {
      $scope.items = x;
      $scope.createNew();
    });
  });

  $scope.createNew = function () {
    var emptyExists = $scope.items.find(function (x) {
      return x.isNew;
    });
    if (!emptyExists) $scope.items.push({
      idlevel: 0,
      level: '',
      isNew: true
    });
  };

  $scope.createNewDetail = function () {
    if ($scope.selected) {
      var emptyExists = $scope.items.find(function (x) {
        return x.isNew;
      });
    }

    if (!emptyExists) $scope.items.push({
      nama: '',
      pengurangankaryawan: 0,
      penguranganperusahaan: 0,
      penambahanpoint: 0,
      isNew: true
    });
  };

  $scope.selectItem = function (item) {
    var selectExists = $scope.items.find(function (x) {
      return x.select;
    });

    if (selectExists) {
      selectExists.select = false;
    }

    $scope.selected = item;
    $scope.selected.select = true;

    if ($scope.selected) {}
  };

  $scope.saveJenis = function (model) {
    if (!model.proccess) {
      model.proccess = true;

      if (!model.idlevel) {
        JenisService.post(model).then(function (x) {
          model.proccess = false;
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Ditambah !'
          });
          model.isNew = false;
          model.idlevel = x.idlevel;
          $scope.selected = model;
          $scope.selected.selected = true;
          $scope.createNew();
        }, function (e) {});
      } else {
        JenisService.put(model).then(function (x) {
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Diubah !'
          });
          model.proccess = false;
          model.isNew = false;
        }, function (e) {});
      }
    }
  };

  $scope.deleteJenis = function (item) {
    Swal.fire({
      title: 'Yakin ?',
      text: 'Hapus Data !',
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes'
    }).then(function (result) {
      if (result.value) {
        JenisService["delete"](item).then(function (x) {
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Di Hapus !'
          });
        }, function (e) {});
      }
    });
  };

  $scope.addDetail = function () {
    if ($scope.selected) {
      if (!$scope.selected.datas) {
        $scope.selected.datas = [];
      }

      var emptyExist = $scope.selected.datas.find(function (x) {
        return x.isNew;
      });

      if (!emptyExist) {
        $scope.selected.datas.push({
          nama: '',
          point: 0,
          deskripsi: '',
          isNew: true
        });
      }
    }
  };

  $scope.saveDetail = function (model) {
    if (!model.proccess) {
      model.proccess = true;

      if (!model.idjenispelanggaran) {
        model.idlevel = $scope.selected.idlevel;
        JenisService.postDetail(model).then(function (x) {
          model.proccess = false;
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Ditambah !'
          });
          model.idjenispelanggaran = x.idjenispelanggaran;
          model.isNew = false;
        }, function (e) {});
      } else {
        JenisService.putDetail(model).then(function (x) {
          var detail = $scope.selected.datas.find(function (x) {
            return x.idjenispelanggaran == model.idjenispelanggaran;
          });

          if (detail) {
            detail.jenispelanggaran = model.jenispelanggaran;
          }

          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Diubah !'
          });
          model.proccess = false;
          model.isNew = false;
        }, function (e) {});
      }
    }
  };

  $scope.deleteDetail = function (item) {
    Swal.fire({
      title: 'Are you sure?',
      text: "You won't be able to revert this!",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Yes, delete it!'
    }).then(function (result) {
      if (result.value) {
        JenisService.deleteDetail(item).then(function (x) {
          var index = $scope.selected.datas.indexOf(item);
          $scope.selected.datas.splice(index, 1);
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Diubah !'
          });
        }, function (e) {});
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
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  $('#photo').on('click', function (x) {
    $('#fileInput').click();
  });
  PerusahaanService.get().then(function (x) {
    var time = 0;
    x.forEach(function (m) {
      setTimeout(function () {
        $scope.$apply(function () {
          $scope.datas.push(m);
        });
      }, time += 200);
    });
    $scope.addItem = false;
  });

  $scope.save = function (model) {
    if (!$scope.isBusy) {
      $scope.isBusy = true;

      if (!model.idperusahaan) {
        PerusahaanService.post(model).then(function (x) {
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Diubah !'
          });
          $scope.newItem();
        });
        $scope.isBusy = false;
      } else {
        PerusahaanService.put(model).then(function (x) {
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

  $scope.select = function (item) {
    $scope.model = item;

    if (!item.logo) {
      item.logo = 'noimage.png';
    }

    $scope.addItem = true;
  };

  $scope.newItem = function () {
    $scope.addItem = true;
    $scope.model = {
      logo: 'noimage.png'
    };
  };

  $scope.getPhoto = function (files, model) {
    Array.from(files).forEach(function (element, index) {
      var reader = new FileReader();

      reader.onload = function (e) {
        model.dataPhoto = e.target.result.split(',')[1]; //model.logo = e.target.result;

        var photo = document.getElementById('photo');
        photo.src = e.target.result;
      };

      reader.readAsDataURL(element);
    });
  };
}

function karyawanController($scope, PerusahaanService, KaryawanService, DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  $scope.isBusy = false;
  $scope.addItem = false;
  $scope.model = {
    photo: 'noimage.png',
    perusahaanKaryawan: {}
  };
  $('#photo').on('click', function (x) {
    $('#fileInput').click();
  });
  PerusahaanService.get().then(function (dataperusahaan) {
    $scope.perusahaan = dataperusahaan;
    KaryawanService.get().then(function (karyawans) {
      $scope.datas = karyawans;
    });
  });

  $scope.save = function (model) {
    if (!$scope.isBusy) {
      $scope.isBusy = true;

      if (!model.idKaryawan) {
        KaryawanService.post(model).then(function (x) {
          Toast.fire({
            icon: 'success',
            title: 'Data Berhasil Diubah !'
          });
          $scope.newItem();
        });
        $scope.isBusy = false;
      } else {
        KaryawanService.put(model).then(function (x) {
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

  $scope.selectItem = function (model) {
    $scope.model = model;
    $scope.addItem = true;
  };

  $scope.newItem = function () {
    $scope.addItem = true;
    $scope.model = {
      photo: 'noimage.png',
      perusahaanKaryawan: {}
    };
  };

  $scope.getPhoto = function (files, model) {
    Array.from(files).forEach(function (element, index) {
      var reader = new FileReader();

      reader.onload = function (e) {
        model.dataPhoto = e.target.result.split(',')[1];
        model.photo = e.target.result;
        var photo = document.getElementById('photo');
        photo.src = e.target.result;
      };

      reader.readAsDataURL(element);
    });
  };
}

function detailPerusahaanController($scope, $stateParams, KaryawanService, PerusahaanService, HelperService, PointService, UserService, PeriodeService, DTOptionsBuilder, DTColumnDefBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  $scope.helper = HelperService;
  PerusahaanService.getById($stateParams.id).then(function (x) {
    $scope.model = x;
    PeriodeService.active().then(function (active) {//var total = PointService.point($scope.model, active);
    });
  });

  $scope.myFilter = function (month, year) {
    if (month && year) {
      return function (event) {
        var bulan = new Date(event.tanggal).getMonth();
        var tahun = new Date(event.tanggal).getFullYear();
        return bulan == month.value && tahun == year ? true : false;
      };
    } else {
      return function (event) {
        return false;
      };
    }
  };

  $scope.onChangeRole = function (model, role) {
    role.idKaryawan = model.idKaryawan;
    KaryawanService.setRole(role).then(function (x) {}, function (err) {
      role.checked = !role.checked;
      Toast.fire({
        icon: 'success',
        title: 'Data Berhasil Diubah !'
      });
    });
  };

  $scope.selectBukti = function (files) {
    $scope.selectedFiles = files;
    var picture = document.getElementById('picture');
    var video = document.getElementById('myvideo');
    picture.src = '';
    video.src = '';
    picture.style.display = 'none';
    video.style.display = 'none';
  };

  $scope.selectFile = function (file) {
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

function detailKaryawanController($scope, $stateParams, KaryawanService, PelanggaranService, HelperService, PointService, UserService, PeriodeService, DTOptionsBuilder) {
  $scope.helper = HelperService;
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  UserService.profile().then(function (profile) {
    $scope.profile = profile;
    KaryawanService.getById($stateParams.id).then(function (x) {
      $scope.model = x;
      $scope.roles = [];
      $scope.helper.roles.forEach(function (element) {
        var role = element;
        role.checked = false;

        if (x.roles.find(function (x) {
          return x == element.role.toLowerCase();
        })) {
          role.checked = true;
        }

        PeriodeService.active().then(function (active) {
          var ctx = document.getElementById('chartPoint');
          var context = ctx.getContext('2d');
          context.clearRect(0, 0, ctx.width, ctx.height);
          var total = PointService.point($scope.model, active); // write text plugin

          var myPieChart = new Chart(ctx, {
            type: 'doughnut',
            data: {
              datasets: [{
                data: [total, $scope.model.pengurangan],
                backgroundColor: ['#84c125', '#d9241b']
              }],
              // These labels appear in the legend and in the tooltips when hovering different arcs
              labels: ['Point', 'Pengurangan']
            },
            options: {
              //cutoutPercentage: 40,
              responsive: false,
              legend: {
                display: false
              }
            }
          });
          Chart.pluginService.register({
            beforeDraw: function beforeDraw(chart) {
              var width = chart.chart.width,
                  height = chart.chart.height,
                  ctx = chart.chart.ctx;
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
    });
  });

  $scope.myFilter = function (month, year) {
    if (month && year) {
      return function (event) {
        var bulan = new Date(event.tanggal).getMonth();
        var tahun = new Date(event.tanggal).getFullYear();
        return bulan == month.value && tahun == year ? true : false;
      };
    } else {
      return function (event) {
        return false;
      };
    }
  };

  $scope.onChangeRole = function (model, role) {
    role.idKaryawan = model.idKaryawan;
    KaryawanService.setRole(role).then(function (x) {}, function (err) {
      role.checked = !role.checked;
      Toast.fire({
        icon: 'success',
        title: 'Data Berhasil Diubah !'
      });
    });
  };

  $scope.selectBukti = function (files) {
    $scope.selectedFiles = files;
    var picture = document.getElementById('picture');
    var video = document.getElementById('myvideo');
    picture.src = '';
    video.src = '';
    picture.style.display = 'none';
    video.style.display = 'none';
  };

  $scope.selectFile = function (file) {
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

function pelanggaranController($scope, PelanggaranService, HelperService, PeriodeService, DTOptionsBuilder) {
  var vm = this;
  vm.dtOptions = DTOptionsBuilder.newOptions().withPaginationType('full_numbers').withDisplayLength(10).withDOM('pitrfl');
  $scope.helper = HelperService;
  $scope.bulans = [];
  PeriodeService.active().then(function (active) {
    $scope.active = active;
    PelanggaranService.get().then(function (datas) {
      $scope.datas = datas;
      var bulan = {
        value: -1,
        name: 'Periode Active'
      };
      $scope.bulans.push(bulan);
      HelperService.bulans.forEach(function (element) {
        $scope.bulans.push(element);
      });
      UserService.profile().then(function (x) {
        $scope.profile = x;
      });
    });
  });

  $scope.myFilter = function (month, year) {
    if (month && month.value == -1 && $scope.active) {
      return function (event) {
        var tgl = new Date(event.tanggal);
        return tgl >= new Date($scope.active.tanggalmulai) && tgl <= new Date($scope.active.tanggalselesai);
      };
    } else if (month && year) {
      return function (event) {
        var bulan = new Date(event.tanggal).getMonth();
        var tahun = new Date(event.tanggal).getFullYear();
        return bulan == month.value && tahun == year ? true : false;
      };
    } else {
      return function (event) {
        return false;
      };
    }
  };

  $scope.selectBukti = function (data) {
    $scope.selected = data;
    $scope.selectedFiles = data.files;
    var picture = document.getElementById('picture');
    var video = document.getElementById('myvideo');
    picture.src = '';
    video.src = '';
    picture.style.display = 'none';
    video.style.display = 'none';
  };

  $scope.selectFile = function (file) {
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

  $scope.updateStatus = function (data, status) {
    PelanggaranService.updateStatus(data.idpelanggaran, status).then(function (x) {
      data.status = x;
      $('#bukti').modal('hide');
    }, function (err) {});
  };
}