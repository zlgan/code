// pages/statistics/index.js
const storage = require('../../utils/storage.js');
const util = require('../../utils/util.js');
const wxCharts = require('../../utils/wxcharts.js');

let trendChart = null;

Page({
  data: {
    startDate: '',
    endDate: '',
    activeTab: 'product',
    analysisType: 'amount',
    trendType: 'amount',
    inboundStats: {
      totalAmount: '0.00',
      totalCount: 0,
      averagePrice: '0.00'
    },
    outboundStats: {
      totalAmount: '0.00',
      totalCount: 0,
      averagePrice: '0.00'
    },
    productRankList: [],
    trendData: {
      categories: [],
      inboundSeries: [],
      outboundSeries: []
    }
  },

  onLoad() {
    // 设置默认日期范围为近6个月
    const now = new Date();
    const endDate = this.formatMonth(now);
    const startDate = this.formatMonth(new Date(now.setMonth(now.getMonth() - 5)));

    this.setData({
      startDate,
      endDate
    }, () => {
      this.loadData();
    });
  },

  // 格式化月份
  formatMonth(date) {
    const year = date.getFullYear();
    const month = (date.getMonth() + 1).toString().padStart(2, '0');
    return `${year}-${month}`;
  },

  // 选择开始月份
  onStartDateChange(e) {
    this.setData({
      startDate: e.detail.value
    }, () => {
      this.loadData();
    });
  },

  // 选择结束月份
  onEndDateChange(e) {
    this.setData({
      endDate: e.detail.value
    }, () => {
      this.loadData();
    });
  },

  // 切换分析维度
  switchAnalysisType(e) {
    const type = e.currentTarget.dataset.type;
    this.setData({
      analysisType: type
    }, () => {
      this.calculateProductRank();
    });
  },

  // 切换趋势类型
  switchTrendType(e) {
    const type = e.currentTarget.dataset.type;
    this.setData({
      trendType: type
    }, () => {
      this.drawTrendChart();
    });
  },

  // 切换标签页
  switchTab(e) {
    const tab = e.currentTarget.dataset.tab;
    this.setData({
      activeTab: tab
    }, () => {
      if (tab === 'trend') {
        this.drawTrendChart();
      }
    });
  },

  // 加载数据
  loadData() {
    const { startDate, endDate } = this.data;

    // 获取入库和出库记录
    const inboundRecords = storage.getData('inboundRecords') || [];
    const outboundRecords = storage.getData('outboundRecords') || [];

    // 筛选日期范围内的记录
    const filteredInbound = inboundRecords.filter(record => {
      const month = record.date.substring(0, 7);
      return month >= startDate && month <= endDate;
    });

    const filteredOutbound = outboundRecords.filter(record => {
      const month = record.date.substring(0, 7);
      return month >= startDate && month <= endDate;
    });

    // 计算统计数据
    const inboundStats = this.calculateStats(filteredInbound);
    const outboundStats = this.calculateStats(filteredOutbound);

    this.setData({
      inboundStats,
      outboundStats
    });

    // 计算产品排名
    this.calculateProductRank(filteredInbound, filteredOutbound);

    // 计算趋势数据
    this.calculateTrendData(filteredInbound, filteredOutbound);
  },

  // 计算统计数据
  calculateStats(records) {
    if (!records.length) {
      return {
        totalAmount: '0.00',
        totalCount: 0,
        averagePrice: '0.00'
      };
    }

    const totalAmount = records.reduce((sum, record) => sum + record.amount, 0);
    const totalQuantity = records.reduce((sum, record) => sum + record.quantity, 0);

    return {
      totalAmount: totalAmount.toFixed(2),
      totalCount: records.length,
      averagePrice: (totalAmount / totalQuantity).toFixed(2)
    };
  },

  // 计算产品排名
  calculateProductRank(inboundRecords = [], outboundRecords = []) {
    const { analysisType } = this.data;
    const productStats = new Map();

    // 合并入库和出库记录的产品统计
    const processRecord = (record, isInbound) => {
      const { product, quantity, amount } = record;
      const key = product.code;
      
      if (!productStats.has(key)) {
        productStats.set(key, {
          code: product.code,
          name: product.name,
          unit: product.unit,
          quantity: 0,
          amount: 0
        });
      }

      const stats = productStats.get(key);
      stats.quantity += quantity;
      stats.amount += amount;
    };

    inboundRecords.forEach(record => processRecord(record, true));
    outboundRecords.forEach(record => processRecord(record, false));

    // 转换为数组并排序
    let productRankList = Array.from(productStats.values())
      .sort((a, b) => {
        const valueA = analysisType === 'amount' ? a.amount : a.quantity;
        const valueB = analysisType === 'amount' ? b.amount : b.quantity;
        return valueB - valueA;
      })
      .slice(0, 10);

    // 计算百分比
    const total = productRankList.reduce((sum, item) => {
      return sum + (analysisType === 'amount' ? item.amount : item.quantity);
    }, 0);

    productRankList = productRankList.map(item => ({
      ...item,
      amount: item.amount.toFixed(2),
      percentage: ((analysisType === 'amount' ? item.amount : item.quantity) / total * 100).toFixed(1)
    }));

    this.setData({ productRankList });
  },

  // 计算趋势数据
  calculateTrendData(inboundRecords, outboundRecords) {
    const { startDate, endDate } = this.data;
    const months = this.getMonthsBetween(startDate, endDate);
    
    // 初始化每月数据
    const monthlyData = new Map(months.map(month => [month, {
      inbound: { amount: 0, count: 0 },
      outbound: { amount: 0, count: 0 }
    }]));

    // 统计入库数据
    inboundRecords.forEach(record => {
      const month = record.date.substring(0, 7);
      const data = monthlyData.get(month);
      if (data) {
        data.inbound.amount += record.amount;
        data.inbound.count += 1;
      }
    });

    // 统计出库数据
    outboundRecords.forEach(record => {
      const month = record.date.substring(0, 7);
      const data = monthlyData.get(month);
      if (data) {
        data.outbound.amount += record.amount;
        data.outbound.count += 1;
      }
    });

    // 转换为图表数据格式
    const trendData = {
      categories: months.map(month => month.substring(5)), // 只显示月份
      inboundSeries: months.map(month => {
        const data = monthlyData.get(month);
        return this.data.trendType === 'amount' ? 
          Number(data.inbound.amount.toFixed(2)) : 
          data.inbound.count;
      }),
      outboundSeries: months.map(month => {
        const data = monthlyData.get(month);
        return this.data.trendType === 'amount' ? 
          Number(data.outbound.amount.toFixed(2)) : 
          data.outbound.count;
      })
    };

    this.setData({ trendData }, () => {
      if (this.data.activeTab === 'trend') {
        this.drawTrendChart();
      }
    });
  },

  // 获取两个日期之间的月份列表
  getMonthsBetween(startDate, endDate) {
    const months = [];
    let current = new Date(startDate);
    const end = new Date(endDate);
    
    while (current <= end) {
      months.push(this.formatMonth(current));
      current.setMonth(current.getMonth() + 1);
    }
    
    return months;
  },

  // 绘制趋势图表
    drawTrendChart() {
    const { trendData, trendType } = this.data;

    if (trendChart) {
      trendChart.updateData({
        categories: trendData.categories,
        series: [{
          name: '入库',
          data: trendData.inboundSeries,
          format: (val) => trendType === 'amount' ? '¥' + val : val
        }, {
          name: '出库',
          data: trendData.outboundSeries,
          format: (val) => trendType === 'amount' ? '¥' + val : val
        }]
      });
      return;
    }

    const windowWidth = wx.getSystemInfoSync().windowWidth;
    trendChart = new wxCharts({
      canvasId: 'trendChart',
      type: 'line',
      categories: trendData.categories,
      series: [{
        name: '入库',
        data: trendData.inboundSeries,
        format: (val) => trendType === 'amount' ? '¥' + val : val
      }, {
        name: '出库',
        data: trendData.outboundSeries,
        format: (val) => trendType === 'amount' ? '¥' + val : val
      }],
      yAxis: {
        title: trendType === 'amount' ? '金额' : '单数',
        format: (val) => trendType === 'amount' ? '¥' + val : val,
        min: 0
      },
      width: windowWidth - 60,
      height: 250,
      dataLabel: false,
      dataPointShape: true,
      legend: true,
      extra: {
        lineStyle: 'curve'
      }
    });
  }
});