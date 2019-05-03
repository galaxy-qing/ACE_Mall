layui.config({
    base: "/layuiadmin/lib/"
}).use(['admin', 'carousel', 'echarts', 'index', 'laydate'], function () {
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
    //堆积条形图
    var echheapbar = [], heapbar = [
        {
            tooltip: {
                trigger: 'axis',
                axisPointer: {            // 坐标轴指示器，坐标轴触发有效
                    type: 'shadow'        // 默认为直线，可选为：'line' | 'shadow'
                }
            },
            legend: {
                data: []
            },
            calculable: true,
            xAxis: [
                {
                    type: 'value'
                }
            ],
            yAxis: [
                {
                    type: 'category',
                    data: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'] //月份
                }
            ],
            series: []
        }
    ]
        , elemheapbar = $('#LAY-index-heapbar').children('div')
        , renderheapbar = function (index) {
            echheapbar[index] = echarts.init(elemheapbar[index], layui.echartsTheme);
            echheapbar[index].setOption(heapbar[index]);
            window.onresize = echheapbar[index].resize;
        };
    if (!elemheapbar[0]) return;
    renderheapbar(0);
    function defaultSelect(year) {
        $.get("/Statistics/SelectSaleYear", { selectyear: year }, function (res) {
            res = JSON.parse(res)
            if (!res) {
                layer.msg("查询过程中发生错误！");
            }
            if (res.status == 0) {
                var baseSeries = {
                    type: 'bar',
                    stack: '总量',
                    itemStyle: { normal: { label: { show: true, position: 'insideRight' } } },
                }
                var seriesArr = [];
                res.data.series.forEach(function (item) {
                    $.extend(item, baseSeries);
                    seriesArr.push(item);
                })
                heapbar[0].legend.data = res.data.category
                heapbar[0].series = seriesArr;
                renderheapbar(0);
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
    ////年月选择器
    //laydate.render({
    //    elem: '#test-laydate-type-month'
    //    , type: 'month'
    //});

});