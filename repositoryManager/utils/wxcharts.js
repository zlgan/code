/*
 * charts for WeChat small app v1.0
 *
 * https://github.com/xiaolin3303/wx-charts
 * 2016-11-28
 *
 * Designed and built with all the love of Web
 */

'use strict';

var config = {
    yAxisWidth: 30,
    yAxisSplit: 5,
    xAxisHeight: 30,
    xAxisLineHeight: 30,
    legendHeight: 30,
    yAxisTitleWidth: 30,
    padding: 12,
    columePadding: 3,
    fontSize: 10,
    dataPointShape: ['circle', 'diamond', 'triangle', 'rect'],
    colors: ['#7cb5ec', '#f7a35c', '#434348', '#90ed7d', '#f15c80', '#8085e9'],
    pieChartLinePadding: 25,
    pieChartTextPadding: 15,
    xAxisTextPadding: 3,
    titleColor: '#333333',
    titleFontSize: 20,
    subtitleColor: '#999999',
    subtitleFontSize: 15,
    toolTipPadding: 3,
    toolTipBackground: '#000000',
    toolTipOpacity: 0.7,
    toolTipLineHeight: 14,
    radarGridCount: 3,
    radarLabelTextMargin: 15
};

// 生成随机id
function generateUUID() {
    var d = new Date().getTime();
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = (d + Math.random() * 16) % 16 | 0;
        d = Math.floor(d / 16);
        return (c == 'x' ? r : (r & 0x3 | 0x8)).toString(16);
    });
}

function drawLineChart(opts, context) {
    var data = opts.data;
    var chartData = {
        xAxisPoints: [],
        calPoints: [],
        originalData: data.series
    };

    // 计算图表数据
    var spacingValid = opts.width - 2 * config.padding - config.yAxisWidth;
    var eachSpacing = spacingValid / (data.categories.length - 1);
    var points = [];
    var minRange = data.yAxis.min || 0;
    var maxRange = data.yAxis.max || Math.max.apply(null, data.series[0].data);
    var range = maxRange - minRange;
    var validHeight = opts.height - 2 * config.padding - config.xAxisHeight - config.legendHeight;

    // 绘制坐标轴
    context.beginPath();
    context.setStrokeStyle("#cccccc");
    context.setLineWidth(1);
    context.moveTo(config.padding + config.yAxisWidth, opts.height - config.padding - config.xAxisHeight);
    context.lineTo(opts.width - config.padding, opts.height - config.padding - config.xAxisHeight);
    context.stroke();
    context.closePath();

    // 绘制折线图
    data.series.forEach(function(serie, seriesIndex) {
        var color = serie.color || config.colors[seriesIndex];
        context.beginPath();
        context.setStrokeStyle(color);
        context.setLineWidth(2);
        
        serie.data.forEach(function(item, index) {
            var x = config.padding + config.yAxisWidth + index * eachSpacing;
            var y = opts.height - config.padding - config.xAxisHeight - (item - minRange) / range * validHeight;
            
            if (index === 0) {
                context.moveTo(x, y);
            } else {
                context.lineTo(x, y);
            }
            
            points.push([x, y]);
            chartData.xAxisPoints.push(x);
        });
        
        context.stroke();
        context.closePath();
        
        // 绘制数据点
        points.forEach(function(item) {
            context.beginPath();
            context.setFillStyle(color);
            context.arc(item[0], item[1], 2, 0, 2 * Math.PI);
            context.fill();
            context.closePath();
        });
    });

    // 绘制X轴文字
    data.categories.forEach(function(item, index) {
        var x = config.padding + config.yAxisWidth + index * eachSpacing;
        context.beginPath();
        context.setFontSize(config.fontSize);
        context.setFillStyle('#666666');
        context.fillText(item, x - measureText(item) / 2, opts.height - config.padding - config.xAxisHeight + config.fontSize + 5);
        context.closePath();
    });

    // 绘制Y轴
    for (var i = 0; i <= config.yAxisSplit; i++) {
        var value = minRange + range / config.yAxisSplit * i;
        var y = opts.height - config.padding - config.xAxisHeight - value / range * validHeight;
        context.beginPath();
        context.setFontSize(config.fontSize);
        context.setFillStyle('#666666');
        context.fillText(value.toFixed(0), config.padding + config.yAxisWidth - measureText(value.toFixed(0)) - 5, y + config.fontSize / 2);
        context.stroke();
        context.closePath();
    }

    context.draw();
    return chartData;
}

function measureText(text) {
    var fontSize = arguments.length > 1 && arguments[1] !== undefined ? arguments[1] : config.fontSize;
    text = String(text);
    var text = text.split('');
    var width = 0;
    text.forEach(function(item) {
        if (/[a-zA-Z]/.test(item)) {
            width += 7;
        } else if (/[0-9]/.test(item)) {
            width += 5.5;
        } else if (/\./.test(item)) {
            width += 2.7;
        } else if (/-/.test(item)) {
            width += 3.25;
        } else if (/[\u4e00-\u9fa5]/.test(item)) {
            width += 10;
        } else if (/\(|\)/.test(item)) {
            width += 3.73;
        } else if (/\s/.test(item)) {
            width += 2.5;
        } else if (/%/.test(item)) {
            width += 8;
        } else {
            width += 10;
        }
    });
    return width * fontSize / 10;
}

var Charts = function(opts) {
    opts.title = opts.title || {};
    opts.subtitle = opts.subtitle || {};
    opts.yAxis = opts.yAxis || {};
    opts.xAxis = opts.xAxis || {};
    opts.extra = opts.extra || {};
    opts.legend = opts.legend === false ? false : true;
    opts.animation = opts.animation === false ? false : true;
    
    var context = wx.createCanvasContext(opts.canvasId);
    
    if (opts.type === 'line') {
        this.chartData = drawLineChart(opts, context);
    }
    
    this.event = function(e) {
        if (this.chartData.xAxisPoints && e.type === 'tap') {
            var touch = e.touches[0];
            var currentOffset = Math.abs(touch.x - this.chartData.xAxisPoints[0]);
            var currentIndex = -1;
            
            this.chartData.xAxisPoints.forEach(function(item, index) {
                var offset = Math.abs(touch.x - item);
                if (offset < currentOffset) {
                    currentOffset = offset;
                    currentIndex = index;
                }
            });
            
            if (currentIndex > -1) {
                var currentData = [];
                this.chartData.originalData.forEach(function(item) {
                    currentData.push({
                        text: item.name,
                        value: item.data[currentIndex]
                    });
                });
                
                if (opts.onTooltipFormat) {
                    opts.onTooltipFormat(currentData);
                }
            }
        }
    };
};

module.exports = Charts;