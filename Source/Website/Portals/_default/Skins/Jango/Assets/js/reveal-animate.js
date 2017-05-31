// REVEAL ANIMATE

var revealAnimate = function() {

	var initialize = function() {

		wow = new WOW(
		{
			animateClass: "animated",
			offset:100,
			live: true,
			mobile: false
		});

	}

	return {
		//main function to initiate the module
		init: function() {

		    initialize();

		}

	};
}();

$(document).ready(function() {
	revealAnimate.init();
	new WOW().init();
	setTimeout(
		function() {
			$(".wow").css("opacity", 1);
		}, 100
	);	
});


