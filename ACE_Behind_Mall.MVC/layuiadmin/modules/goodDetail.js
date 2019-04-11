/**

 @Name：layuiAdmin 商品详情
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：GPL-2
    
 */


layui.define(['table', 'form', 'vue', 'element', 'jquery', 'upload', 'layer'], function (exports) {
    var $ = layui.$
        , table = layui.table
        , form = layui.form
        , element = layui.element;
    form.render();
    console.log(getUrlParam('id'))
    function getUrlParam(name) {
        var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)')
        var result = window.location.search.substr(1).match(reg)
        return result ? decodeURIComponent(result[2]) : null
    }
    $(function () {
        var vm = new Vue({
            el: "#formGoodDetail"
        });
        var url = '/Good/GetGoodDetail';
        $.ajax({
            url: url,
            type: "GET",
            data: {
                id: getUrlParam('id')
            },
            success: function (result) {
                var data = JSON.parse(result).data[0]
                // var data = JSON.stringify(eval("(" + result + ")").data);//转换为json对象 
                // var str = data.replace("[,", "").replace("]", "");
                $.ajax({
                    url: '/Category/GetCategoryList',
                    dataType: 'json',
                    success: function (res) {
                        // alert(JSON.stringify(res.data[0].Name));
                        $.each(res.data, function (index, item) {
                            var str = JSON.stringify(item);
                            $('#CategoryID').append(new Option(str.Name, str.ID));//往下拉菜单里添加元素
                            console.log($('#CategoryID').val());
                        })
                        //form.render('select', 'CategoryID')
                        form.render();
                    }
                })
                vm.$set('$data', data);
                $("#CoverImage").attr("src", data.CoverImage);
                var pictureArr = data.InfoImage.split(',');
                $("#InfoImage").empty();
                for (var i = 0; i < pictureArr.length; i++) {
                    $("#InfoImage").append("<img id='InfoImage" + i + "'; name='InfoImage" + i + "' style='width: 200px; height: 120px;margin-left:10px;margin-top:10px;' src=" + pictureArr[i] + ">");
                }
                var pictureArr1 = data.DetailImage.split(',');
                $("#DetailImage").empty();
                for (var i = 0; i < pictureArr1.length; i++) {
                    $("#DetailImage").append("<img id='DetailImage" + i + "'; name='DetailImage" + i + "' style='width: 200px; height: 120px;margin-left:10px;margin-top:10px;' src=" + pictureArr1[i] + ">");
                }
                console.log(data.IsDelete);
                if (data.IsDelete == 0) {
                    vm._data.IsDelete = '未下架'
                }
                if (data.IsDelete == 1) {
                    vm._data.IsDelete = '已下架'
                }
                form.render();
            }
        })
        return false;
    })
    exports('goodDetail', {})
});