angular.module('admin', [ 'ui.router' ]).config(function($urlRouterProvider, $stateProvider) {
	$urlRouterProvider.otherwise('/home');
	$stateProvider
		.state({
			name: 'home',
			url: '/home',
			template: 'Home'
		})
		.state({
			name: 'karyawan',
			url: '/karyawan',
			templateUrl: './views/karyawan.html'
		})
		.state({
			name: 'perusahaan',
			url: '/perusahaan',
			templateUrl: './views/perusahaan.html'
		})
		.state({
			name: 'pelanggaran-baru',
			url: '/pelanggaran-baru',
			templateUrl: './views/pelanggaran-baru.html'
		});
});
