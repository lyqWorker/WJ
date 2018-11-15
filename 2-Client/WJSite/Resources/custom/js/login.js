var login = function () {
    var username, pwd;

    //赋值
    var getValue = function () {
        username = $('#txtLoginName').val();
        pwd = $('#txtPwd').val();

    }

    var showWarning = function (tag,msg) {
        switch (tag) {
            case 'username': {
                $('.sp_loginname').addClass('warning_show');
                $('.input_loginname>.input').addClass('warning_border');
                $('.sp_loginname>.warning_text').text(msg);
                break;
            }
            case 'pwd': {
                $('.sp_pwd').addClass('warning_show');
                $('.input_pwd>.input').addClass('warning_border');
                $('.sp_pwd>.warning_text').text(msg);
                break;
            }
            default:
                break;
        }
    }
    var hideWarning = function () {
        var id = $(this)[0].id;
        switch (id) {
            case 'txtLoginName': {
                $('.sp_loginname').removeClass('warning_show');
                $('.input_loginname>.input').removeClass('warning_border');
                break;
            }
            case 'txtPwd': {
                $('.sp_pwd').removeClass('warning_show');
                $('.input_pwd>.input').removeClass('warning_border');
                break;
            }
            default:
                break;
        }
    }

    var checkInput = function () {
        getValue();
        if (username.length == 0) {
            showWarning("username", "帐号不能为空");
            return false;
        }
        if (pwd.length == 0) {
            showWarning("pwd", "密码不能为空");
            return false;
        }
    }

    var l = function () {
        var v = true && checkInput();
        if (!v) {
            return;
        }

    }
   



    return {
        init: function () {
            $('#btnLogin').on('click', l);
            $('#txtLoginName').on('focus', hideWarning);
            $('#txtPwd').on('focus', hideWarning);
        }()
    };
}()