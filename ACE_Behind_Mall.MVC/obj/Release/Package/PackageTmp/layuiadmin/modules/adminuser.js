/**

 @Name：ACE-MALL 商城后台
 @Author：张青青
    
 */
layui.define(['table', 'form', 'laydate', 'vue', 'jquery'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , laydate = layui.laydate;
    var tableIns=table.render({
        elem: '#adminUserList'
        , url: '/AdminUser/GetAdmUserList' //模拟接口
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
            , { field: 'RoleName', title: '角色', align: 'center', sort: true }
            //, { field: 'Image',title:'头像', align: 'center' }
            , { field: 'ReallyName', title: '真实姓名', align: 'center' }
            , { field: 'Account', title: '账号', align: 'center', sort: true }
            , { field: 'Phone', title: '电话', align: 'center' }
            , { field: 'Email', title: '邮箱', align: 'center' }
            , { field: 'Birthday', title: '出生日期', align: 'center', sort: true}
            , { field: 'Sex',title:'性别', align: 'center', sort: true }
            , { field: 'CreateTime',title:'加入时间', align: 'center' }
            , { title: '操作', align: 'center', fixed: 'right', toolbar: '#barList' }
        ]]
        , page: true
        , limit: 10
        , limits: [10, 15, 20, 25, 30]
        , text: {
            none: '暂无相关数据' //默认：无数据。
        }
    });
    $('.demoTable .layui-btn').on('click', function () {
        var type = $(this).data('type');
        active[type] ? active[type].call(this) : '';
    });
    var vm = new Vue({
        el: "#formDetail",
        data: function () {
            return {
                RoleID: '',
                ReallyName: '',
                Account: '',
                Phone: '',
                Email: '',
                Birthday: '',
                Sex: '',
                CreateTime: '',
                //IsDelete: '',
            }
        },
        methods: {
            removeImg: function (index) {
                this.DetailImage.splice(index, 1)
            },
            removeImg1: function (index) {
                this.InfoImage.splice(index, 1)
            }
        }
    });
    //查看员工信息
    var detailDlg = function () {
        //var vm = new Vue({
        //    el: "#formDetail"
        //});
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
                    if (!(update == false && look == false)) {
                        vm.$set('$data', data);
                        vm._data.Birthday = data.Birthday.substr(0, 10);
                    }
                    if ((update == false && look == false)) {
                        vm.$set('$data', {
                            RoleID: '',
                            ReallyName: '',
                            Account: '',
                            Phone: '',
                            Email: '',
                            Birthday: '',
                            Sex: '',
                            CreateTime: '',
                            //IsDelete: '',
                        })
                    }
                    $.ajax({
                        url: '/Role/GetRoleList',
                        dataType: 'json',
                        success: function (res) {
                            var html = ''
                            res.data.forEach(function (item, index) {
                                html += "<option  value='" + item.ID + "'>" + item.Name + "</option>"
                            })
                            $("#RoleID").append(html)
                            $("#RoleID")[0].selectedIndex = data.RoleID;
                            form.render('select')
                        }
                    });
                    $("#divSubmit").show();
                    if (look == true) {
                        $("#divSubmit").hide();
                    }
                    form.render();
                },
                end: tableIns
            });
            var url = "/AdminUser/Add";
            if (update) {
                url = "/AdminUser/Update";
            }
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
    laydate.render({
        elem: '#Birthday'
        , format: 'yyyy-MM-dd'
    });
    //监听表格内部按钮
    table.on('tool(adminUserList)', function (obj) {
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
            var checkStatus = table.checkStatus('adminUserList')
                , data = checkStatus.data; //得到选中的数据

            if (data.length === 0) {
                return layer.msg('请选择数据');
            }
            if (data.length > 1) {
                return layer.msg('一次只能操作一条数据');
            }
            layer.confirm('确定离职该员工吗？', function (index) {
                var url = "/AdminUser/Delete";
                $.post(url, data[0], function (data) {
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
    exports('adminuser', {})
});