var Register = function () {

    var wait = 120;
    var userName, pw, pw2, code, invate;

    var usernametips = $("#usernametips");
    var pwtips = $("#pwtips");
    var pwtips2 = $("#pwtips2");
    var agreetips = $("#agreetips");
    var yzmtips2 = $("#yzmtips2");

    var showImg = function () {
        // 载入随机验证码
        var url = "/api/user/getvalidateimg"
        $.ajax({
            url: url,
            type: 'get',
            cache: false,
            dataType: 'json',
            success: function (data) {
                $("#enCode2").val(data[0]);
                $("#vimg").attr('src', data[1]);
            },
            error: function (xhr, status, error) {
                var errorText = xhr.status + status + error;
                console.log(errorText);
            }
        });
    }

    var checkUserName = function () {
        if (userName == "") {
            usernametips.css("display", "");
            return false;
        }
        var regexp = /^(1[3|4|5|7|8]\d{9})$/
        if (!regexp.test(userName)) {
            usernametips.css("display", "");
            return false;
        }

        if (userName.length > 50) {
            usernametips.css("display", "");
            return false;
        }

        return true;
    }

    var checkPw = function (v, tips) {
        if (v == "") {
            tips.css("display", "");
            return false;
        }
        if (v.length < 6 || v.length > 12) {
            tips.css("display", "");
            tips.text("密码长度必须在6到12位");
            return false;
        }
        var regexp2 = /^[a-zA-Z0-9]{6,12}$/
        if (!regexp2.test(v)) {
            tips.css("display", "");
            tips.text("密码中不能出现数字和英文字母以外的字符！");
            return false;
        }
        if (pw != pw2) {
            pwtips2.css("display", "");
            return false;
        }

        return true;
    }

    var updateTips = function () {
        usernametips.css("display", "none");
        pwtips.css("display", "none");
        pwtips2.css("display", "none");
        agreetips.css("display", "none");
        yzmtips2.css("display", "none");

    }

    var checkCode = function () {
        if (code == "" || code.length > 8) {
            return false;
        }
        return true;
    }

    var getValue = function () {
        userName = $("#username").val();
        pw = $("#pw").val();
        pw2 = $("#pw2").val();
        code = $("#code").val();
        invate = $("#invate").val();
    }

    var SendMsg = function () {
        getValue();
        var bValid = true;

        bValid = bValid && checkUserName();
        if (bValid == false) {
            return;
        }
        bValid = bValid && checkPw(pw, pwtips);
        if (bValid == false) {
            return;
        }
        bValid = bValid && checkPw(pw2, pwtips2);
        if (bValid == false) {
            return;
        }
        if ($('#code2').val().length != 4) {
            yzmtips2.css("display", "");
            yzmtips2.text("请输入4位验证码");
            return;
        }
        // 调用后台检查用户名，密码是否正确
        //================
        if (bValid) {
            var entryno2 = $("#code2").val();
            var encode2 = $("#enCode2").val();
            var url2 = "/api/user/checkCodeForReg?entryno=" + entryno2 + "&encode=" + escape(encode2) + "&to=" + userName;
            $.ajax({
                url: url2,
                type: 'get',
                cache: false,
                dataType: 'text',
                success: function (data) {
                    if (data != "Error") {
                        $('#enCode').val(data);
                        $("#username").attr("readonly", "readonly");
                        $("#pw").attr("readonly", "readonly");
                        $("#pw2").attr("readonly", "readonly");
                        //$("#invate").attr("readonly", "readonly");
                        updateTips();
                        SendTime($("#btnSendMsg"));
                    }
                    else {
                        showImg();
                        yzmtips2.css("display", "");
                    }
                },
                error: function (xhr, status, error) {
                    var errorText = xhr.status + status + error;
                    showImg();
                    return false;
                }
            });
        }
    }

    var Reg = function () {

        getValue();
        var bValid = true;
        bValid = bValid && checkUserName();
        bValid = bValid && checkPw(pw, pwtips);
        bValid = bValid && checkPw(pw2, pwtips2);
        bValid = bValid && checkCode();

        if (bValid == false) {
            return;
        }
        var invate = $("#invate").val();
        if (invate.length != 8 && invate.length != 0) {
            $("#yqmtips").css("display", "");
            return;
        }

        var isAgree = $("#acceptAggrement");
        if (isAgree[0].checked == false) {
            $("#agreetips").css("display", "");
            return;
        }
        $('#theOverLay').attr('style', "visibility:visible");

        // 调用后台检查用户名，密码是否正确
        //================
        if (bValid) {
            var url = "/api/user/reg?uid=" + userName + "&pw=" + pw + "&code=" + code + "&invate=" + invate + "&encode=" + escape($("#enCode").val());
            $.ajax({
                type: 'POST',
                url: url,
                //data:data
                contentType: 'application/json; charset=utf-8',
                dataType: 'text',
                success: function (data) {

                    datas = data.split(":");

                    if (datas[0] == "Ok") {

                        window.location.href = "Views/user/regsuccess.html?uid=" + datas[1] + "&userName=" + userName;
                    }
                    else {
                        updateTips();
                        if (data == "无效的邀请码") {
                            $("#yqmtips").css("display", "");
                            $("#yqmtips").text("无效的邀请码");
                        }
                        else if (data == "账号不符合规范" || data == "该邮箱已在云景任务网注册！" || data == "该手机已在云景任务网注册！") {
                            $("#usernametips").css("display", "");
                            $("#usernametips").text(data);
                        }
                        else {
                            $("#yzmtips").css("display", "");
                            $("#yzmtips").text(data);
                        }

                    }
                    $('#theOverLay').attr('style', "visibility:hidden");
                },
                error: function (xhr, status, error) {
                    var errorText = xhr.status + "\r\n" + status + "\r\n" + error;
                    updateTips();
                    $('#theOverLay').attr('style', "visibility:hidden");
                }
            });
            updateTips();
        }

    }

    var DoKey = function (e) {
        try {
            var event = e ? e : window.event;
            var el_keydown = event.srcElement ? event.srcElement : event.target;

            if (event.keyCode == 13 && el_keydown.id == "username") {
                document.getElementById("pw").focus();
            }
            else if (event.keyCode == 13 && el_keydown.id == "pw") {
                document.getElementById("pw2").focus();
            }
            else if (event.keyCode == 13 && el_keydown.id == "pw2") {
                document.getElementById("code2").focus();
            }
            else if (event.keyCode == 13 && el_keydown.id == "code2") {
                //document.getElementById("btnSendMsg").focus();
            }
        }
        catch (e) {
            //alert(e);
            return;
        }
    }

    var SendTime = function (o) {
        if (wait == 0) {
            //o.removeAttribute("disabled");
            o.attr("disabled", false);
            o.text("获取验证码");
            $("#username").removeAttr("readonly");
            $("#pw").removeAttr("readonly");
            $("#pw2").removeAttr("readonly");
            //$("#invate").removeAttr("readonly");
            wait = 120;
            return;
        } else {
            //o.setAttribute("disabled", true);
            o.attr("disabled", true);
            o.text(wait + "秒后可重新获取");
            wait--;
            setTimeout(function () {
                SendTime(o)
            }, 1000)
        }
    }

    return {
        init: function () {

            showImg();

            $("#vimg").on("click", showImg);

            $("#btnSendMsg").on("click", SendMsg);
            $("#btnReg").on("click", Reg);
            $(document).on("keydown", DoKey);
            $("#username").on("focus", updateTips);
            $("#pw").on("focus", updateTips);
            $("#pw2").on("focus", updateTips);
        }()
    };
}()