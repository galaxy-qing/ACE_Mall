layui.config({
    base: "/layuiadmin/layui/modules/"
}).use(['layer', 'jquery', 'table'], function () {
/*    var*/ /*form = layui.form(),*/
        var layer = layui.layer,
        $ = layui.jquery;
    var table = layui.table;
    var toplayer = (top == undefined || top.layer === undefined) ? layer : top.layer;  //顶层的LAYER  
    var config = {};  //table的参数，如搜索key，点击tree的id
    var mainList = function (options) {
        if (options != undefined) {
            $.extend(config, options);
        }
        table.reload('mainList', {
            url: '/AdminUser/GetAdmUserList',
            where: config
        });
    }
})