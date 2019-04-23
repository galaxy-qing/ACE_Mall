/**

 @Name：layuiAdmin Echarts集成
 @Author：star1029
 @Site：http://www.layui.com/admin/
 @License：GPL-2
    
 */
layui.define(function (exports) {
    layui.use(['index', 'laydate'], function () {
        var laydate = layui.laydate;
        var $ = layui.$
        //年选择器
        laydate.render({
            elem: '#test-laydate-type-year'
            , type: 'year'
            , done: function (value, date, endDate) {
                let year = date.year;
                console.log(year);
                $.get("/Statistics/SelectYear", { selectyear: year }, function (res) {
                    res = JSON.parse(res)
                    if (!res) {
                        layer.msg("查询过程中发生错误！");
                    }
                    if(res.status==0) {
                        //MonthDataBind(data);
                        console.log(res.data);
                    }
                })
            }
        });
        //年月选择器
        laydate.render({
            elem: '#test-laydate-type-month'
            , type: 'month'
        });
    })
    //区块轮播切换
    layui.use(['admin', 'carousel'], function () {
        var $ = layui.$
            , admin = layui.admin
            , carousel = layui.carousel
            , element = layui.element
            , device = layui.device();

        //轮播切换
        $('.layadmin-carousel').each(function () {
            var othis = $(this);
            carousel.render({
                elem: this
                , width: '100%'
                , arrow: 'none'
                , interval: othis.data('interval')
                , autoplay: othis.data('autoplay') === true
                , trigger: (device.ios || device.android) ? 'click' : 'hover'
                , anim: othis.data('anim')
            });
        });
        element.render('progress');
    });
    //折线图
    layui.use(['carousel', 'echarts'], function () {
        var $ = layui.$
            , carousel = layui.carousel
            , echarts = layui.echarts;
        //堆积面积图
        var echheaparea = [], heaparea = [
            {
                tooltip: {
                    trigger: 'axis'
                },
                legend: {
                    data: ['无畏系列', 'A.C.E.LOGO系列', 'CUBAN系列', 'TENNIS系列']//系列名称
                },
                calculable: true,
                xAxis: [
                    {
                        type: 'category',
                        boundaryGap: false,
                        data: ['一月', '二月', '周三', '周四', '周五', '周六', '周日'] //月份
                    }
                ],
                yAxis: [
                    {
                        type: 'value'
                    }
                ],
                series: [
                    {
                        name: '无畏系列',//系列名字
                        type: 'line',
                        stack: '总量',
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: [120, 132, 101, 134, 90, 230, 210]//值
                    },
                    {
                        name: 'A.C.E.LOGO系列',
                        type: 'line',
                        stack: '总量',
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: [220, 182, 191, 234, 290, 330, 310]
                    },
                    {
                        name: 'CUBAN系列',
                        type: 'line',
                        stack: '总量',
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: [150, 232, 201, 154, 190, 330, 410]
                    },
                    {
                        name: 'TENNIS系列',
                        type: 'line',
                        stack: '总量',
                        itemStyle: { normal: { areaStyle: { type: 'default' } } },
                        data: [820, 932, 901, 934, 1290, 1330, 1320]
                    }
                ]
            }
        ]
            , elemheaparea = $('#LAY-index-heaparea').children('div')
            , renderheaparea = function (index) {
                echheaparea[index] = echarts.init(elemheaparea[index], layui.echartsTheme);
                echheaparea[index].setOption(heaparea[index]);
                window.onresize = echheaparea[index].resize;
            };
        if (!elemheaparea[0]) return;
        renderheaparea(0);
    });



    exports('goodsenior', {})

});