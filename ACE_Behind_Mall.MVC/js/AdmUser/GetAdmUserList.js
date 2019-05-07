layui.config({
    base: "/js/"
}).use(['form', 'table', 'jquery', 'laydate', 'vue', 'layer', 'upload'], function () {
    var form = layui.form,
        layer = layui.layer,
        upload = layui.upload,
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
        var vm = new Vue({
            el: "#formDetail"
        });
        var update = false;  //是否为更新
        var look = false;//是否为查看
        var showDetail = function (data) {
            var title = update ? "编辑信息" : "添加";
            layer.open({
                title: title,
                area: ["800px", "600px"],
                type: 1,
                btn: ['取消'],
                content: $('#divDetail'),
                success: function () {
                    //alert(JSON.stringify(data));
                    vm.$set('$data', data);
                    $.ajax({
                        url: '/AdminUser/GetRoleList',
                        dataType: 'json',
                        type: 'post',
                        success: function (data) {
                            $.each(data, function (index, item) {
                                $('#RoleID').append(new Option(item.Name,item.ID));//往下拉菜单里添加元素
                            })

                            form.render();//菜单渲染 把内容加载进去
                        }
                    });
                    $(":radio[name='Sex'][value='" + data.Sex + "']").prop("checked", "checked");
                    $("#Birthday").val(data.Birthday);
                    //$("#CreateTime").val(data.CreateTime);
                    $("#divSubmit").show();
                    if (look == true) {
                        $("#divSubmit").hide();
                    }   
                    form.render();
                    //tableIns.reload();
                },
                end: tableIns
            });
            var url = "/AdminUser/Add";
            if (update) {
                url = "/AdminUser/Update";
            }
            //var url = "/AdminUser/Update";
            //alert(url);
            //提交数据
            form.on('submit(formSubmit)',
                function (data) {
                    $.post(url,
                        data.field,
                        function (data) {
                            layer.msg(data.message);
                            layer.closeAll();
                            tableIns.reload();
                        },
                        "json");
                    return false;
                });
          
        }
        return {
            add: function () { //弹出添加
                look = false;
                update = false;
                showDetail({
                    ID: ''
                });
            },
            update: function (data) { //弹出编辑框
                look = false;
                update = true;
                showDetail(data);
            }
            ,
            detail: function (data) { //查看编辑框
                look = true;
                showDetail(data);
            }
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
            detailDlg.update(data);
        }
    });
    form.on('select(RoleID)', function (data) {
        console.log(data.value); //得到被选中的值
        $.post("AdminUser/GetRoleList" + data.value, function (res) {
            if (res.success) {
                layer.msg(res.message);
            }
        });
    })
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
            alert(JSON.stringify(checkData));
            layer.confirm('确定离职该员工吗？', function (index) {
                var url = "/AdminUser/Delete";
                $.post(url,data[0], function (data) {
                        layer.msg(data.message);
                    layer.close(index); //如果设定了yes回调，需进行手工关闭
                    tableIns.reload();
                    }, "json");
                    return false;
                });
        }
        , add: function () {  //添加
            detailDlg.add();
        } 
    };
});