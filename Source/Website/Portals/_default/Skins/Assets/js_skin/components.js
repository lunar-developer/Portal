/**
 Core layout handlers and component wrappers
 **/

// BEGIN: Layout Brand
var LayoutBrand = function () {

	return {
		//main function to initiate the module
		init: function () {
			$("body").on("click", ".c-hor-nav-toggler", function () {
				var target = $(this).data("target");
				$(target).toggleClass("c-shown");
			});
		}

	};
}();
// END


// BEGIN: Layout Header
var LayoutHeader = function () {
	var offset = parseInt($(".c-layout-header").attr("data-minimize-offset") > 0 ? parseInt($(".c-layout-header").attr("data-minimize-offset")) : 0);
	var handleHeaderOnScroll = function () {
		if ($(window).scrollTop() > offset) {
			$("body").addClass("c-page-on-scroll");
		} else {
			$("body").removeClass("c-page-on-scroll");
		}
	}

	var handleTopbarCollapse = function () {
		$(".c-layout-header .c-topbar-toggler").on("click", function () {
			$(".c-layout-header-topbar-collapse").toggleClass("c-topbar-expanded");
		});
	}

	return {
		//main function to initiate the module
		init: function () {
            // ReSharper disable once UnknownCssClass
			if ($("body").hasClass("c-layout-header-fixed-non-minimized")) {
				return;
			}

			handleHeaderOnScroll();
			handleTopbarCollapse();

			$(window).scroll(function () {
				handleHeaderOnScroll();
			});
		}
	};
}();
// END


// BEGIN: Layout Mega Menu
var LayoutMegaMenu = function () {

	return {
		//main function to initiate the module
		init: function () {
			$(".c-mega-menu").on("click", ".c-toggler", function (e) {
				if (App.getViewPort().width < App.getBreakpoint("md")) {
					e.preventDefault();
					if ($(this).closest("li").hasClass("c-open")) {
						$(this).closest("li").removeClass("c-open");
					} else {
						$(this).closest("li").addClass("c-open");
					}
				}
			});

			$(".c-layout-header .c-hor-nav-toggler:not(.c-quick-sidebar-toggler)").on("click", function () {
				$(".c-layout-header").toggleClass("c-mega-menu-shown");

				if ($("body").hasClass("c-layout-header-mobile-fixed")) {
					var height = App.getViewPort().height - $(".c-layout-header").outerHeight(true) - 60;
					$(".c-mega-menu").css("max-height", height);
				}
			});
		}
	};
}();
// END

// BEGIN: Layout Go To Top
var LayoutGo2Top = function () {

	var handle = function () {
		var currentWindowPosition = $(window).scrollTop(); // current vertical position
		if (currentWindowPosition > 300) {
			$(".c-layout-go2top").show();
		} else {
			$(".c-layout-go2top").hide();
		}
	};

	return {

		//main function to initiate the module
		init: function () {

			handle(); // call headerFix() when the page was loaded

			if (navigator.userAgent.match(/iPhone|iPad|iPod/i)) {
				$(window).bind("touchend touchcancel touchleave", function () {
					handle();
				});
			} else {
				$(window).scroll(function () {
					handle();
				});
			}

			$(".c-layout-go2top").on("click", function (e) {
				e.preventDefault();
				$("html, body").animate({
					scrollTop: 0
				}, 600);
			});
		}

	};
}();
// END: Layout Go To Top

// BEGIN : SCROLL TO VIEW DETECTION
function isScrolledIntoView(elem)
{
    var $elem = $(elem);
    var $window = $(window);

    var docViewTop = $window.scrollTop();
    var docViewBottom = docViewTop + $window.height();

    var elemTop = $elem.offset().top;
    var elemBottom = elemTop + $elem.height();

    return ((elemBottom <= docViewBottom) && (elemTop >= docViewTop));
}
// END : SCROLL TO VIEW FUNCTION

// Main theme initialization
$(document).ready(function () {
	// init layout handlers
	LayoutBrand.init();
	LayoutHeader.init();
	LayoutMegaMenu.init();
	LayoutGo2Top.init();
});