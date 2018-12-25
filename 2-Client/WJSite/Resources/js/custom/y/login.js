var login = function () {
    var username, pwd, isValidator = false, guid;
    //赋值
    var getValue = function () {
        username = $('#txtLoginName').val();
        pwd = $('#txtPwd').val();
        guid = $('#guid').text();
    }
    //显示错误信息
    var showWarning = function (tag, msg) {
        switch (tag) {
            case 'user':
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
            case 'validator': {
                $('.sp_validator').addClass('warning_show');
                $('.sp_validator>.warning_text').text(msg);
                var content = "<span class='fa fa-remove'></span>&nbsp;&nbsp;<span>" + msg+"</span>";
                $('#btn-verification').html(content);
                $("#btn-verification").removeClass("btn-verification").removeClass("btn-verification-success");
                $("#btn-verification").addClass("btn-verification-fail");
                setTimeout(function () {
                    $("#btn-verification").attr("disabled", false);
                    $('.sp_validator').removeClass('warning_show');
                    $("#btn-verification").removeClass("btn-verification").removeClass("btn-verification-fail").addClass("btn-verification");
                    var content = "<div id='shield'></div><span>点击完成验证</span>";
                    $('#btn-verification').html(content);
                },1200);
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
        if (!isValidator || guid.length == 0) {
            showWarning("validator", "验证失败");
            return false;
        }
        return true;
    }
    //登录逻辑
    var l = function () {
        var v = true && checkInput();
        if (!v) {
            return;
        }
        var uno = username + "|" + guid;
        $.ajax({
            url: '/api/user/login',
            type: 'post',
            data: {
                UNO: uno,
                PW: pwd,
            },
            dataType: 'json',
            success: function (res) {
                if (res.State === 1) {
                    window.location.href = res.Msg;
                }
                else {
                    showWarning(res.ErrorItem, res.Msg);
                }
            },
            error: function () {

            },
            beforeSend: function () {
                $('#btnLogin').button('loading');
            },
            complete: function () {
                $('#btnLogin').button('reset');
            }
        });
    }

    var changeShild = function (e) {
        if (e.type == "mouseover") {
            $("#shield").attr('src', 'Resources/image/shieldbule.png'); 
        } else if (e.type == "mouseout") {
            $("#shield").attr('src', 'Resources/image/shield.png'); 
        }
    }

    //
    var showValidatorImg = function (e) {
        $('.sp_validator').removeClass('warning_show');
        $('#verification_panel').show();
        $('#verification_panel').animate({ top: 60 }, 300);
        $("#verification").slide({
            //验证后的回调
            successCallBack: function () {
                isValidator = true;
                //样式修改
                //guid = data;
                $('#verification_panel').hide();
                var content = "<span class='fa fa-check'></span>&nbsp;&nbsp;<span>验证成功</span>";
                $('#btn-verification').html(content);
                $("#btn-verification").attr("disabled", true);
                $("#btn-verification").removeClass("btn-verification")
                                      .removeClass("btn-verification-fail")
                                      .addClass("btn-verification-success");
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
            $('#guid').hide();
            $('#btnLogin').on('click', l);
            $('#txtLoginName').on('focus', hideWarning);
            $('#txtPwd').on('focus', hideWarning);
            $('#btn-verification').on("click", showValidatorImg);
            $('#btn-verification').on("mouseover mouseout", changeShild);
            $(this).on('click', hideValidator);
        }()
    };
}()