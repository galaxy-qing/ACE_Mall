layui.config({
    base: "/layuiadmin/layui/lay/modules/"
}).use(['form', 'table', 'jquery', 'laydate', 'vue', 'layer', 'upload'], function () {
    var form = layui.form,
        layer = layui.layer,
        upload = layui.upload,
        $ = layui.jquery;
    var laydate = layui.laydate;
    var table = layui.table;  
    //加载表格
    var tableIns = table.render({
        elem: '#demo'
        , toolbar: '#toolbarDemo'
        , height: 'full-200' //高度最大化减去差值
        , url: '/Category/GetCategoryList'//数据接口
        , done: function (res, curr, count) {
        }
        , parseData: function (res) {
            return {
                "code": res.status,//解析接口状态
                "msg": res.message,//解析提示文本
                "count": res.total,//解析数据长度
                "data": res.data//解析数据列表
            };
        }
        , page: true
        , cellMinWidth: 80
        , cols: [[
            { type: 'checkbox', fixed: 'left' }
            , { field: 'Name', title: '分类名称', align: 'center', edit: 'text' }
            , { field: 'CreateTime', title: '添加时间', align: 'center' }
            , { field: 'IsDelete', title: '是否显示', align: 'center', templet: '#IsDelete', sort: true }
        ]]
        , id: 'idTest'
    });
    var $ = layui.$, active = {
        reload: function () {
            //执行重载
            tableIns.reload({
                page: {
                    curr: 1 //重新从第 1 页开始
                }
            });
        }
    };
    //添加商品分类
    var addlDlg = function () {
        var showDetail = function (data) {
            var title = "添加商品分类";
            var addlayer=layer.open({
                title: title,
                area: ["500px", "300px"],
                type: 1,
                btn: ['取消'],
                content: $('#divAdd'),
                success: function () {
                },
                end: tableIns
            });
            var url = "/Category/Add";
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
    table.on('edit(test)', function (obj) {
        var value = obj.value //得到修改后的值
            , datas = obj.data //得到所在行所有键值
            , field = obj.field; //得到字段
        var url = "/Category/UpdateCategoryName";
        if (obj.value != '') {
            $.post(url, datas, function (data) {
                layer.msg(data.message);
                layer.closeAll();
            }, "json");
            return false;
        }
    });
    //监听提交
    form.on('switch(switchTest)', function (data) {
        var elem = $(data.elem);
        var x = data.elem.checked;
        var content = "确定不显示吗？";
        if (x == true) {
            content = "确认显示吗？";
        }
        layer.open({
            content: content
            , btn: ['确定', '取消']
            , yes: function (index, layero) {
                data.elem.checked = x;
                $.post('/Category/IsShow', { isShow: data.elem.checked, id: elem.data("id") },
                    function (data) {
                        if (data.result == true) {
                            layer.msg(data.message);
                            location.reload();
                            //if (callback != undefined) callback();
                        } else { layer.msg(data.message); }
                    }, "json"
                );
                form.render();
                layer.close(index);
                //按钮【按钮一】的回调
            }
            , btn2: function (index, layero) {
                //按钮【按钮二】的回调
                data.elem.checked = !x;
                form.render();
                layer.close(index);
                //return false 开启该代码可禁止点击该按钮关闭
            }
            , cancel: function () {
                //右上角关闭回调
                data.elem.checked = !x;
                form.render();
                //return false 开启该代码可禁止点击该按钮关闭
            }
        });
        return false;
    })
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    //监听页面主按钮操作
    //事件
    var active = {
        delete: function () {
            var checkStatus = table.checkStatus('demo')
                , data = checkStatus.data; //得到选中的数据

            if (data.length === 0) {
                return layer.msg('请选择数据');
            }
            if (data.length > 1) {
                return layer.msg('一次只能删除一条数据');
            }
            alert(JSON.stringify(checkData));
            layer.confirm('确定删除吗？', function (index) {
                var url = "/Category/Delete";
                $.post(url, data[0], function (data) {
                    layer.msg(data.message);
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    tableIns.reload();
                }, "json");
                return false;
            });
        }
        ,add: function () {  //添加
            addlDlg.add();
        }
    };

});