layui.config({
    base: "/layuiadmin/lib/"
}).use(['admin', 'carousel', 'echarts', 'index', 'laydate'], function ()  {
    var laydate = layui.laydate;
    var $ = layui.$
        , admin = layui.admin
        , carousel = layui.carousel
        , element = layui.element
        , device = layui.device()
        , echarts = layui.echarts;
        //获取当前年
     var year = new Date().getFullYear();
     defaultSelect(year);
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
    //堆积面积图
    var echheaparea = [], heaparea = [
        {
            tooltip: {
                trigger: 'axis'
            },
            legend: {
                width: '100%',
                //itemGap: 30,        // 间隔
                data: []
            },
            calculable: true,
            xAxis: [
                {
                    type: 'category',
                    boundaryGap: false,
                    data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'] //月份
                }
            ],
            yAxis: [
                {
                    type: 'value'
                }
            ],
            series: []
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
    function defaultSelect(year) {
        $.get("/Statistics/SelectYear", { selectyear: year }, function (res) {
            res = JSON.parse(res)
            if (!res) {
                layer.msg("查询过程中发生错误！");
            }
            if (res.status == 0) {
                //MonthDataBind(data);
                var baseSeries = {
                    type: 'line',
                    stack: '总量',
                    itemStyle: { normal: { areaStyle: { type: 'default' } } }
                }
                var seriesArr = [];
                res.data.series.forEach(function (item) {
                    $.extend(item, baseSeries);
                    seriesArr.push(item);
                })
                heaparea[0].legend.data = res.data.category
                heaparea[0].series = seriesArr;
                renderheaparea(0);
            }
        })
    }
    //年选择器
    laydate.render({
        elem: '#test-laydate-type-year'
        , type: 'year'
        , done: function (value, date, endDate) {
            let year = date.year;
            defaultSelect(year);
           
        }
    });
    //年月选择器
    laydate.render({
        elem: '#test-laydate-type-month'
        , type: 'month'
    });

});