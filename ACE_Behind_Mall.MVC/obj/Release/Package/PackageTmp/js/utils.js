
layui.define("jquery", function (exports) {
    var jQuery = layui.jquery,
        $ = layui.jquery;

    //获取url的参数值
    $.getUrlParam = function (name) {
        var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
        var r = window.location.search.substr(1).match(reg);
        if (r != null) return unescape(r[2]); return null;
    }

    //把name/value的数组转为obj对象
    $.arrayToObj = function (array) {
        var result = {};
        for (var i = 0; i < array.length; i++) {
            var field = array[i];
            if (field.name in result) {
                result[field.name] += ',' + field.value;
            } else {
                result[field.name] = field.value;
            }
        }
        return result;
    }

    //加载菜单
    $.fn.extend({
        loadMenus: function (modulecode) {
            var dom = $(this);
            $.ajax("/ModuleManager/LoadAuthorizedMenus?modulecode=" + modulecode,
           {
               async: false
               , success: function (data) {
                   var obj = JSON.parse(data);
                   var sb = '';
                   $.each(obj,
                       function () {
                           var element = this;
                           sb += ("<button " + " data-type='" + element.DomId +
                               "' " + " class='layui-btn layui-btn-small " + element.Class +
                               "' " + element.Attr + ">");

                           if (element.Icon != null && element.Icon != '') {
                               sb += ("<i class='layui-icon'>" + element.Icon + "</i>");
                           }
                           sb += (element.Name + "</button>");
                           if (element.Name === '搜索') { //增加搜索输入框和重置按钮
                              
                               sb += '&nbsp;	  <input type="text" name="txtsearch"  placeholder="请输入搜索条件" id="key" autocomplete="off" class="layui-search" style="height:34px;width:250px;">  ';
                               sb += '&nbsp;	  <button data-type="btnRefresh" class="layui-btn">刷新</button> ';
                           }
                       });

                   dom.html(sb);
               }
           });
        }
    });


    exports('utils');
});