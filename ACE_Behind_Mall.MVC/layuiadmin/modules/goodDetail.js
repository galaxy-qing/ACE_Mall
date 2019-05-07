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
        ,upload=layui.upload
        , element = layui.element;
    form.render();
    var vm = new Vue({
        el: "#formGoodDetail",
        data: function () {
            return {
                Category: [],
                CategoryID:'',
                Name: '',
                CoverImage: '',
                DetailImage: [],
                InfoImage: [],
                PresentPrice: '',
                OriginalPrice: '',
                SaleNumber: '',
                Stock: '',
                CreateTime: new Date().toLocaleString(),
                IsDelete:'',
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
    function getUrlParam(name) {
        var reg = new RegExp('(^|&)' + name + '=([^&]*)(&|$)')
        var result = window.location.search.substr(1).match(reg)
        return result ? decodeURIComponent(result[2]) : null
    }
    //执行实例
    upload.render({
        elem: '#imageUpload1' //绑定元素
        , url: "/Good/UploadImage1" //上传接口
        , before: function (obj) {
            //预读本地文件示例，不支持ie8
            obj.preview(function (index, file, result) {
                $('#CoverImage').attr('src', result); //图片链接（base64）
            });
        }
        , done: function (res) {
            //如果上传失败
            if (res.status ==1) {
                return layer.msg('上传失败');
            }
            //上传成功
            //$('#CoverImage').val(res.message);
            vm.$set('_data.CoverImage', res.message )
        }
    });
    upload.render({
        elem: '#imageUpload2',
        url: "/Good/UploadImage2",
        choose: function (obj) {
            //将每次选择的文件追加到文件队列
            var files = obj.pushFile();
            obj.preview(function (index, file, result) {
                //console.log(index); //得到文件索引
               //console.log(file.Name); //得到文件对象
               // console.log(result); //得到文件base64编码，比如图片
               // vm._data.DetailImage.push(result);
            });
        }
        , multiple: true
        , done: function (res, index, upload) { //每个文件提交一次触发一次。详见“请求成功的回调”
            vm.$set('_data.DetailImage', res.message)
           // console.log(res)
        }
    });   
    upload.render({
        elem: '#imageUpload3'
        , url: "/Good/UploadImage3"
        , choose: function (obj) {
            $("#InfoImage").empty();
            //将每次选择的文件追加到文件队列
            var files = obj.pushFile();
            obj.preview(function (index, file, result) {
               // console.log(index); //得到文件索引
               // console.log(file.Name); //得到文件对象
               // console.log(result); //得到文件base64编码，比如图片
                //$("#InfoImage").append("<img  name='InfoImage'; style='width: 200px; height: 120px;margin-left:10px;margin-top:10px;' src=" + result + ">");
                //vm._data.InfoImage.push(result);
            });
        }
        , multiple: true
        , done: function (res, index, upload) { //每个文件提交一次触发一次。详见“请求成功的回调”
            vm.$set('_data.InfoImage', res.message)
        }
    });   
    $(function () {
        var url = '/Good/GetGoodDetail';
        $.ajax({
            url: url,
            type: "GET",
            data: {
                id: getUrlParam('id') == null ? 0 : getUrlParam('id')
            },
            success: function (result) {
                var data = JSON.parse(result).data[0]
                $.ajax({
                    url: '/Category/GetCategoryList',
                    dataType: 'json',
                    success: function (res) {
                        var html = ''
                        res.data.forEach(function (item, index) {
                            html += "<option  value='" + item.ID + "'>" + item.Name + "</option>"
                        })
                        $("#CategoryID").append(html)
                        $("#CategoryID")[0].selectedIndex = data.CategoryID;
                        form.render('select')
                    }
                })
                if (getUrlParam('id') != null) {
                    vm.$set('$data', data);
                    vm._data.InfoImage = data.InfoImage.split(',');
                    vm._data.DetailImage = data.DetailImage.split(',');
                    form.render();
                }             
            }
        })
        return false;
    })
    form.on('select(CategoryID)', function (data) {
        vm.$set('_data.CategoryID', data.value)
    }); 
    form.on('submit(formSubmit)',
        function (data) {
            $.ajax({
                url: '/Good/SubmitGoodInfo',
                method: 'POST',
                data: vm._data,
                success: function (data) {
                    layer.msg("保存成功")
                    //window.location.href='/Good/GoodList'
                },
            })
            return false;
        });
   
    exports('goodDetail', {})
});