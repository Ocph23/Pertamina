
$(function() {
	var btn = document.getElementById('close-menu');

	if (btn)
		btn.addEventListener('click', (x) => {
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
				span.parentElement.style.justifyContent = left.offsetWidth >= 220 ? 'center' : 'start';
			}
			anime({
				targets: left,
				width: width,
				duration: '200ms',
				easing: 'easeInOutQuad'
			});
		});

    var $el = $('.employes');
    var lenght = 2;
    function anim() {
		var st = $el.scrollTop();
        var sb = $el.prop('scrollHeight') - $el.innerHeight();
        var data= document.getElementsByClassName("employe-data");
        if (data) {
            lenght = data.length <= 0 ? 2 : data.length;
        }
        $el.animate({ scrollTop: st < sb / 2 ? sb : 0 }, lenght*1000 , anim);
	}
	function stop() {
		$el.stop();
    }

    anim();
	$el.hover(stop, anim);
});
