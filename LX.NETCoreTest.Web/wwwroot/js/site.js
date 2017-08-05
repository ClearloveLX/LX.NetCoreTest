var PyStudio = function () {
    var now = new Date();

    return {
        bindSubmitBtn: function () {
            $("input[name='btnSubmit']").on("click", function () {

                var _btn = $(this);
                _btn.addClass("hide");
                var _msg = $("#msgbox");
                _msg.html("提交中，请稍后...");

                var _form = $("form[name='form_submit']");
                if (_form.valid()) {
                    _form.submit();
                } else {
                    _btn.removeClass("hide");
                    _msg.html("");
                }
            });
        },
        getUserInfo: function () {
            $.post("/Member/GetLoginInfo?t=" + now.getTime(), function (data) {
                console.log(data);
                var loginArr = [];
                if (data) {
                    if (data.isOk && data.data !== null) {
                        var user = data.data;
                        loginArr.push('<li><a href="/UserCenter/Index">你好<span style="color:red">' + user.nickName + '</span></a></li>');
                        loginArr.push('<li><a href="/UserCenter/Index">个人中心</a></li>');
                        loginArr.push('<li><a href="/Member/Loginout">注销</a></li>');
                    }
                }
                if (loginArr.length > 0) {
                    $("#ul_login").html(loginArr.join(''));
                }
            });
        },
        getUserLog: function (tabId, codeId, page, pageSize) {
            if (tabId.length <= 0 || codeId.length <= 0) {
                return;
            }

            $.post("/UserCenter/UserLog", { codeId: codeId, page: page, pageSize: pageSize }, function (data) {
                console.log(data);
                if (data) {
                    if (!data.isOk) {
                        $("#" + tabId + " tbody").html('<tr><td>获取失败，请稍后重试</td></tr>');
                        return;
                    }

                    var trArr = [];
                    $.each(data.data, function (i, item) {
                        trArr.push('<tr><td>' + item.des + '</td></tr>');
                    });

                    if (trArr.length > 0) {
                        $("#" + tabId + " tbody").html(trArr.join(''));
                    } else {
                        $("#" + tabId + " tbody").html('<tr><td>暂无</td></tr>');
                    }
                }
            });
        },
        getUserStatis: function (tabId) {
            if (tabId.length <= 0) {
                return;
            }

            $.post("/UserCenter/UserStatis", { x: 520 }, function (data) {
                console.log(data);
                if (data) {
                    if (!data.isOk) {
                        $("#" + tabId + " tbody").html('<tr><td colspan="2">获取失败，稍后重试</td></tr>');
                        return;
                    }

                    var trArr = [];
                    $.each(data.data, function (i, item) {
                        trArr.push('<tr><td>' + item.name + '</td><td>' + item.total + '</td></tr>');
                    });

                    if (trArr.length > 0) {
                        $("#" + tabId + " tbody").html(trArr.join(''));
                    } else {
                        $("#" + tabId + " tbody").html('<tr><td>暂无</td></tr>');
                    }
                }
            });
        },
        getUserUp: function (tabId) {
            if (tabId.length <= 0) { return; }
            $.post("/UserCenter/UserUp", { x: 520 }, function (data) {
                console.log(data);
                if (data) {
                    if (!data.isOk) { $("#" + tabId + " tbody").html('<tr><td>获取失败，稍后重试</td></tr>'); return; }
                    var trArr = [];
                    $.each(data.data, function (i, item) {

                        trArr.push('<tr><td><a href="/Pictures/PicView/' + item.id + '" target="_blank">' + item.name + '【浏览：' + item.readNum + '】</a></td></tr>');
                    });
                    if (trArr.length > 0) {
                        $("#" + tabId + " tbody").html(trArr.join(''));
                    } else {
                        $("#" + tabId + " tbody").html('<tr><td>暂无</td></tr>');
                    }
                }
            });
        }
    }
}

var LXPyStudio = new PyStudio();
//提交按钮
LXPyStudio.bindSubmitBtn();
//用户信息
LXPyStudio.getUserInfo();