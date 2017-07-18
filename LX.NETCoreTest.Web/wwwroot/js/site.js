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
        }
    }
}

var LXPyStudio = new PyStudio();
LXPyStudio.bindSubmitBtn();