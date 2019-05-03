/**

 @Name：ACE-MALL 商城后台
 @Author：张青青
    
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
            { type: 'checkbox', fixed: 'left' }
            , { field: 'OrderNo', title: '订单编号', minWidth: 225, align: 'center' }
            , { field: 'Name', title: '收件人姓名', align: 'center' }
            //, { field: 'progress', title: '进度', width: 200, align: 'center', templet: '#progressTpl' }
            , { field: 'progress', title: '进度', align: 'center' }
            , { field: 'Address', title: '收件人地址', align: 'center' }
            , { field: 'Phone', title: '收件人电话', align: 'center' }
            , { field: 'PayTime', title: '支付时间', align: 'center' }
            , { title: '操作', align: 'left', minWidth: 280, fixed: 'right', toolbar: '#table-system-order'}
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        ,text: {
            none: '暂无相关数据' //默认：无数据。
        }
        //, done: function () {
        //    element.render('progress')
        //}
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
    //快递信息编辑窗口
    var courierDlg = function () {
        var vm1 = new Vue({
            el: "#formCourierDetail"
        });
        var showDetail = function (data) {
            var title = "编辑快递信息";
            var courierLayer=layer.open({
                title: title,
                area: ["500px", "300px"],
                type: 1,
                btn: ['取消'],
                content: $('#divCourierDetail'),
                success: function () {
                //将myform表单中input元素type为button、submit、reset、hidden排除 .val('')
                    $(':input', '#formCourierDetail').not(':button,:submit,:reset,:hidden').val('');
                    vm1.$set('$data', data);
                    form.render();
                },
                end: table.render
            });
            var url = "/Order/UpdateCourier";
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    $.post(url,
                        data.field,
                        function (data) {
                            layer.msg(data.message);
                            layer.close(courierLayer);
                        },
                        "json");
                    return false;
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
      
        if (obj.event === 'orderDetail') {      //查看订单详情
            detailDlg.detail(data);
        }
        if (obj.event === 'goodDetail') {      //查看订单商品列表
            var goodLayer = layer.open({
                title: "查看订单商品列表",
                area: ["1500px", "800px"],
                type: 1,
                btn: ['取消'],
                content: $('#divGoodDetail'),
                success: function () {
                    table.render({
                        elem: '#orderGoodList'
                        , url: '/Order/GetOrderGoodList?id=' + data.OrderNo //模拟接口
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
                            , { field: 'GoodName', title: '商品名称', minWidth: 380, align: 'center' }
                            , { field: 'GoodImage', title: '商品图片', align: 'center', templet: '#img' }
                            , { field: 'PresentPrice', title: '商品现价', align: 'center', align: 'center' }
                            , { field: 'GoodNumber', title: '购买数量', align: 'center' }
                        ]]
                        , page: true
                        , limit: 10
                        , limits: [10, 15, 20, 25, 30]
                        , text: '对不起，加载出现异常！'
                        //, done: function () {
                        //    element.render('progress')
                        //}
                    });
                },
                end: table.render
            });
        }   
        if (obj.event === 'edit') {
            courierDlg.detail(data);
        }
    });
    exports('order', {})
});