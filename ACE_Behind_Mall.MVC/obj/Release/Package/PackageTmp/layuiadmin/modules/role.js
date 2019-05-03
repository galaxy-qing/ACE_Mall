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

    var tableIns=table.render({
        elem: '#roleList'
        , toolbar: '#toolbarDemo'
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
            , { field: 'ID', title: '角色ID', minWidth: 225, sort: true, align: 'center' }
            , { field: 'Name', title: '角色名称', align: 'center', edit: 'roleName' }
            , { field: 'Describe', title: '角色描述', align: 'center', edit: 'roleDescribe' }
            , { field: 'CreateTime', title: '创建时间', sort: true, align: 'center' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: {
            none: '暂无相关数据' //默认：无数据。
        }
    });
    //添加
    var addlDlg = function () {
        var showDetail = function (data) {
            var title = "添加角色";
            var addlayer = layer.open({
                title: title,
                area: ["500px", "300px"],
                type: 1,
                btn: ['取消'],
                content: $('#divAdd'),
                success: function () {
                },
                end: tableIns
            });
            var url = "/Role/Add";
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    $.post(url,
                        data.field,
                        function (data) {
                            layer.msg(data.message);
                            layer.close(addlayer);
                            tableIns.reload();
                        },
                        "json");
                    return false;
                });

        }
        return {
            add: function () { //弹出添加
                showDetail({
                    ID: ''
                });
            }
        };
    }();
    //监听单元格编辑
    table.on('edit(roleName)', function (obj) {
        var value = obj.value //得到修改后的值
            , datas = obj.data //得到所在行所有键值
            , field = obj.field; //得到字段
        var url = "/Role/UpdateRoleName";
        if (obj.value != '') {
            $.post(url, datas, function (data) {
                layer.msg(data.message);
                layer.closeAll();
            }, "json");
            return false;
        }
    });
    table.on('edit(roleDescribe)', function (obj) {
        var value = obj.value //得到修改后的值
            , datas = obj.data //得到所在行所有键值
            , field = obj.field; //得到字段
        var url = "/Role/UpdateDescribeName";
        if (obj.value != '') {
            $.post(url, datas, function (data) {
                layer.msg(data.message);
                layer.closeAll();
            }, "json");
            return false;
        }
    });
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    //事件
    var active = {
        delete: function () {
            var checkStatus = table.checkStatus('roleList')
                , data = checkStatus.data; //得到选中的数据
            if (data.length === 0) {
                return layer.msg('请选择数据');
            }
            if (data.length > 1) {
                return layer.msg('一次只能删除一条数据');
            }
            layer.confirm('确定删除该角色及该角色下的员工吗？', function (index) {
                var url = "/Role/Delete";
                $.post(url, data[0], function (data) {
                    layer.msg(data.message);
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    tableIns.reload();
                }, "json");
                return false;
            });
        }
        , add: function () {  //添加
            addlDlg.add();
        }
    };
    exports('role', {})
});