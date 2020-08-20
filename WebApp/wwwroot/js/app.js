angular
	.module('dashboard', [])
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
function dashboardController($scope, DataService) {
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

			// These labels appear in the legend and in the tooltips when hovering different arcs
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
					display: false
				},

				animation: {
					animateScale: true,
					animateRotate: true
				}
			}
		});

		DataService.get('api/karyawan').then((data) => {
			$scope.source = data;
			var ctx = document.getElementById('myChart').getContext('2d');
			var myChart = new Chart(ctx, {
				type: 'bar',
				data: {
					labels: [ 'Red', 'Blue', 'Yellow', 'Green', 'Purple', 'Orange' ],
					datasets: [
						{
							label: '# of Votes',
							data: [ 12, 19, 3, 5, 2, 3 ],
							backgroundColor: [
								'rgba(255, 99, 132, 0.2)',
								'rgba(54, 162, 235, 0.2)',
								'rgba(255, 206, 86, 0.2)',
								'rgba(75, 192, 192, 0.2)',
								'rgba(153, 102, 255, 0.2)',
								'rgba(255, 159, 64, 0.2)'
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
		});
	});
}
