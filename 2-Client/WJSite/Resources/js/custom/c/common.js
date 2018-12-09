$(function () {
    $.ajax({
        url: "/Views/common/footer.html",
        type: "get",
        dataType: "html",
        success: function (a) {
            $("#footer").html(a);
        }
    });
    $.ajax({
        url: "/Views/common/header.html",
        type: "get",
        dataType: "html",
        success: function (a) {
            $("#header").html(a);
        }
    });
    $.ajax({
        url: "/Views/common/menu.html",
        type: "get",
        dataType: "html",
        success: function (a) {
            $("#menu").html(a);
        }
    });
});


