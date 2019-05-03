layui.config({
    base: '../../layuiadmin/' //静态资源所在路径
}).extend({
    index: 'lib/index' //主入口模块
}).use(['index']);
layui.use(['laypage', 'layer', 'form', 'jquery', 'vue', 'element'], function () {
    var laypage = layui.laypage
        , form = layui.form
        , element = layui.element
        , $ = layui.jquery
        , layer = layui.layer,
        idList = [];
    form.on('checkbox(checkGoods)', function (data) {
        var id = $(this).data('id')
        if (idList.indexOf(id) > -1) {
            idList.splice(idList.indexOf(id), 1)
        } else {
            idList.push(id)
        }
    });
    var active = {
        addGood: function () {
            window.location.href = "/Good/GoodDetail/";
        }
        ,offShelves: function () {
            layer.confirm('确定下架吗？', function (index) {
                var url = "/Good/OffShelves";
                $.post(url,
                    { idList: idList },
                    function (data) {
                        layer.close(index); //如果设定了yes回调，需进行手工关闭
                        window.location.reload();//刷新当前页面.
                    }, "json");
                return false;
            });
        }
        , onShelves: function () {
            layer.confirm('确定上架吗？', function (index) {
                var url = "/Good/OnShelves";
                $.post(url,
                    { idList: idList },
                    function (data) {
                        layer.close(index); //如果设定了yes回调，需进行手工关闭
                        window.location.reload();//刷新当前页面.
                    }, "json");
                return false;
            });
        }
        , reload: function () {
            var url = "/Good/GoodList";
            $.post(url,
                { goodName: $("#inputReload").val() },
                function (data) {
                    console.log('-----');
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    window.location.reload();//刷新当前页面.
                }, "json");
            return false;
        }
    };

    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    $('.cmdlist-container').on('click', function () {
        var id = $(this).data('id');
        window.location.href = "/Good/GoodDetail?id="+id;

    });
    //总页数低于页码总数
    //laypage.render({
    //    elem: '#goodsDiv'
    //    , count: 50 //数据总数
    //});
});