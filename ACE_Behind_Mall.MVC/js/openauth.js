/**
 * Openauth通用工具库
 * yubaolee @ 2017
 * www.cnblogs.com/yubaolee
 */
layui.define(['jquery', 'layer'], function (exports) {
    var $ = layui.jquery;
    var layer = layui.layer;
    //字符串常量
    var MOD_NAME = 'openauth',
        THIS = 'layui-this',
        SHOW = 'layui-show',
        HIDE = 'layui-hide',
        DISABLED = 'layui-disabled';

    //外部接口
    var openauth = {
        config: {} //全局配置项

        //设置全局项
        , set: function (options) {
            var that = this;
            that.config = $.extend({}, that.config, options);
            return that;
        }
        //事件监听
        , on: function (events, callback) {
            return layui.onevent.call(this, MOD_NAME, events, callback);
        }

        //删除
        , del: function (url, dataids, callback) {
            if (dataids == undefined || dataids == "" || dataids.length == 0) {
                layer.msg("至少选择一条记录");
                return;
            }
            layer.confirm('真的删除么', function (index) {
                $.post(url, { ids: dataids },
                    function (data) {
                        if (data.Code == 200) {
                            if (callback != undefined) callback();
                        } else {
                            layer.msg(data.Message);
                        }
                    }, "json");
                layer.close(index);
            });
        },
        //初始化密码
        init:function(url,dataids,callback){
            if (dataids == undefined || dataids == "" || dataids.length == 0) {
                layer.msg("至少选择一条记录");
                return;
            }
            layer.confirm('您确定要把此账号的密码初始化为【123456】吗？', function (index) {
                $.post(url, { ids: dataids },
                    function (data) {
                        if (data.Code == 200) {
                            if (callback != undefined) callback();
                        } else {
                            layer.msg(data.Message);
                        }
                    }, "json");
                layer.close(index);
            });
        }
        //恢复
        , rec: function (url, dataids, callback) {
            if (dataids == undefined || dataids == "" || dataids.length == 0) {
                layer.msg("至少选择一条记录");
                return;
            }
            layer.confirm('确定要恢复吗？', function (index) {
                $.post(url, { ids: dataids },
                    function (data) {
                        if (data.Code == 200) {
                            if (callback != undefined) callback();
                        } else {
                            layer.msg(data.Message);
                        }
                    }, "json");
                layer.close(index);
            });
        }
    }

    exports(MOD_NAME, openauth);
});