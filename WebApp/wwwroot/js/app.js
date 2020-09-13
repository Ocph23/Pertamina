angular
	.module('dashboard', [ 'service' ])
	.factory('DataService', dataService)
	.controller('dashboard.controller', dashboardController);

function dataService($http, $q) {
	var service = {};

	service.get = (url) => {
		var defer = $q.defer();

		$http({ method: 'get', url: url }).then(
			(response) => {
				defer.resolve(response.data);
			},
			(err) => {
				defer.reject(err);
			}
		);
		return defer.promise;
	};

	service.jenis = () => {
		var defer = $q.defer();
		$http({ method: 'get', url: 'api/dashboard' }).then(
			(response) => {
				defer.resolve(response.data);
			},
			(err) => {
				defer.reject(err);
			}
		);
		return defer.promise;
	};

	return service;
}
function dashboardController($scope, DataService, PointService, PeriodeService) {
	$scope.datas = [];
	DataService.jenis().then((x) => {
		$scope.datas = x.datas;

		var dataDounat = {
			datasets: [
				{
					data: x.datas.map((data) => {
						return data.karyawan;
					}),
					backgroundColor: [ '#CD6155', '#5DADE2', '#F5B041' ]
				}
			],
			labels: [ 'Ringan', 'Sedang', 'Berat' ]
		};

		var ctxdounatChart = document.getElementById('dounatChart').getContext('2d');
		var dounatChart = new Chart(ctxdounatChart, {
			type: 'doughnut',
			data: dataDounat,
			options: {
				responsive: true,
				maintainAspectRatio: false,
				legend: {
					display: true,
					position: 'bottom'
				},

				animation: {
					animateScale: true,
					animateRotate: true
				}
			}
		});

		var ctx = document.getElementById('myChart').getContext('2d');
		var myChart = new Chart(ctx, {
			type: 'bar',
			data: {
				labels: x.perusahaan.map((x) => {
					return x.namaperusahaan;
				}),
				datasets: [
					{
						label: '',
						data: x.perusahaan.map((x) => {
							return x.total;
						}),
						backgroundColor: [
							'rgba(255, 99, 132, 0.5)',
							'rgba(54, 162, 235, 0.5)',
							'rgba(255, 206, 86, 0.5)',
							'rgba(75, 192, 192, 0.5)',
							'rgba(153, 102, 255, 0.5)',
							'rgba(255, 159, 64, 0.5)'
						],
						borderColor: [
							'rgba(255, 99, 132, 1)',
							'rgba(54, 162, 235, 1)',
							'rgba(255, 206, 86, 1)',
							'rgba(75, 192, 192, 1)',
							'rgba(153, 102, 255, 1)',
							'rgba(255, 159, 64, 1)'
						],
						borderWidth: 1
					}
				]
			},
			options: {
				responsive: true,
				legend: {
					display: false
				},
				maintainAspectRatio: false,
				scales: {
					yAxes: [
						{
							ticks: {
								beginAtZero: true
							}
						}
					]
				}
			}
		});

		PeriodeService.active().then((active) => {
			DataService.get('api/karyawan').then((data) => {
				data.forEach((element) => {
					element.total = PointService.point(element, active);
				});

				$scope.source = data.filter((x) => x.total >= 105);
			});
		});
	});
}

angular.module('undian', []).controller('undianController', undianController);
function undianController($scope, $http) {
	var timer = null;
	$scope.isPlay = false;
	$scope.model = {};
	$http({ method: 'get', url: 'api/karyawan' }).then(
		(response) => {
			$scope.datas = response.data;
			$http({ method: 'get', url: 'api/periode/active' }).then(
				(response) => {
					$scope.active = response.data;
				},
				(err) => {
					defer.reject(err);
				}
			);
		},
		(err) => {
			defer.reject(err);
		}
	);

	$scope.start = () => {
		$scope.isPlay = true;
		setTimeout(() => {
			sound('../Sounds/Soft-electronic-track-loop.mp3');
			$scope.sound.play();
			timerStart(100);
		}, 100);
	};

	function timerStart(interval) {
		clearInterval(timer);

		if (interval) {
			timer = setInterval(radomEmployee, interval);
		} else {
			clearInterval(timer);
		}
	}

	function sound(src) {
		$scope.sound = document.getElementById('audio');
		$scope.sound.src = src;
	}

	$scope.play = function() {
		$scope.sound.play();
	};
	$scope.stop = function() {
		$scope.isPlay = 'hide';
		setTimeout(() => {
			timerStart(500);
		}, 5000);
		setTimeout(() => {
			timerStart(1500);
		}, 10000);
		setTimeout(() => {
			clearInterval(timer);
			var win = document.getElementById('winner');
			win.innerHTML = 'PEMENANG';
			$scope.sound.pause();
		}, 15000);
	};

	function radomEmployee() {
		var randomItem = $scope.datas[Math.floor(Math.random() * $scope.datas.length)];
		$scope.model = randomItem;
		var img = document.getElementById('undian');
		var nama = document.getElementById('nama');
		var kode = document.getElementById('kode');
		nama.innerHTML = randomItem.namakaryawan;
		kode.innerHTML = randomItem.kodekaryawan;
		img.src = 'images/profiles/' + randomItem.photo;
	}
}
