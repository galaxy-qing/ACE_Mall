/**

 @Name：layuiAdmin 商品评价
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：GPL-2
    
 */


layui.define(['table', 'form', 'vue', 'element', 'jquery'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;
    table.render({
        elem: '#evaluationList'
        , url: '/Evaluation/GetEvaluationList' //模拟接口
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
            , { field: 'UserName', title: '用户姓名', minWidth: 225, sort: true, align: 'center' }
            , { field: 'GoodName', title: '商品名称', align: 'center' }
            , { field: 'Star', title: '评价级别', align: 'center', templet: '#progressTpl', align: 'center' }
            , { field: 'Evaluation', title: '评价', align: 'center' }
            , { field: 'CreateTime', title: '添加时间', align: 'center' }
            //, { title: '操作', align: 'left', minWidth: 280, fixed: 'right', toolbar: '#table-system-evaluation' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: {
            none: '暂无相关数据' //默认：无数据。
        }
        , done: function () {
            element.render('progress')
        }
    });
    //监听表格内部按钮
    //table.on('tool(orderList)', function (obj) {
    //    var data = obj.data;
    //    if (obj.event === 'orderDetail') {      //查看
    //        detailDlg.detail(data);
    //    }
    //    if (obj.event === 'edit') {
    //        courierDlg.detail(data);
    //    }
    //});
    exports('evaluation', {})
});