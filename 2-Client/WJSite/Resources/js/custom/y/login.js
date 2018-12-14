var login = function () {
    var username, pwd;

    //赋值
    var getValue = function () {
        username = $('#txtLoginName').val();
        pwd = $('#txtPwd').val();

    }

    //获取验证码
    var getVetificationCode = function () {
        var action = '', spec = '';
        $.ajax({
            url: '/api/common/verification',
            data: { action: action, spec: spec },
            type: 'get',
            dataType: 'json',
            success: function (data) {
                console.log(data);
            },
            error: function (t, s, v) {
                console.log(t);
                console.log(s);
                console.log(v);
            }
        });
    }

    //显示错误信息
    var showWarning = function (tag, msg) {
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
    //隐藏错误信息
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
    //输入校验
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

    //登录逻辑
    var l = function () {
        var v = true && checkInput();
        if (!v) {
            return;
        }

    }

    //
    var showValidatorImg = function (e) {
        $('#verification_panel').show();
        $('#verification_panel').animate({ top: 60 }, 300);
        $("#verification").slide({
            //验证后的回调
            successCallBack: function () {
                $('#verification_panel').hide();
                var content = "<span class='fa fa-check'></span>&nbsp;&nbsp;<span>验证成功</span>";
                $('#btn-verification').html(content);
                $("#btn-verification").attr("disabled", true);
                $("#btn-verification").removeClass("btn-verification");
                $("#btn-verification").addClass("btn-verification-success");
            }
        });
        e.stopPropagation();
    }
    var hideValidator = function () {
        //$('#verification_panel').hide();
        //$('#verification').html();
    }
    return {
        init: function () {
            //getVetificationCode();
            $('#btnLogin').on('click', l);
            $('#txtLoginName').on('focus', hideWarning);
            $('#txtPwd').on('focus', hideWarning);
            $('.btn-verification').on("click", showValidatorImg);
            $(this).on('click', hideValidator);
        }()
    };
}()