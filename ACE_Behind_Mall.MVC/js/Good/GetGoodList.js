    layui.config({
        base: '../../layuiadmin/' //静态资源所在路径
        }).extend({
        index: 'lib/index' //主入口模块
}).use(['index']);
        layui.use(['laypage', 'layer'], function () {
            var laypage = layui.laypage
        , layer = layui.layer;
            //监听搜索
            form.on('submit(btnSearch)', function (data) {
             var field = data.field;

                //执行重载
                //table.reload('orderList', {
                //    where: field
                //});
            });
    //总页数低于页码总数
            laypage.render({
        elem: 'demo0'
    , count: 50 //数据总数
});
});