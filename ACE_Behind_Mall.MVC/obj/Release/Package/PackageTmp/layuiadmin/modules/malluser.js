/**

 @Name：ACE-MALL 商城后台
 @Author：张青青
    
 */


layui.define(['table', 'form', 'vue', 'element', 'jquery'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;

    table.render({
        elem: '#userList'
        , url: '/User/GetUserList' //模拟接口
        , parseData: function (res) {
            console.log(res)
            return {
                "code": res.status,//解析接口状态
                "msg": res.message,//解析提示文本
                "count": res.total,//解析数据长度
                "data": res.data//解析数据列表
            };
        }
        , cols: [[
            { type: 'checkbox', fixed: 'left' }
            , { field: 'Email', title: '邮箱', align: 'center' }
            , { field: 'Image', title: '头像', align: 'center', templet: '#img' }
            , { field: 'Account', title: '账户', align: 'center' }
            , { field: 'ReceiveName', title: '收货人姓名', align: 'center', sort: true, }
            , { field: 'ReceiveAddress', title: '收货人地址', align: 'center' }
            , { field: 'ReceivePhone', title: '收货人电话', sort: true, align: 'center' }
            , { field: 'CreateTime', title: '添加时间', sort: true, align: 'center' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: {
            none: '暂无相关数据' //默认：无数据。
        }
    });
    exports('malluser', {})
});