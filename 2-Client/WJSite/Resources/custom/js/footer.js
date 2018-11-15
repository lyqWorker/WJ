$.ajax({
    url: "/Views/common/footer.html",
    type: "get",
    dataType: "html",
    success: function (a) {
        $("#footer").html(a);
    }
});
