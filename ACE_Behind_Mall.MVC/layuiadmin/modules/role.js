/**

 @Name：layuiAdmin 订单系统
 @Author：gala-qing
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
            , { title: '操作', align: 'center', fixed: 'right', toolbar: '#table-system-role' }
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
    //监听表格内部按钮
    table.on('tool(roleList)', function (obj) {
        var data = obj.data;
        if (obj.event === 'rolePower') {      //为角色分配权限
            //window.location.href = "/Role/AssignPower?roleId=" + data.ID;
            var tr = $(obj.tr);
            layer.open({
                type: 2
                , title: '为角色分配权限'
                , content: "/Role/AssignPower?roleId=" + data.ID
               // , content: '/Role/AssignPower'
                , area: ['750px', '750px']
                , btn: [ '取消']
                //, yes: function (index, layero) {
                //    var iframeWindow = window['layui-layer-iframe' + index]
                //        , submitID = 'LAY-user-back-submit'
                //        , submit = layero.find('iframe').contents().find('#' + submitID);

                //    //监听提交
                //    iframeWindow.layui.form.on('submit(' + submitID + ')', function (data) {
                //        var field = data.field; //获取提交的字段

                //        //提交 Ajax 成功后，静态更新表格中的数据
                //        //$.ajax({});
                //        table.reload('LAY-user-front-submit'); //数据刷新
                //        layer.close(index); //关闭弹层
                //    });

                //    submit.trigger('click');
                //}
                //, success: function (layero, index) {

                //}
            })
        }
    });
    exports('role', {})
});