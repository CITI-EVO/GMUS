
$(document).ready(function () {
    $('#nav').children('li').children('a').click(function () {
        $('#nav').children('li').children('a').next('ul').hide();
        $(this).next('ul').show();
    });

    init();
});


function init() {
    var parentLinks = $('#nav li > ul');

    parentLinks.each(function () {
        var activeLinks = $(this).children('li').children('a.sub_active');

        if (activeLinks.length) {
            $(this).show();
        }
    });
}


//var isLeft = false;

var foldUp = 'foldUp';
var foldDown = 'foldDown';
var commandCookieKey = 'command';

$(document).ready(function () {

    Initialize();

    function Initialize() {

        if (!$.cookie(commandCookieKey)) {
            $.cookie(commandCookieKey, foldUp);
            foldUpFoldDown(foldDown);
            return;
        }

        var command = $.cookie(commandCookieKey) == foldUp ? foldDown : foldUp;
        foldUpFoldDown(command);
    }


    $(".menu_navigation").click(function () {
        var command = $.cookie(commandCookieKey);

        foldUpFoldDown(command);

        $.cookie(commandCookieKey, command == foldUp ? foldDown : foldUp);
    });


    function foldUpFoldDown(command) {


        if (command == foldUp) {

            $("#left_panel").animate({ width: '240px' });

            $(".menu_navigation").css("padding-left", "195px");

            $(".home img").css("width", '105px');

            $(".home ").css("height", '110px');

            $("#content").css("padding", "16px 20px 0 280px");

        } else {

            $("#left_panel").animate({ width: '50px' });

            $(".menu_navigation").css("padding-left", "0");

            $(".menu_navigation img").css("padding-left", "6px");

            $(".home img").css("padding", "20px 0 0 0");

            $(".home img").css("margin-left", "-15px");

            $(".home img").css("transition", "width 0.25s ease 0s");

            $(".home img").css("width", '35px');

            $(".home ").css("height", '70px');

            $("#content").css("padding", "16px 20px 0 80px");

        }
    }


});



