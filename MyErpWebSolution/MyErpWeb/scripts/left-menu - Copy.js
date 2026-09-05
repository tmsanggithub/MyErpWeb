
$(window).ready(function () {
    $('.hamburger-menu').click(function () {
        if ($('#left_menu_sidebar').hasClass('active')) {
            $('#left_menu_sidebar').removeClass('active');
            $('.hamburger-menu').addClass('hide');
        }
        else {
            $('#left_menu_sidebar').addClass('active');
            $('.hamburger-menu').removeClass('hide');
        }

    });
    $(".exit_menu_left").click(function () {
        $("#left_menu_sidebar").addClass("active");
        $(".left-main-menu").removeClass("active");
        // $(".dark_bg_black").hide();


    });

    $(".dark_bg_black").click(function () {
        $(".left-content_menu").removeClass("active");
        $(".left-main-menu").removeClass("active");
        $(this).hide();

    });

    $(".left-form-login-link").click(function () {
        $(".left-content_menu").removeClass("active");
        $(".left-main-menu").removeClass("active");
        $(".left-content-form-login").addClass("active");
        $(".dark_bg_black").show();
    });

    $("#left-list-link-menu-left-1 .left-list-link-menu-left-other").click(function () {
        $("#left-list-link-menu-left-2 ul").addClass('hidden');
        var obj = $("#left-list-link-menu-left-2 ul:nth-child(" + (parseInt($(this).attr('data-ul')) + 2) + ")").removeClass('hidden');
        $(".left-main-menu .left-list-link-menu-left").addClass("move-left");

        $("#left-list-link-menu-left-2").scrollTop(0);
    });

    $("#left-list-link-menu-left-2 .left-list-link-menu-left-back").click(function () {
        $(".left-main-menu .left-list-link-menu-left").removeClass("move-left");
        $("#left-list-link-menu-left-1").scrollTop(0);
    });

    /*
    $(window).scroll(function () {

        show_pc_header();

    });*/

});


function show_pc_header() {
    var scroll = $(window).scrollTop();
    if (scroll > 0) {
        $(".section-header").addClass("fixed");
        $("#left_menu_sidebar").addClass("fixed");
    } else {
        $(".section-header").removeClass("fixed");
        $("#left_menu_sidebar").removeClass("fixed");
    }
}
