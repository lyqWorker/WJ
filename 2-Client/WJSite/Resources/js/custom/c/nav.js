
var nav = function () {
    // nav收缩展开
    var openMenu = function () {
        if (!$('.nav').hasClass('nav-mini')) {
            if ($(this).next().css('display') == "none") {
                //展开未展开
                $('.nav-item').children('ul').slideUp(300);
                $(this).next('ul').slideDown(300);
                $(this).parent('li').addClass('nav-show').siblings('li').removeClass('nav-show');
            } else {
                //收缩已展开
                $(this).next('ul').slideUp(300);
                $('.nav-item.nav-show').removeClass('nav-show');
            }
        }
    }
    //nav-mini切换
    var updateNavMin = function () {
        if (!$('.nav').hasClass('nav-mini')) {
            $('.nav-item.nav-show').removeClass('nav-show');
            $('.nav-item').children('ul').removeAttr('style');
            $('.nav').addClass('nav-mini');
            $('#content').removeClass('main_content_normal').addClass('main_content_big');
        } else {
            $('.nav').removeClass('nav-mini');
            $('#content').removeClass('main_content_big').addClass('main_content_normal');
        }
    }
    return {
        init: function() {
            $('.nav-item>a').on('click', openMenu);
            $('#mini').on('click', updateNavMin);
        }()
    };
}()