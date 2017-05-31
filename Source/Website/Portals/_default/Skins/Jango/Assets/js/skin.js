$(document).ready(function() {
    var slider = $(".c-layout-revo-slider .tp-banner");

    slider.show().revolution({
        sliderType: "standard",
        sliderLayout: "fullscreen",
        gridwidth: 4064,
        gridheight: 720,
        delay: 15000,
        startwidth: 1170,
        startheight: App.getViewPort().height,
        navigationType: "hide",
        navigationArrows: "solo",
        touchenabled: "on",
        navigation: {
            keyboardNavigation: "off",
            keyboard_direction: "horizontal",
            mouseScrollNavigation: "off",
            onHoverStop: "on",
            arrows: {
                style: "circle",
                enable: true,
                hide_onmobile: false,
                hide_onleave: false,
                tmp: "",
                left: {
                    h_align: "left",
                    v_align: "center",
                    h_offset: 30,
                    v_offset: 0
                },
                right: {
                    h_align: "right",
                    v_align: "center",
                    h_offset: 30,
                    v_offset: 0
                }
            }
        },
        spinner: "spinner2",
        shadow: 0,
        disableProgressBar: "on",
        hideThumbsOnMobile: "on",
        hideNavDelayOnMobile: 1500,
        hideBulletsOnMobile: "on",
        hideArrowsOnMobile: "on",
        hideThumbsUnderResolution: 0
    });

    /***************** Smooth Scrolling ******************/
    $("a[href*='#']:not([href='#'])")
        .click(function () {
            if (location.pathname.replace(/^\//, "") === this.pathname.replace(/^\//, "") &&
                location.hostname === this.hostname) {
                var target = $(this.hash);
                target = target.length ? target : $("[name=" + this.hash.slice(1) + "]");
                if (target.length) {
                    $("html, body")
                        .animate({
                            scrollTop: target.offset().top
                        },
                            2000);
                    return false;
                }
            }
            return false;
        });
}); //ready