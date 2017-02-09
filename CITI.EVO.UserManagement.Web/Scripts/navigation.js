$(document).ready(function () {
    $("#btn_show").hide();

    if ($.cookie('header_open') == 'close') {
        $("#header-center").slideToggle("slow", function () { });
        $('#header-center').hide();

        $("#btn_show").slideToggle("slow", function () { });
        $('#btn_show').show();
    }

    $("#btn_up").click(function () {
        $("#header-center").slideToggle("slow", function () {
            $("#btn_show").slideToggle("slow", function () { });

            $.cookie('header_open', 'close');
        });

        return false;
    });


    $(".inner").hide();
    $(".slide").click(function () {
        $(".inner").animate({ width: 'toggle' }, 350);

        return false;
    });


    $("#btn_show").click(function () {
        $("#btn_show").slideToggle("slow", function () {
            $("#header-center").slideToggle("slow", function () { });

            $.cookie('header_open', null);
        });

        return false;
    });

    // for left side
    if ($.cookie('sidebar_left_open') == 'close') {
        $("#leftpanelcell").animate({ marginLeft: '-190px' }, 300);
        $("#centerpanelcell").animate({ marginLeft: '-190px' }, 300);
        $("#sidebar_left").addClass('openleft_c');
    };


    $("#sidebar_left").click(function () {
        if ($.cookie('sidebar_left_open') == null) {
            $("#leftpanelcell").animate({ marginLeft: '-190px' }, 300);
            $("#centerpanelcell").animate({ marginLeft: '-190px' }, 300);
            $("#sidebar_left").addClass('openleft_c');

            $.cookie('sidebar_left_open', 'close');
        }
        else {
            $("#leftpanelcell").animate({ marginLeft: '0' }, 300);
            $("#centerpanelcell").animate({ marginLeft: '0px' }, 300);
            $("#sidebar_left").removeClass('openleft_c');
            $("#sidebar_left").addClass('openleft');
            $.cookie('sidebar_left_open', null);
        }

    });
    


    // Navigation in left side
    var checkCookie = $.cookie("nav-item");
    var checkCookie1 = $.cookie("sub-nav-item");

    if (checkCookie != "") {
        $('#left_nav_menu > ul > li > a:eq(' + checkCookie + ')').addClass('active').next().show();
        $('#left_nav_menu > ul > li > ul > li > a:eq(' + checkCookie1 + ')').addClass('active').next().show();
    }

    $('#left_nav_menu > ul > li > a').click(function () {
        var navIndex = $('#left_nav_menu > ul > li > a').index(this);

        $.cookie("nav-item", navIndex);
        $('#left_nav_menu ul li ul').slideUp();

        if ($(this).next().is(":visible")) {
            $(this).next().slideUp();
        } else {
            $(this).next().slideToggle();
        }

        $('#left_nav_menu ul li a').removeClass('active');
        $(this).addClass('active');
    });

    $('#left_nav_menu > ul > li > ul > li > a').click(function () {
        var navIndex = $('#left_nav_menu > ul > li > ul > li > a').index(this);

        $.cookie("sub-nav-item", navIndex);
        $('#left_nav_menu ul li ul li ul').slideUp();

        if ($(this).next().is(":visible")) {
            $(this).next().slideUp();
        } else {
            $(this).next().slideToggle();
        }

        $('#left_nav_menu ul li ul li a').removeClass('active');
        $(this).addClass('active');
    });

    //For tabs

//    var checkCookie = $.cookie("tab-sheet");
//    var checkCookie1 = $.cookie("tab-sheet1");

//    if (checkCookie != "") {
//        $('.tab > ul > li.tab_sheet > a.tab_sheet_title:eq(' + checkCookie + ')').addClass('active').next().show();
//          $('.tab > ul > li.tab_sheet > a.tab_sheet_title(' + checkCookie + ')').addClass('active').next().show();
//        $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1:eq(' + checkCookie1 + ')').addClass('active').next().show();
//    }

//    $('.tab > ul > li.tab_sheet > a.tab_sheet_title').click(function () {
//        var navIndex = $('.tab > ul > li.tab_sheet > a.tab_sheet_title').index(this);
//        $.cookie("tab-sheet", navIndex);
//        $('.tab ul li.tab_sheet ul').slideUp();
//        if ($(this).next().is(":visible")) {
//            $(this).next().slideUp();
//        } else {
//            $(this).next().slideToggle();
//        }
//        $('.tab ul li.tab_sheet a.tab_sheet_title').removeClass('active');
//        $(this).addClass('active');
//    });

//    $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1').click(function () {
//        var navIndex = $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1').index(this);
//        $.cookie("tab-sheet1", navIndex);
//        $('.tab ul li.tab_sheet ul li.tab_sheet1 ul').slideUp();
//        if ($(this).next().is(":visible")) {
//            $(this).next().slideUp();
//        } else {
//            $(this).next().slideToggle();
//        }
//        $('.tab ul li.tab_sheet ul li.tab_sheet1 a.tab_sheet_title1').removeClass('active');
//        $(this).addClass('active');
//    });
// 
//});

    //For tabs

    var checkCookie = $.cookie("tab-sheet");
    var checkCookie1 = $.cookie("tab-sheet1");

    if (checkCookie != "") {
        $('.tab > ul > li.tab_sheet > a.tab_sheet_title:eq(' + checkCookie + ')').addClass('active').next().show();
        //  $('.tab > ul > li.tab_sheet > a.tab_sheet_title(' + checkCookie + ')').addClass('active').next().show();
        $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1:eq(' + checkCookie1 + ')').addClass('active').next().show();
    }

    $('.tab > ul > li.tab_sheet > a.tab_sheet_title').click(function () {
        var navIndex = $('.tab > ul > li.tab_sheet > a.tab_sheet_title').index(this);
        $.cookie("tab-sheet", navIndex);
        $('.tab ul li.tab_sheet ul').slideUp();
        if ($(this).next().is(":visible")) {
            $(this).next().slideUp();
        } else {
            $(this).next().slideToggle();
        }
        $('.tab ul li.tab_sheet a.tab_sheet_title').removeClass('active');
        $(this).addClass('active');
    });

    $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1').click(function () {
        var navIndex = $('.tab > ul > li.tab_sheet > ul > li.tab_sheet1 > a.tab_sheet_title1').index(this);
        $.cookie("tab-sheet1", navIndex);
        $('.tab ul li.tab_sheet ul li.tab_sheet1 ul').slideUp();
        if ($(this).next().is(":visible")) {
            $(this).next().slideUp();
        } else {
            $(this).next().slideToggle();
        }
        $('.tab ul li.tab_sheet ul li.tab_sheet1 a.tab_sheet_title1').removeClass('active');
        $(this).addClass('active');
    });

});



//For theme
var menuids = ["sidebarmenu1"];  //Enter id(s) of each Side Bar Menu's main UL, separated by commas

function initsidebarmenu() {
    for (var i = 0; i < menuids.length; i++) {
        var ultags = document.getElementById(menuids[i]).getElementsByTagName("ul");
        for (var t = 0; t < ultags.length; t++) {
            ultags[t].parentNode.getElementsByTagName("a")[0].className += " subfolderstyle";
            if (ultags[t].parentNode.parentNode.id == menuids[i]) //if this is a first level submenu
                ultags[t].style.left = ultags[t].parentNode.offsetWidth + "px"; //dynamically position first level submenus to be width of main menu item
            else //else if this is a sub level submenu (ul)
                ultags[t].style.left = ultags[t - 1].getElementsByTagName("a")[0].offsetWidth + "px";  //position menu to the right of menu item that activated it
            ultags[t].parentNode.onmouseover = function () {
                this.getElementsByTagName("ul")[0].style.display = "block";
            }
            ultags[t].parentNode.onmouseout = function () {
                this.getElementsByTagName("ul")[0].style.display = "none";
            }
        }
        for (var t = ultags.length - 1; t > -1; t--) { //loop through all sub menus again, and use "display:none" to hide menus (to prevent possible page scrollbars
            ultags[t].style.visibility = "visible";
            ultags[t].style.display = "none";
        }
    }
}

if (window.addEventListener)
    window.addEventListener("load", initsidebarmenu, false);
else if (window.attachEvent)
    window.attachEvent("onload", initsidebarmenu);