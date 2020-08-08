$(function() {
	document.getElementById('close-menu').addEventListener('click', (x) => {
		var left = document.getElementsByClassName('main-left')[0];

		var width = left.offsetWidth < 220 ? '220px' : '110px';
		var spans = document.getElementsByClassName('menu-text');
		var avatar = document.getElementById('avatar');
		var userName = document.getElementsByClassName('user-name')[0];
		var userJob = document.getElementsByClassName('user-job')[0];
		if (left.offsetWidth < 220) {
			anime({
				targets: avatar,
				width: '100px',
				height: '100px',
				duration: '200ms',
				easing: 'easeInOutQuad'
			});
			userName.style.display = 'block';
			userJob.style.display = 'block';
			anime({
				targets: document.getElementById('close-icon'),
				rotate: {
					value: 0,
					duration: 200,
					easing: 'easeInOutSine'
				},
				delay: 150
			});
		} else {
			anime({
				targets: avatar,
				width: '75px',
				height: '75px',
				duration: '200ms',
				easing: 'easeInOutQuad'
			});
			userName.style.display = 'none';
			userJob.style.display = 'none';
			anime({
				targets: document.getElementById('close-icon'),
				rotate: {
					value: 180,
					duration: 200,
					easing: 'easeInOutSine'
				},
				delay: 150
			});
		}

		for (const span of spans) {
			span.style.display = left.offsetWidth >= 220 ? 'none' : 'block';
			span.parentElement.style.justifyContent = 'center';
		}

		anime({
			targets: left,
			width: width,
			duration: '200ms',
			easing: 'easeInOutQuad'
		});
	});
});
