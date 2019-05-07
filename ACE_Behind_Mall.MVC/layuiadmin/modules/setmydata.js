
layui.define(['table', 'form', 'vue', 'element', 'jquery'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;
    var vm = new Vue({
        el: "#formInfo",
        data: function () {
            return {
                RoleName: '',
                Account: '',
                ReallyName: '',
                Sex: '',
                Phone: '',
                Email: '',
                Birthday: '',
            }
        }
    });
    $(function () {
        $.ajax({
            url: '/SetMyData/GetMyInfo',
            dataType: 'json',
            type: 'get',
            success: function (res) {
                console.log(res.data)
                vm.$set('$data', res.data);
                $(":radio[name='Sex'][value='" + res.data.Sex + "']").prop("checked", "checked");
                $("#Birthday").val(res.data.Birthday);
                form.render();//菜单渲染 把内容加载进去
            }
        });
    });
    form.on('submit(setmyinfo)',
        function (data) {
            $.ajax({
                url: '/SetMyData/UpdateMyInfo',
                method: 'POST',
                data: data.field,
                success: function (data) {
                    layer.msg("保存成功");
                    form.render();
                    //window.location.href='/Good/GoodList'
                },
            })
            return false;
        });
exports('order', {})
});