/**

 @Name：layuiAdmin 工单系统
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：GPL-2
    
 */


layui.define(['table', 'form','vue', 'element', 'jquery'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;

    table.render({
        elem: '#orderList'
        , url: '/Order/GetOrderList' //模拟接口
        , parseData: function (res) {
            return {
                "code": res.status,//解析接口状态
                "msg": res.message,//解析提示文本
                "count": res.total,//解析数据长度
                "data": res.data//解析数据列表
            };
        }
        , cols: [[
            { type: 'numbers', fixed: 'left', align: 'center' }
            , { field: 'OrderNo', title: '订单编号', minWidth: 225, sort: true, align: 'center' }
            , { field: 'Name', title: '收件人姓名',  align: 'center' }
            , { field: 'Progress', title: '进度', align: 'center', templet: '#progressTpl', align: 'center' }
            , { field: 'Address', title: '收件人地址', align: 'center' }
            , { field: 'Phone', title: '收件人电话', align: 'center' }
            , { field: 'PayTime', title: '支付时间', align: 'center' }
            , { title: '操作', align: 'center', minWidth: 280, fixed: 'right', toolbar: '#table-system-order', align: 'center' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: '对不起，加载出现异常！'
        , done: function () {
            element.render('progress')
        }
    });
    //查看订单信息信息
    var detailDlg = function () {
        var vm = new Vue({
            el: "#formOrderDetail"
        });
        var showDetail = function (data) {
            var title = "查看订单";
            layer.open({
                title: title,
                area: ["1000px", "800px"],
                type: 1,
                btn: ['取消'],
                content: $('#divOrderDetail'),
                success: function () {
                    vm.$set('$data', data);
                    form.render();
                    //tableIns.reload();
                },
                end: table.render
            });
        }
        return {
            detail: function (data) { //查看编辑框
                look = true;
                showDetail(data);
            }
        };
    }();
    //监听表格内部按钮
    table.on('tool(orderList)', function (obj) {
        var data = obj.data;
        if (obj.event === 'orderDetail') {      //查看
            detailDlg.detail(data);
        }
        if (obj.event === 'edit') {
            detailDlg.update(data);
        }
    });
    //监听工具条
    //table.on('tool(orderList)', function (obj) {
    //    var data = obj.data;
    //    if (obj.event === 'orderdetail') {
    //        var tr = $(obj.tr);
    //        alert(JSON.stringify(obj.data));
    //        var vm = new Vue({
    //            el: "#formOrderDetail"
    //        });
    //        layer.open({
    //            type: 2
    //            , title: '查看订单详情'
    //            , content: $('#divOrderDetail')
    //            , area: ['1000px', '800px']
    //            , btn: ['去发货', '取消']
    //            , yes: function (index, layero) {
    //                var iframeWindow = window['layui-layer-iframe' + index]
    //                    , submitID = 'LAY-app-workorder-submit'
    //                    , submit = layero.find('iframe').contents().find('#' + submitID);

    //                //监听提交
    //                iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
    //                    var field = data.field; //获取提交的字段

    //                    //提交 Ajax 成功后，静态更新表格中的数据
    //                    //$.ajax({});
    //                    table.reload('LAY-user-front-submit'); //数据刷新
    //                    layer.close(index); //关闭弹层
    //                });

    //                submit.trigger('click');
    //            }
    //            , success: function (layero, index) {
    //                vm.$set('$data', data);
    //            }
    //        });
    //    }
    //});
    exports('order', {})
});