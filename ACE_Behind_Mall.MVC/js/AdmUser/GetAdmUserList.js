//layui.config({
//    base: "/layuiadmin/layui/modules/"
//}).use(['layer', 'jquery', 'table'], function () {
///*    var*/ /*form = layui.form(),*/
//        var layer = layui.layer,
//        $ = layui.jquery;
//    var table = layui.table;
//    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER  
//    var config = {};  //table的参数，如搜索key，点击tree的id
//    var mainList = function (options) {
//        if (options != undefined) {
//            $.extend(config, options);
//        }
//        table.reload('mainList', {
//            url: '/AdminUser/GetAdmUserList',
//            where: config
//        });
//    }
//})
layui.config({
    base: "/js/"
}).use(['form', 'table','laydate', 'vue','layer'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery;
    var laydate = layui.laydate;
    var table = layui.table;
    var tableIns = table.reload('mainList', {
        url: '/AdminUser/GetAdmUserList'
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
    });
    var $ = layui.$, active = {
        reload: function () {
            var inputReload = $('#inputReload');
            console.log(inputReload.val());
            //执行重载
            tableIns.reload({
                page: {
                    curr: 1 //重新从第 1 页开始
                }
                , where: {
                    key: inputReload.val()
                }
            });
        }
    };
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    //查看员工信息
    var detailDlg = function () {
        var vm1 = new Vue({
            el: "#formDetail"
        });
        var showDetail = function (data) {
            var title = "查看详情";
            layer.open({
                title: title,
                area: ["800px", "600px"],
                type: 1,
                btn: [ '取消'],
                content: $('#divDetail'),
                success: function () {
                    //alert(JSON.stringify(data));
                    vm1.$set('$data', data);
                    $(":radio[name='Sex'][value='" + data.Sex + "']").prop("checked", "checked");
                    $("#Birthday").val(data.Birthday);
                    //$("#CreateTime").val(data.CreateTime);
                    form.render();
                    tableIns.reload();
                },
                end: tableIns
            });
        }
        return {
            detail: function (data) {
                showDetail(data);
            },
        };
    }();
    //修改员工信息
    var updateDlg = function () {
        var vm= new Vue({
            el: "#formDetail"
        });
        var showUpdate = function (data) {
            var title ="编辑信息";
            layer.open({
                title: title,
                area: ["800px", "600px"],
                type: 1,
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    var url = "/AdminUser/Update";
                    //提交数据
                    form.on('submit(formSubmit)',
                        function (data) {
                            $.post(url,
                                data.field,
                                function (data) {
                                    layer.msg(data.message);
                                },
                                "json");
                            return false;
                        });
                    $('#formSubmit').trigger('click');
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    tableIns.reload();
                },
                content: $('#divDetail'),
                success: function () {
                    //alert(JSON.stringify(data));
                    vm.$set('$data', data);
                    $(":radio[name='Sex'][value='" + data.Sex + "']").prop("checked", "checked");
                    $("#Birthday").val(data.Birthday);
                    //$("#CreateTime").val(data.CreateTime);
                    form.render();
                    tableIns.reload();
                },
                end: tableIns
            });
            var url = "/RecRecruit/Detail";
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    $.post(url,
                        data.field,
                        function (data) {
                            layer.msg(data.Message);
                        },
                        "json");
                    return false;
                });
        }
        return {
            update: function (data) {
                showUpdate(data);
            },
        };
    }();
    //自定义日期格式
    var date = $("#Birthday").val();
    laydate.render({
        elem: '#Birthday'
        , format: 'yyyy-MM-dd'
        ,value:date  //可任意组合
    });
    //监听表格内部按钮
    table.on('tool(list)', function (obj) {
        var data = obj.data;
        if (obj.event === 'detail') {      //查看
            detailDlg.detail(data);
        }
        if (obj.event === 'update') {
            updateDlg.update(data);
        }
    });
    //事件
    var active = {
        delete: function () {
            var checkStatus = table.checkStatus('mainList')
                , data = checkStatus.data; //得到选中的数据

            if (data.length === 0) {
                return layer.msg('请选择数据');
            }
            if (data.length >1) {
                return layer.msg('一次只能删除一条数据');
            }
            layer.confirm('确定删除吗？', function (index) {
                //alert(JSON.stringify(checkData));
                var url = "/AdminUser/Delete";
                $.post(url,data[0], function (data) {
                        layer.msg(data.message);
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    tableIns.reload();
                    }, "json");
                    return false;
                });
        }
        , add: function () {
            layer.open({
                title: "添加员工",
                area: ["800px", "600px"],
                type: 1,
                btn: ['确定', '取消'],
                yes: function (index, layero) {
                    var url = "/AdminUser/Add";
                    //提交数据
                    form.on('submit(formSubmit1)',
                        function (data) {
                            $.post(url,
                                data.field,
                                function (data) {
                                    layer.msg(data.message);
                                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                                    tableIns.reload(); 
                                },
                                "json");
                            return false;
                        });
                    $('#formSubmit1').trigger('click');
                    
                },
                content: $('#divAdd'),
            });
        }
    };
});