angular.module('files', []).directive('ngFiles', [
	'$parse',
	function($parse) {
		function flink(scope, element, attrs) {
			var change = $parse(attrs.ngFiles);

			element.on('change', (event) => {
				change(scope, { $files: event.target.files });
			});
		}

		return { link: flink };
	}
]);
