layui.config({
    base: "/js/"
}).use(['form', 'layer'], function () {

    if (self != top) {
        //如果在iframe中，则跳转
        top.location.replace("/Login/Login");
      
    }

    var form = layui.form,
		layer = parent.layer === undefined ? layui.layer : parent.layer,
		$ = layui.jquery;

    // Cloud Float...
    var $main = $cloud = mainwidth = null;
    var offset1 = 450;
    var offset2 = 0;
    var offsetbg = 0;

    $(document).ready(
        function () {
            $main = $("#mainBody");
            $body = $("body");
            $cloud1 = $("#cloud1");
            $cloud2 = $("#cloud2");

            mainwidth = $main.outerWidth();

        }
    );

    setInterval(function flutter() {
        if (offset1 >= mainwidth) {
            offset1 = -580;
        }

        if (offset2 >= mainwidth) {
            offset2 = -580;
        }

        offset1 += 1.1;
        offset2 += 1;
        $cloud1.css("background-position", offset1 + "px 100px")

        $cloud2.css("background-position", offset2 + "px 460px")
    }, 70);
    setInterval(function bg() {
        if (offsetbg >= mainwidth) {
            offsetbg = -580;
        }

        offsetbg += 0.9;
        $body.css("background-position", -offsetbg + "px 0")
    }, 90);
    $(function () {
        //得到焦点
        $("#password").focus(function () {
            $("#left_hand").animate({
                left: "150",
                top: " -38"
            }, {
                step: function () {
                    if (parseInt($("#left_hand").css("left")) > 140) {
                        $("#left_hand").attr("class", "left_hand");
                    }
                }
            }, 2000);
            $("#right_hand").animate({
                right: "-64",
                top: "-38px"
            }, {
                step: function () {
                    if (parseInt($("#right_hand").css("right")) > -70) {
                        $("#right_hand").attr("class", "right_hand");
                    }
                }
            }, 2000);
        });
        //失去焦点
        $("#password").blur(function () {
            $("#left_hand").attr("class", "initial_left_hand");
            $("#left_hand").attr("style", "left:100px;top:-12px;");
            $("#right_hand").attr("class", "initial_right_hand");
            $("#right_hand").attr("style", "right:-112px;top:-12px");
        });

        $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
        $(window).resize(function () {
            $('.loginbox').css({ 'position': 'absolute', 'left': ($(window).width() - 692) / 2 });
        })
    });
    $('input').on('keydown', function (event) {
        if (event.keyCode == 13) {
            $("#login").click();
        }
    });
    //登录按钮事件
    form.on("submit(login)", function (data) {
        $.getJSON("/Login/LoginIndex"
            , data.field
            , function (data) {
                if (data.result == true) {
                    layer.msg(data.message);
                    window.location.href = "/Login/Index";
                } else {
                    layer.msg(data.message);
                }
            });
        return false;
    })

})
