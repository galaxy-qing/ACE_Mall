/**

 @Name：layuiAdmin 订单系统
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
        elem: '#roleList'
        , url: '/Role/GetRoleList' //模拟接口
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
            ,{ field: 'ID', title: '角色ID', minWidth: 225, sort: true, align: 'center' }
            , { field: 'Name', title: '角色名称', align: 'center' }
            , { field: 'Describe', title: '角色描述', align: 'center' }
            , { field: 'CreateTime', title: '创建时间', align: 'center' }
            , { title: '操作', align: 'left', minWidth: 280, fixed: 'right', toolbar: '#table-system-order' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: '对不起，加载出现异常！'
    });
    //监听表格内部按钮
    table.on('tool(roleList)', function (obj) {
        var data = obj.data;

        if (obj.event === 'orderDetail') {      //查看订单详情
            detailDlg.detail(data);
        }
        if (obj.event === 'edit') {
            courierDlg.detail(data);
        }
    });
    exports('role', {})
});