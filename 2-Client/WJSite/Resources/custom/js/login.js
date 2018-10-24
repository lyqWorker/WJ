var login = function () {
    var username, pwd, code;


    var checkNetwork = function () {
        $.ajax({
            url: '/api/common/message',
            type: 'get',
            dataType: 'text',
            success: function (data) {
                console.log(data);
            },
            error: function () {

            }
        });
    };








    return {
        init: function () {
            checkNetwork();
        }()
    };
}()